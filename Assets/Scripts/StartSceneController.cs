using System.Collections;
using UnityEngine;
using TMPro;

public class StartSceneController : MonoBehaviour
{
    [Header("サウンド設定")]
    public AudioClip MainSong_sound;
    public float fadeDuration = 2f;       // フェードイン・アウト時間
    private AudioSource audioSource;
    private bool isFading = false;

    [Header("UI設定")]
    public TextMeshProUGUI Title_txt;
    public TextMeshProUGUI HajimeruButton_txt;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = MainSong_sound;
        audioSource.volume = 0f;
        audioSource.Play();

        StartCoroutine(FadeIn(audioSource, fadeDuration));
        StartCoroutine(LoopWithFade(audioSource));
        StartCoroutine(StartScene());
        
    }

    public void OnStartButtonClick()
    {
        Debug.Log("ゲームスタート");

    }

    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(KakuText(Title_txt, "ぼくのにっき"));
        yield return new WaitForSeconds(5f);
        StartCoroutine(KakuText(HajimeruButton_txt, "はじめる"));
    }

    IEnumerator FadeIn(AudioSource source, float duration)
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

    IEnumerator FadeOut(AudioSource source, float duration)
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
                yield return StartCoroutine(FadeOut(source, fadeDuration));

                source.Stop();
                source.Play();
                yield return StartCoroutine(FadeIn(source, fadeDuration));
                isFading = false;
            }
        }
    }

    IEnumerator KakuText(TextMeshProUGUI targetText, string content)
    {
        targetText.text = "";
        Color _color = targetText.color;
        _color.a = 1f;
        targetText.color = _color;
        float waitTime = 0.5f;

        foreach (char c in content)
        {
            targetText.text += c;
            yield return new WaitForSeconds(waitTime);
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

    
    
}

