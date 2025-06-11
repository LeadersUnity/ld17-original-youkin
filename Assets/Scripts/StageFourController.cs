using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageFourController : MonoBehaviour
{
    [Header("stage1管理")]
    public int stage4Num_i = 0;

    [Header("UI関連")]
    public GameObject Date_obj;
    public TextMeshProUGUI Date_txt;
    public string[] Date_string;
    public GameObject NikkiContent_obj;
    public TextMeshProUGUI NikkiContent_txt;
    public string[] NikkiContent_string;

    [Header("サウンド")]
    public AudioSource noise_sound;
    public AudioSource writing_sound;
    [Header("ステージ4オブジェクト")]
    public GameObject room_stage4_obj;
    public GameObject roomShadow_obj;
    [Header("プレイヤー情報")]
    public GameObject Player_obj;
    public GameObject PlayerShadow_obj;
    public PlayerController PC;
    // Start is called before the first frame update
    void Start()
    {
        Player_obj = GameObject.FindWithTag("Player");
        PC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        noise_sound.Play();
        Date_obj.SetActive(false);
        NikkiContent_obj.SetActive(false);
        room_stage4_obj.SetActive(false);
        roomShadow_obj.SetActive(false);
        StartCoroutine(StartScene());
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
    IEnumerator FadeInText(TextMeshProUGUI text)
    {
        float FinishTime_f = 1f;
        float NowTime_f = 0f;

        Color c = text.color;
        c.a = 1f; 
        text.color = c;

        while (NowTime_f < FinishTime_f)
        {
            NowTime_f += Time.deltaTime;
            c.a = Mathf.Clamp01(NowTime_f / FinishTime_f);
            text.color = c;
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
}
