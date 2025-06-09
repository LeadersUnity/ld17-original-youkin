using System.Collections;
using UnityEngine;

public class StartSceneController : MonoBehaviour
{
    [Header("サウンド設定")]
    public AudioClip MainSong_sound;
    public float fadeDuration = 2f;       // フェードイン・アウト時間
    private AudioSource audioSource;
    private bool isFading = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = MainSong_sound;
        audioSource.volume = 0f;
        audioSource.Play();

        StartCoroutine(FadeIn(audioSource, fadeDuration));
        StartCoroutine(LoopWithFade(audioSource));
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
            timer += Time.deltaTime /4;
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
}

