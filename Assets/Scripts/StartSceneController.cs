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
        SceneManager.LoadScene("Stage1");
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
        targetText.text = "";
        Color _color = targetText.color;
        _color.a = 1f;
        targetText.color = _color;
        float waitTime = 0.5f;

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

