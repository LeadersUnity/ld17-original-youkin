using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageFourController : MonoBehaviour
{
    [Header("stage4管理")]
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
    public GameObject room_obj;
    public GameObject bed_obj;
    public GameObject bed_Bokuin_obj;
    public GameObject boku_inBed_obj;
    public GameObject roomShadow_obj;
    public GameObject Yuuchan_hanten_half_obj;

    [Header("プレイヤー情報")]
    public GameObject Player_obj;
    public GameObject PlayerShadow_obj;
    public PlayerController PC;
    [Header("その他のオブジェクト")]
    public GameObject[] deleteArea_obj;
    // Start is called before the first frame update
    void Start()
    {
        Player_obj = GameObject.FindWithTag("Player");
        PC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        noise_sound.Play();
        Date_obj.SetActive(false);
        NikkiContent_obj.SetActive(false);
        room_obj.SetActive(false);
        roomShadow_obj.SetActive(false);
        bed_obj.SetActive(false);
        bed_Bokuin_obj.SetActive(false);
        boku_inBed_obj.SetActive(false);
        Yuuchan_hanten_half_obj.SetActive(false);
        StartCoroutine(StartScene());
    }

    private void Update() {
        switch (stage4Num_i)
        {
            case 1:
                StartCoroutine(PhaseOne());
                //deleteArea_obj[1].SetActive(true);
                stage4Num_i = 0;
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
        yield return new WaitForSeconds(2.3f);
        //日記フェードアウト
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[1]));
        yield return new WaitForSeconds(2.3f);
        //日記フェードアウト
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[2]));
        yield return new WaitForSeconds(2.3f);


        //ルーム表示
        room_obj.SetActive(true);
        SpriteRenderer room_SR = room_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn(room_SR));

        bed_obj.SetActive(true);
        SpriteRenderer bed_SR = bed_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn(bed_SR));
        roomShadow_obj.SetActive(true);
        SpriteRenderer roomShadow_SR = roomShadow_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeIn(roomShadow_SR));

        //プレイヤー操作
        PC.playerCanMove_b = true;
        PC.AorDCan_b = true;
        PC.jumpCan_b = true;
        PC.KakikomuCan_b = true;
        
    }

    IEnumerator PhaseOne()
    {
        //プレイヤー操作
        PC.player_anim.SetBool("walk", false);
        PC.playerCanMove_b = false;
        //プレイヤーフェードアウト
        yield return new WaitForSeconds(2f);
        SpriteRenderer player_SR = Player_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeOut(player_SR));

        SpriteRenderer bed_SR = bed_obj.GetComponent<SpriteRenderer>();
        bed_Bokuin_obj.SetActive(true);
        boku_inBed_obj.SetActive(true);
        SpriteRenderer bed_bokuin_SR = bed_Bokuin_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn(bed_bokuin_SR));
        StartCoroutine(FadeOut(bed_SR));
        SpriteRenderer Boku_inbed_SR = boku_inBed_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn(Boku_inbed_SR));
        SpriteRenderer playerShadow_SR = PlayerShadow_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(playerShadow_SR));
        //Yuuchan表示
        yield return new WaitForSeconds(2.7f);
        Yuuchan_hanten_half_obj.SetActive(true);
        yield return new WaitForSeconds(2f);
        SpriteRenderer Yuuchan_hanten_half_SR = Yuuchan_hanten_half_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(Yuuchan_hanten_half_SR));
        




        
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
