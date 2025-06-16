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
    public GameObject NikkiContent_Yuuchan_obj;
    public TextMeshProUGUI NikkiContent_Yuuchan_txt;
    public string[] NikkiContent_Yuuchan_string;

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
    //フェーズ２
    public GameObject KarasuSpoon_obj;
    public GameObject background_obj;
    //フェーズ3
    public GameObject Tree_hanten_obj;
    public GameObject TreeShadow_hanten_obj;
    public GameObject Karasu_stand_hanten_obj;
    public GameObject Karasu_fly_hanten_obj;
    public GameObject karasuAttackArea_obj;
    public GameObject River_obj;
    public GameObject RiverShadow_obj;
    public GameObject Oka_obj;
    public GameObject OkaShadow_obj;

    [Header("プレイヤー情報")]
    public GameObject Player_obj;
    public GameObject PlayerShadow_obj;
    public PlayerController PC;
    [Header("ユウちゃん情報")]
    public GameObject Yuuchan_obj;
    public GameObject YuuchanShadow_obj;
    public YuuchanController YC;
    [Header("その他のオブジェクト")]
    public GameObject RestartPos_obj;
    public GameObject[] deleteArea_obj;
    public StrokeController SC;
    [Header("UI関連")]
    public Material text_mat;
    public Color textColor_black = Color.black;
    public Color textColor_white = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        //UI情報
        text_mat = Date_obj.GetComponent<Material>();
        Date_txt.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, textColor_black );
        SC = GameObject.FindWithTag( "StrokeController" ).GetComponent<StrokeController>();
        for (int i = 0; i < 10; i++)
        {
            if (deleteArea_obj[i] != null)
            {
                deleteArea_obj[i].SetActive(false);
            }
        }

        YC = Yuuchan_obj.GetComponent<YuuchanController>();
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
        background_obj.SetActive(false);
        Yuuchan_obj.SetActive(false);
        Tree_hanten_obj.SetActive(false);
        TreeShadow_hanten_obj.SetActive(false);
        Karasu_stand_hanten_obj.SetActive(false);
        Karasu_fly_hanten_obj.SetActive(false);
        karasuAttackArea_obj.SetActive(false);
        River_obj.SetActive(false);
        RiverShadow_obj.SetActive(false);
        Oka_obj.SetActive(false);
        OkaShadow_obj.SetActive(false);
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
            case 2:
                StartCoroutine(PhaseTwo());
                //deleteArea_obj[1].SetActive(true);
                stage4Num_i = 0;
                break;
            case 3:
                StartCoroutine(PhaseThree());
                stage4Num_i = 0;
                break;
            case 4:
                StartCoroutine(PhaseFour());
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
        //日記内容変更
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[1]));
        yield return new WaitForSeconds(2.3f);
        //日記フェードアウト
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        //日記内容変更
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

        deleteArea_obj[0].SetActive(true);

        //プレイヤー操作
        PC.playerCanMove_b = true;
        PC.AorDCan_b = true;
        PC.jumpCan_b = true;
        PC.KakikomuCan_b = true;
        
    }

    IEnumerator PhaseOne()
    {
        deleteArea_obj[0].SetActive(false);
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
        //日記フェードアウト
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        //日記内容変更
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[3]));
        //Yuuchan表示
        yield return new WaitForSeconds(0.5f);
        Yuuchan_hanten_half_obj.SetActive(true);
        yield return new WaitForSeconds(2f);
        SpriteRenderer Yuuchan_hanten_half_SR = Yuuchan_hanten_half_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(Yuuchan_hanten_half_SR));

        //ステージ4フェーズ1全消し
        yield return new WaitForSeconds(2f);
        SpriteRenderer room_SR = room_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(room_SR));
        //SpriteRenderer bed_SR = bed_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(bed_bokuin_SR));
        StartCoroutine(FadeOut(Boku_inbed_SR));
        SpriteRenderer roomShadow_SR = roomShadow_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(roomShadow_SR));
        yield return new WaitForSeconds(1f);
        room_obj.SetActive(false);
        bed_Bokuin_obj.SetActive(false);
        boku_inBed_obj.SetActive(false);
        roomShadow_obj.SetActive(false);

        //日記フェードアウト
        StartCoroutine(FadeOutText(Date_txt));
        StartCoroutine(FadeOutText(NikkiContent_txt));

        //フェード2
        yield return new WaitForSeconds(1f);
        background_obj.SetActive(true);
        SpriteRenderer background_SR = background_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeIn(background_SR));
        yield return new WaitForSeconds(1f);
        Date_txt.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, textColor_white);
        NikkiContent_txt.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, textColor_white);
        RestartPos_obj.transform.localPosition = new Vector3(6, -6, 0);
        //プレイヤー反転
        //書き込み反転
        SC.lineColor = Color.white;
        PC.isHanten_b = true;

        //日記表示
        yield return StartCoroutine(KakuText(Date_txt, Date_string[0]));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[4]));
        yield return new WaitForSeconds(2f);

        //プレイヤー表示
        yield return StartCoroutine(FadeIn(player_SR));
        //日記変更
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[5]));

        //ユウちゃん表示 反転はAnimatorで最初からtrueにしてる
        Yuuchan_obj.SetActive(true);
        SpriteRenderer yuuchan_SR = Yuuchan_obj.GetComponent<SpriteRenderer>();
        //YC.yuuchan_anim.SetBool("hanten", true);   
        StartCoroutine(FadeIn(yuuchan_SR));
        KarasuSpoon_obj.SetActive(false);

        //プレイヤー操作可能
        PC.playerCanMove_b = true;

        //日記の内容変更
        yield return new WaitForSeconds(2f);
        //日記変更
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[6]));

        //カラス始動
        KarasuSpoon_obj.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
    }

    IEnumerator PhaseTwo()
    {
        Destroy(KarasuSpoon_obj);
        yield return new WaitForSeconds(1f);
        //プレイヤーの移動ストップ
        PC.playerCanMove_b = false;
        PC.player_anim.SetBool("walk", false);
        //ユウちゃん非表示
        SpriteRenderer Yuuchan_SR = Yuuchan_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(Yuuchan_SR));
        yield return new WaitForSeconds(1f);
        Yuuchan_obj.SetActive(false);

        //日記の内容変更
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[7]));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));

        //プレイヤー非表示
        yield return new WaitForSeconds(2f);
        SpriteRenderer player_SR = Player_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(player_SR));
        RestartPos_obj.transform.localPosition = new Vector3(-5f, 0.5f, 0);

        //KarasuTreeステージ表示
        yield return new WaitForSeconds(1f);
        Tree_hanten_obj.SetActive(true);
        TreeShadow_hanten_obj.SetActive(true);
        SpriteRenderer tree_SR = Tree_hanten_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer treeShadow_SR = TreeShadow_hanten_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn(tree_SR));
        StartCoroutine(FadeIn(treeShadow_SR));
        yield return new WaitForSeconds(0.5f);
        Karasu_stand_hanten_obj.SetActive(true);
        SpriteRenderer karasu_stand_SR = Karasu_stand_hanten_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn(karasu_stand_SR));
        yield return new WaitForSeconds(0.5f);
        karasuAttackArea_obj.SetActive(true);
        Karasu_fly_hanten_obj.SetActive(true);
        SpriteRenderer karasu_fly_SR = Karasu_fly_hanten_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn(karasu_fly_SR));
        
        

        //Player_obj.SetActive(true);
        //Player表示
        yield return new WaitForSeconds(1.5f);
        Player_obj.transform.position = new Vector3(-5.23f, 2.78f, 0);
        StartCoroutine(FadeIn(player_SR));
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[8]));
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[9]));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        deleteArea_obj[1].SetActive(true);
        //プレイヤー移動可能
        PC.playerCanMove_b = true;

    }

    IEnumerator PhaseThree()
    {
        yield return new WaitForSeconds(1f);
        //プレイヤーの移動ストップ
        PC.playerCanMove_b = false;
        PC.player_anim.SetBool("walk", false);

        //Tree非表示
        yield return new WaitForSeconds(1f);
        SpriteRenderer tree_SR = Tree_hanten_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer treeShadow_SR = TreeShadow_hanten_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer karasu_stand_SR = Karasu_stand_hanten_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(tree_SR));
        StartCoroutine(FadeOut(treeShadow_SR));
        StartCoroutine(FadeOut(karasu_stand_SR));
        yield return new WaitForSeconds(1.2f);
        Tree_hanten_obj.SetActive(false);
        TreeShadow_hanten_obj.SetActive(false);
        Karasu_stand_hanten_obj.SetActive(false);
        karasuAttackArea_obj.SetActive(false);
        deleteArea_obj[1].SetActive(false);

        //Player表示
        yield return new WaitForSeconds(1f);
        SpriteRenderer player_SR = Player_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(player_SR));

        //ResetPos変更
        RestartPos_obj.transform.localPosition = new Vector3(5.93f, -5.44f, 0);

        //Riverの表示
        yield return new WaitForSeconds(2f);
        River_obj.SetActive(true);
        RiverShadow_obj.SetActive(true);
        SpriteRenderer River_SR = River_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer RiverShadow_SR = RiverShadow_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn(River_SR));
        StartCoroutine(FadeIn(RiverShadow_SR));

        //Player表示
        Player_obj.transform.localPosition = new Vector3(5.79f, -2.91f, 0);
        StartCoroutine(FadeIn(player_SR));

        //Player移動可能
        PC.playerCanMove_b = true;
        
    }

    IEnumerator PhaseFour()
    {
        //player操作不可
        PC.player_anim.SetBool("walk", false);
        PC.playerCanMove_b = false;
        yield return new WaitForSeconds(1f);
        Debug.Log("最終フェーズ");
        //
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
