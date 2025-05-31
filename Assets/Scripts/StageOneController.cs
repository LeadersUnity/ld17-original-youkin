using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage1Controller : MonoBehaviour
{
    [Header("stage1管理")]
    public int stage1Num_i = 0;

    [Header("UI関連")]
    public GameObject Date_obj;
    public TextMeshProUGUI Date_txt;
    public string[] Date_string;
    public GameObject NikkiContent_obj;
    public TextMeshProUGUI NikkiContent_txt;
    public string[] NikkiContent_string;
    public GameObject Jump_obj;

    [Header("チュートリアルのオブジェクト")]
    public GameObject MoveText_obj;
    public GameObject AorD_obj;
    public GameObject WorSpace_obj;

    void Start()
    {
        Date_obj.SetActive(false);
        NikkiContent_obj.SetActive(false);
        MoveText_obj.SetActive(false);
        AorD_obj.SetActive(false);
        WorSpace_obj.SetActive(false);
        Jump_obj.SetActive(false);
        StartCoroutine(StartScene());
        
    }

    void Update()
    {
        if (stage1Num_i == 1)
        {
            StartCoroutine(JumpScene());
            stage1Num_i = 0;
        }
    }

    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(2f);
        //Debug.Log("ゲームが始まる");

        Date_obj.SetActive(true);
        //日付表示
        yield return StartCoroutine(KakuText(Date_txt, Date_string[0]));
        yield return new WaitForSeconds(1f);

        //日記内容表示
        NikkiContent_obj.SetActive(true);
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[0]));
        yield return new WaitForSeconds(0.5f);

        //AorD表示
        TextMeshProUGUI Move_txt = MoveText_obj.GetComponent<TextMeshProUGUI>();
        MoveText_obj.SetActive(true);
        yield return StartCoroutine(KakuText(Move_txt, "い ど う"));
        yield return new WaitForSeconds(0.5f);
        SpriteRenderer AorD_SR = AorD_obj.GetComponent<SpriteRenderer>();
        AorD_obj.SetActive(true);
        yield return StartCoroutine(FadeIn(AorD_SR));
    }

    IEnumerator JumpScene()
    {
        /*
        SpriteRenderer AorD_SR = AorD_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeOut(AorD_SR));
        TextMeshProUGUI Move_txt = MoveText_obj.GetComponent<TextMeshProUGUI>();
        yield return StartCoroutine(FadeOutText(Move_txt));
        */
        SpriteRenderer AorD_SR = AorD_obj.GetComponent<SpriteRenderer>();
        TextMeshProUGUI Move_txt = MoveText_obj.GetComponent<TextMeshProUGUI>();
        yield return StartCoroutine(FadeOutText(Move_txt));
        yield return StartCoroutine(FadeOut(AorD_SR));
        MoveText_obj.SetActive(false); 
        AorD_obj.SetActive(false);
        yield return new WaitForSeconds(0);
        Jump_obj.SetActive(true);
        TextMeshProUGUI Jump_txt = Jump_obj.GetComponent<TextMeshProUGUI>();
        yield return StartCoroutine(KakuText(Jump_txt, "ジャンプ"));
        yield return new WaitForSeconds(0.5f);
        SpriteRenderer WorSpace_SR = WorSpace_obj.GetComponent<SpriteRenderer>();
        WorSpace_obj.SetActive(true);
        yield return StartCoroutine(FadeIn(WorSpace_SR));
    }

    IEnumerator FadeIn(SpriteRenderer SR)
    {
        float FinishTime_f = 1f;
        float NowTime_f = 0f;

        Color c = SR.color;
        c.a = 0;
        SR.color = c;

        while (NowTime_f < FinishTime_f)
        {
            NowTime_f += Time.deltaTime;
            c.a = Mathf.Clamp01(NowTime_f / FinishTime_f);
            SR.color = c;
            yield return null;
        }
    }

    
    IEnumerator FadeOut(SpriteRenderer SR)
    {
        float FinishTime_f = 1f;
        float NowTime_f = 0f;

        Color c = SR.color;
        c.a = 1f;
        SR.color = c;

        while (NowTime_f < FinishTime_f)
        {
            NowTime_f += Time.deltaTime;
            c.a = Mathf.Clamp01(1 - (NowTime_f / FinishTime_f));
            SR.color = c;
            yield return null;
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
    

    IEnumerator KakuText(TextMeshProUGUI targetText, string content)
    {
        targetText.text = "";

        float waitTime = 0.1f; // ← 一文字ごとの固定時間

        foreach (char c in content)
        {
            targetText.text += c;
            yield return new WaitForSeconds(waitTime);
        }
    }

    
    
}