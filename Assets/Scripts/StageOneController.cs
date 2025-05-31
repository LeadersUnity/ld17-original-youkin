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
        StartCoroutine(StartScene());
    }

    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("ゲームが始まる");

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
        AorD_SR.color = new Color(255,255,255,0);
        AorD_SR.color = new Color(255,255,255,Time.deltaTime * 500);

    }

    IEnumerator FadeIn()
    {
        Debug.Log("FadeIn");
    }

    IEnumerator FadeIn()
    {
        Debug.Log("FadeOut");
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