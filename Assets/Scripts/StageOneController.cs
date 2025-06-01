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


    [Header("チュートリアルの文字オブジェクト")]
    public GameObject MoveText_obj;
    public GameObject AorD_obj;
    public GameObject WorSpace_obj;
    public GameObject Jump_obj; 
    public GameObject Kakikomu_obj;
    public GameObject Mouse_obj;

    [Header("ステージ1オブジェクト")]
    public GameObject[] stone_obj;
    [Header("その他の情報")]
    public PlayerController PC;
    public GameObject Oka_stage1_obj;

    void Start()
    {
        PC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        Date_obj.SetActive(false);
        NikkiContent_obj.SetActive(false);
        MoveText_obj.SetActive(false);
        AorD_obj.SetActive(false);
        WorSpace_obj.SetActive(false);
        Jump_obj.SetActive(false);
        stone_obj[0].SetActive(false);
        stone_obj[1].SetActive(false);
        Kakikomu_obj.SetActive(false);
        Mouse_obj.SetActive(false);
        Oka_stage1_obj.SetActive(false);
        StartCoroutine(StartScene());
        
    }

    void Update()
    {
        switch (stage1Num_i)
        {
            case 1:
                StartCoroutine(JumpScene());
                stage1Num_i = 0;
                break;

            case 2:
                StartCoroutine(DrawLineScene());
                stage1Num_i = 0;
                break;
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
        PC.playerCanMove_b = true;
        PC.AorDCan_b = true;
    }

    IEnumerator JumpScene()
    {
        PC.player_anim.SetBool("walk", false);
        PC.playerCanMove_b = false;
        stone_obj[0].SetActive(true);
        SpriteRenderer stone_SR = stone_obj[0].GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeIn(stone_SR));
        stone_obj[1].SetActive(true);
        SpriteRenderer stoneShadow_SR = stone_obj[1].GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeIn(stoneShadow_SR));
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
        PC.playerCanMove_b = true;
        PC.jumpCan_b = true;
    }

    IEnumerator DrawLineScene()
    {
        //プレイヤー一旦止める
        PC.player_anim.SetBool("walk", false);
        PC.playerCanMove_b = false;
        //ジャンプ系　見えなくする
        TextMeshProUGUI Jump_txt = Jump_obj.GetComponent<TextMeshProUGUI>();
        SpriteRenderer WorSpace_SR = WorSpace_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeOutText(Jump_txt));
        yield return StartCoroutine(FadeOut(WorSpace_SR));
        Jump_obj.SetActive(false);
        WorSpace_obj.SetActive(false);
        //石の非表示
        SpriteRenderer stone_SR = stone_obj[0].GetComponent<SpriteRenderer>();
        SpriteRenderer stoneShadow_SR = stone_obj[1].GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeOut(stone_SR));
        yield return StartCoroutine(FadeOut(stoneShadow_SR));
        Destroy(stone_obj[0]);
        Destroy(stone_obj[1]);
        //書き込む表示
        Debug.Log("DrawLineScene");
        Kakikomu_obj.SetActive(true);
        TextMeshProUGUI Kakikomu_txt = Kakikomu_obj.GetComponent<TextMeshProUGUI>();
        yield return StartCoroutine(KakuText(Kakikomu_txt, "かきこむ"));
        SpriteRenderer mouse_SR = Mouse_obj.GetComponent<SpriteRenderer>();
        Mouse_obj.SetActive(true);
        yield return StartCoroutine(FadeIn(mouse_SR));
        //ステージの表示
        SpriteRenderer Oka_stage1_SR = Oka_stage1_obj.GetComponent<SpriteRenderer>();
        Oka_stage1_obj.SetActive(true);
        yield return StartCoroutine(FadeIn(Oka_stage1_SR));

        //プレイヤー動作可能
        PC.playerCanMove_b = true;
        PC.KakikomuCan_b = true;
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