using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalSceneController : MonoBehaviour
{
    [Header("サウンド設定")]
    public AudioClip MainSong_sound;
    public float fadeDuration = 1.5f;       // フェードイン・アウト時間
    private AudioSource audioSource;
    private bool isFading = false;
    public AudioSource writing_Yuuchan_sound;
    public GameObject NameCover_obj;
    public GameObject Panel_obj;

    [Header("UI設定")]
    public TextMeshProUGUI Title_txt;
    public TextMeshProUGUI YuuchanName_txt;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FinalScene());
        
    }

    IEnumerator FinalScene()
    {
        yield return new WaitForSeconds(2F);
        SpriteRenderer NameCover_SR = NameCover_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeOut(NameCover_SR));
        yield return new WaitForSeconds(1F);
        yield return StartCoroutine(KakuText_Yuuchan(YuuchanName_txt, "ユウちゃん"));
        yield return new WaitForSeconds(1F);
        Image Panel_SR = Panel_obj.GetComponent<Image>();
        yield return StartCoroutine(FadeIn(Panel_SR));
    }
    IEnumerator FadeIn(Image SR)
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

    IEnumerator KakuText_Yuuchan(TextMeshProUGUI targetText, string content)
    {
        targetText.text = "";
        Color _color = targetText.color;
        _color.a = 1f;
        targetText.color = _color;
        float waitTime = 0.5f;

        // サウンドを再生
        if (writing_Yuuchan_sound != null && !writing_Yuuchan_sound.isPlaying)
        {
            writing_Yuuchan_sound.Play();
        }

        foreach (char c in content)
        {
            targetText.text += c;
            yield return new WaitForSeconds(waitTime);
        }

        // サウンドを停止
        if (writing_Yuuchan_sound != null && writing_Yuuchan_sound.isPlaying)
        {
            writing_Yuuchan_sound.Stop();
        }
        
    }
}
