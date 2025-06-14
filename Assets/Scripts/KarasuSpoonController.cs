using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KarasuSpoonController : MonoBehaviour
{
    [Header("出現させるカラスのプレハブ配列")]
    public GameObject[] karasuPrefabs;
    [Header("ユウちゃん発言")]
    public GameObject NikkiContent_Yuuchan_obj;
    public TextMeshProUGUI NikkiContent_Yuuchan_txt;
    public string[] NikkiContent_Yuuchan_string;
    [Header("サウンド")]
    public AudioSource writing_sound;
    [Header("プレイヤー情報")]
    private PlayerController PC;

    // 内部管理用変数
    private Coroutine runningSpoonCoroutine;
    private bool isResetting = false;

    void Start()
    {
        // プレイヤーコントローラーの参照を取得
        PC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (PC == null)
        {
            Debug.LogError("PlayerControllerが見つかりません！");
            this.enabled = false;
            return;
        }

        // 開始時に出現シーケンスを開始
        runningSpoonCoroutine = StartCoroutine(SpoonSequence());
    }

    void Update()
    {
        // --- ゲームオーバー時のリセット処理 ---
        NikkiContent_Yuuchan_obj.SetActive(true);
        // プレイヤーがゲームオーバーになり、かつまだリセット処理を開始していない場合
        if (PC.GameOver_b && !isResetting)
        {
            // 「リセット処理中」のフラグを立て、処理の重複を防ぐ
            isResetting = true;
            Debug.Log("ゲームオーバーを検知。カラスの出現をリセットします。");

            // 実行中の出現コルーチンがあれば停止
            if (runningSpoonCoroutine != null)
            {
                StopCoroutine(runningSpoonCoroutine);
            }

            // 全てのカラスを非表示にする
            foreach (GameObject karasu in karasuPrefabs)
            {
                if (karasu != null) karasu.SetActive(false);
            }

            // 新しく出現シーケンスを開始し、その参照を保存
            runningSpoonCoroutine = StartCoroutine(SpoonSequence());
        }

        // ゲームオーバー状態でなくなったら、次のリセットに備えてフラグを戻す
        if (!PC.GameOver_b && isResetting)
        {
            isResetting = false;
        }
    }

    // カラスの出現タイミングと位置を管理するコルーチン
    IEnumerator SpoonSequence()
    {
        // 念のため、開始前に全てのカラスを非表示にする
        foreach (GameObject karasu in karasuPrefabs)
        {
            if (karasu != null) karasu.SetActive(false);
        }
        yield return new WaitForSeconds(3.0f);
        // --- ここから出現シーケンス ---
        yield return StartCoroutine(KakuText(NikkiContent_Yuuchan_txt, NikkiContent_Yuuchan_string[0]));
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(FadeOutText(NikkiContent_Yuuchan_txt));
        yield return new WaitForSeconds(1.0f);

        //カラス１
        karasuPrefabs[0].transform.localPosition = new Vector3(0, 0, 0);
        karasuPrefabs[0].SetActive(true);

        //カラス2
        yield return new WaitForSeconds(3.0f);
        karasuPrefabs[1].transform.localPosition = new Vector3(-11.43f, 2.33f, 0);
        karasuPrefabs[1].SetActive(true);

        //カラス3
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(KakuText(NikkiContent_Yuuchan_txt, NikkiContent_Yuuchan_string[1]));
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(FadeOutText(NikkiContent_Yuuchan_txt));
        yield return new WaitForSeconds(2.0f);
        karasuPrefabs[2].transform.localPosition = new Vector3(-8.65f, 1.83f, 0);
        karasuPrefabs[2].SetActive(true);

        
        yield return new WaitForSeconds(3.0f);
        yield return StartCoroutine(KakuText(NikkiContent_Yuuchan_txt, NikkiContent_Yuuchan_string[2]));
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(FadeOutText(NikkiContent_Yuuchan_txt));
        yield return new WaitForSeconds(2.0f);
        karasuPrefabs[3].transform.localPosition = new Vector3(-11.77f, -4.67f, 0);
        karasuPrefabs[3].SetActive(true);
        karasuPrefabs[4].transform.localPosition = new Vector3(0.32f, -4.86f, 0);
        karasuPrefabs[4].SetActive(true);

        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(KakuText(NikkiContent_Yuuchan_txt, NikkiContent_Yuuchan_string[3]));
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(FadeOutText(NikkiContent_Yuuchan_txt));
        yield return new WaitForSeconds(2.0f);
        karasuPrefabs[5].transform.localPosition = new Vector3(-0.08f, 1.58f, 0);
        karasuPrefabs[5].SetActive(true);
        karasuPrefabs[6].transform.localPosition = new Vector3(-5.95f, 1.64f, 0);
        karasuPrefabs[6].SetActive(true);
    }

    IEnumerator KakuText(TextMeshProUGUI targetText, string content)
    {
        targetText.text = "";
        Color _color = targetText.color;
        _color.a = 1f;
        targetText.color = _color;
        float waitTime = 0.1f;

        // サウンドを再生
        if (writing_sound != null && !writing_sound.isPlaying)
        {
            writing_sound.Play();
        }

        foreach (char c in content)
        {
            targetText.text += c;
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