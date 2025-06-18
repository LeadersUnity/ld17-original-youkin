using UnityEngine;
using System.Collections; // コルーチンを使うために必要

public class MainSoundController : MonoBehaviour
{
    // シングルトンインスタンス
    public static MainSoundController instance = null;

    [Tooltip("このフラグがtrueの間、BGMはシーンを跨いでも継続します。")]
    public bool start_b = false; // シーンを跨いで音楽を継続するかどうかのフラグ

    [Tooltip("Inspectorから設定するBGM用のAudioSource。")]
    public AudioSource main_sound; // Inspectorで設定されるAudioSource

    [Tooltip("フェードアウトにかける時間（秒）。")]
    public float fadeOutTime = 2.0f; // フェードアウトにかける時間

    private float originalVolume; // フェードアウト前の元の音量を保持

    void Awake()
    {
        // シングルトンパターンの実装
        if (instance == null)
        {
            instance = this;

            // AudioSourceが設定されているか確認
            if (main_sound == null)
            {
                Debug.LogError("MainSoundController: main_sound (AudioSource) が設定されていません！", this);
                return; // AudioSourceがなければ処理を中断
            }

            // 元の音量を保存（フェードアウト後も元の音量に戻せるように）
            originalVolume = main_sound.volume;

            // ★修正点1: Awake()では常にDontDestroyOnLoadを適用する
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("MainSoundControllerオブジェクトにDontDestroyOnLoadを適用しました。");

            // ★修正点2: Awake()でのBGM再生は、start_bの初期値に従う
            if (start_b)
            {
                if (!main_sound.isPlaying)
                {
                    main_sound.Play();
                    Debug.Log("Awake: start_bがtrueのためBGMを再生しました。");
                }
            }
            else
            {
                Debug.Log("Awake: start_bがfalseのためBGMは自動再生されません。");
            }
        }
        else if (instance != this)
        {
            // 既にインスタンスが存在する場合、新しい方を破棄
            Destroy(this.gameObject);
            Debug.Log("重複するMainSoundControllerオブジェクトを破棄しました。");
        }
    }

    // シーンがロードされたときに呼ばれるコールバック
    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // シーン切り替え時のstart_bの状態に応じたBGM制御
        if (start_b)
        {
            if (!main_sound.isPlaying)
            {
                main_sound.volume = originalVolume; // シーン切り替えでフェードアウトしていたら音量を戻す
                main_sound.Play();
                Debug.Log("OnSceneLoaded: start_bがtrueのためBGMを継続再生します。");
            }
        }
        else
        {
            // start_bがfalseになったらBGMをフェードアウト停止
            if (main_sound.isPlaying)
            {
                // 現在フェードアウト中のコルーチンがある場合は停止してから新しいフェードアウトを開始
                StopAllCoroutines(); 
                StartCoroutine(FadeOutAndStop());
                Debug.Log("OnSceneLoaded: start_bがfalseのためBGMをフェードアウト停止します。");
            }
        }
    }

    /// <summary>
    /// start_b の状態を外部から設定するメソッド。
    /// BGMの再生/停止、フェードアウトを制御します。
    /// </summary>
    /// <param name="state">trueでBGMを継続/再生、falseでBGMをフェードアウト停止</param>
    public void SetStartB(bool state)
    {
        if (start_b == state) return; // 状態に変更がなければ何もしない

        start_b = state;

        if (start_b)
        {
            // trueになった場合、BGMを再生または再開
            if (!main_sound.isPlaying)
            {
                // フェードアウト中にstart_bがtrueに戻された場合を考慮し、既存のフェードアウトを停止
                StopAllCoroutines(); 
                main_sound.volume = originalVolume; // 音量を元の状態に戻す
                main_sound.Play();
                Debug.Log("SetStartB: start_bがtrueに設定され、BGMを再生しました。");
            }
        }
        else
        {
            // falseになった場合、BGMをフェードアウト停止
            if (main_sound.isPlaying)
            {
                // フェードアウト中に再度SetStartB(false)が呼ばれた場合を考慮し、既存のフェードアウトを停止
                StopAllCoroutines(); 
                StartCoroutine(FadeOutAndStop());
                Debug.Log("SetStartB: start_bがfalseに設定され、BGMをフェードアウト停止します。");
            }
        }
    }

    /// <summary>
    /// BGMを徐々にフェードアウトさせて停止するコルーチン
    /// </summary>
    private IEnumerator FadeOutAndStop()
    {
        float startVolume = main_sound.volume; // フェードアウト開始時の音量

        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            main_sound.volume = Mathf.Lerp(startVolume, 0, t / fadeOutTime);
            yield return null; // 1フレーム待つ
        }

        main_sound.volume = 0; // 完全に音量を0にする
        main_sound.Stop();     // 停止
        main_sound.volume = originalVolume; // 次回再生のために音量を元に戻しておく
        Debug.Log("BGMのフェードアウトが完了し、停止しました。");
    }

    // テスト用：エディタ上からstart_bを切り替えるボタン
    [ContextMenu("Set start_b to True")]
    void SetStartBTrueDebug()
    {
        SetStartB(true);
    }

    [ContextMenu("Set start_b to False")]
    void SetStartBFalseDebug()
    {
        SetStartB(false);
    }
}