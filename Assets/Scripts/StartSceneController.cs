using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    [Header("サウンド設定")]
    public AudioClip MainSong_sound;
    public float fadeDuration = 1.5f;       // フェードイン・アウト時間
    private AudioSource audioSource;
    private bool isFading = false;
    public AudioSource writing_sound;

    [Header("UI設定")]
    public TextMeshProUGUI Title_txt;
    public TextMeshProUGUI HajimeruButton_txt;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = MainSong_sound;
        audioSource.volume = 0f;
        audioSource.Play();

        StartCoroutine(FadeInAudio(audioSource, fadeDuration));
        StartCoroutine(LoopWithFade(audioSource));
        StartCoroutine(StartScene());
        
    }

    public void OnStartButtonClick()
    {
        //Debug.Log("ゲームスタート");
        StartCoroutine(StartNextScene());
    }

    IEnumerator StartNextScene()
    {
        yield return new WaitForSeconds(0f);
        StartCoroutine(FadeOutText(HajimeruButton_txt));
        StartCoroutine(FadeOutAudio(audioSource, fadeDuration));
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("Scene1_main");
    }

    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(KakuText(Title_txt, "ぼくのにっき"));
        yield return new WaitForSeconds(4f);
        StartCoroutine(KakuText(HajimeruButton_txt, "はじめる"));
        
    }

    IEnumerator FadeInAudio(AudioSource source, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime / 4;
            source.volume = Mathf.Lerp(0f, 1f, timer / duration);
            yield return null;
        }
        source.volume = 1f;
    }

    IEnumerator FadeOutAudio(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime / 4;
            source.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }
        source.volume = 0f;
    }

    IEnumerator LoopWithFade(AudioSource source)
    {
        while (true)
        {
            float clipLength = source.clip.length;
            yield return new WaitForSeconds(clipLength - fadeDuration); // フェードアウト前まで待つ

            if (!isFading)
            {
                isFading = true;
                yield return StartCoroutine(FadeOutAudio(source, fadeDuration));

                source.Stop();
                source.Play();
                yield return StartCoroutine(FadeInAudio(source, fadeDuration));
                isFading = false;
            }
        }
    }

    IEnumerator KakuText(TextMeshProUGUI targetText, string content)
    {
        // 最初にテキストコンポーネント自体のアルファを1（不透明）にしておく
        Color baseColor = targetText.color;
        baseColor.a = 1f;
        targetText.color = baseColor;

        float waitTime = 1f;

        // サウンドを再生
        if (writing_sound != null && !writing_sound.isPlaying)
        {
            writing_sound.Play();
        }

        // 1. 全文をテキストコンポーネントにセットする
        targetText.text = content;

        // 2. テキスト情報を強制的に更新して、文字ごとの情報を生成させる
        targetText.ForceMeshUpdate();

        TMP_TextInfo textInfo = targetText.textInfo;
        int totalCharacters = textInfo.characterCount;

        // 3. 全ての文字の頂点カラーを透明にする
        for (int i = 0; i < totalCharacters; i++)
        {
            // 文字情報を取得
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            
            // 文字が存在しない、または空白などの場合はスキップ
            if (!charInfo.isVisible)
            {
                continue;
            }

            // 文字を構成する頂点のマテリアルインデックスと頂点インデックスを取得
            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            // 頂点カラー情報を取得し、アルファ値を0（透明）に設定
            Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;
            Color32 transparentColor = vertexColors[vertexIndex + 0];
            transparentColor.a = 0;

            vertexColors[vertexIndex + 0] = transparentColor;
            vertexColors[vertexIndex + 1] = transparentColor;
            vertexColors[vertexIndex + 2] = transparentColor;
            vertexColors[vertexIndex + 3] = transparentColor;
        }

        // 頂点カラーの変更をメッシュに適用（この時点ではまだ画面に反映されない）
        targetText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        // 4. 1文字ずつ透明度を戻していく
        for (int i = 0; i < totalCharacters; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;

            // 頂点カラーを255（不透明）に戻す
            Color32 opaqueColor = vertexColors[vertexIndex + 0];
            opaqueColor.a = 255;
            vertexColors[vertexIndex + 0] = opaqueColor;
            vertexColors[vertexIndex + 1] = opaqueColor;
            vertexColors[vertexIndex + 2] = opaqueColor;
            vertexColors[vertexIndex + 3] = opaqueColor;

            // 変更をメッシュに適用して、文字を表示
            targetText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            yield return new WaitForSeconds(waitTime);
        }

        // サウンドを停止
        if (writing_sound != null && writing_sound.isPlaying)
        {
            writing_sound.Stop();
        }
    }

    IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        float FinishTime_f = 1f;
        float NowTime_f = 0f;

        Debug.Log("文字消えた");    
        Color c = text.color;
        c.a = 1f; 
        text.color = c;

        while (NowTime_f < FinishTime_f)
        {
            NowTime_f += Time.deltaTime;
            c.a = Mathf.Clamp01(1f - (NowTime_f / FinishTime_f));
            text.color = c;
            yield return null;
        }
    }

    
    
}

