using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    [Header("サウンド")]
    public AudioSource noise_sound;
    public AudioSource writing_sound;



    [Header("チュートリアルの文字オブジェクト")]
    public GameObject MoveText_obj;
    public GameObject AorD_obj;
    public GameObject WorSpace_obj;
    public GameObject Jump_obj; 
    public GameObject Kakikomu_obj;
    public GameObject Mouse_obj;

    [Header("ステージ1オブジェクト")]
    public GameObject[] stone_obj;
    public GameObject River_obj;
    public GameObject RiverShadow_obj;
    //stage1-2
    public GameObject Tree_obj;
    public GameObject TreeShadow_obj;
    public GameObject karasu_stand_obj;
    public GameObject Yuuchan_karasuStage_obj;
    public GameObject[] deleteArea_obj = new GameObject[10];
    public GameObject karasu_fly_obj;
    public GameObject karasu_Attack_area_obj;
    public GameObject Classroom_obj;
    public GameObject classroomCollider_obj;
    public GameObject CloseDoor_obj;
    public GameObject OpenDoor_obj;
    public GameObject whiteBoard_obj;
    public GameObject DoorNobu_obj;
    public bool stage1Finish_b;


    [Header("その他の情報")]
    public GameObject Player_obj;
    public GameObject PlayerShadow_obj;
    public PlayerController PC;
    public GameObject Oka_stage1_obj;
    public GameObject Yuuchan_obj;


    void Start()
    {
        
        Player_obj = GameObject.FindWithTag("Player");
        PC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        noise_sound.Play();
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
        Yuuchan_obj.SetActive(false);
        River_obj.SetActive(false);
        Tree_obj.SetActive(false);
        karasu_stand_obj.SetActive(false);
        Yuuchan_karasuStage_obj.SetActive(false);
        karasu_fly_obj.SetActive(false);
        karasu_Attack_area_obj.SetActive(false);
        Classroom_obj.SetActive(false);
        CloseDoor_obj.SetActive(false);
        OpenDoor_obj.SetActive(false);
        whiteBoard_obj.SetActive(false);
        for (int i = 0; i < 10; i++)
        {
            if (deleteArea_obj[i] != null)
            {
                deleteArea_obj[i].SetActive(false);
            }

        }
        deleteArea_obj[0].SetActive(true);
        StartCoroutine(StartScene());
        
    }

    void Update()
    {
        switch (stage1Num_i)
        {
            case 1:
                StartCoroutine(JumpScene());
                deleteArea_obj[1].SetActive(true);
                stage1Num_i = 0;
                break;

            case 2:
                StartCoroutine(DrawLineScene());
                deleteArea_obj[2].SetActive(true);
                stage1Num_i = 0;
                break;
            case 3:
                StartCoroutine(Stage1_1Scene());
                deleteArea_obj[3].SetActive(true);
                stage1Num_i = 0;
                break;
            case 4:
                StartCoroutine(Stage1_2Scene());
                deleteArea_obj[4].SetActive(true);
                stage1Num_i = 0;
                break;
            case 5:
                StartCoroutine(Stage1_3Scene());
                stage1Num_i = 0;
                break;
        }

        if (stage1Finish_b)
        {
            //classroomCollider_obj.SetActive(false);
            StartCoroutine(Stage1_FinalStage());
            stage1Finish_b = false;
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
        deleteArea_obj[1].SetActive(true);
        PC.playerCanMove_b = true;
        PC.AorDCan_b = true;
    }

    IEnumerator JumpScene()
    {
        //Player動き固定
        PC.player_anim.SetBool("walk", false);
        PC.playerCanMove_b = false;
        //日記内容変更
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        //色のアルファを戻すスクリプトは、KakuText内に記入
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[1]));

        //石表示
        stone_obj[0].SetActive(true);
        SpriteRenderer stone_SR = stone_obj[0].GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeIn(stone_SR));
        stone_obj[1].SetActive(true);
        SpriteRenderer stoneShadow_SR = stone_obj[1].GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeIn(stoneShadow_SR));

        //AorD非表示 and MoveText非表示
        SpriteRenderer AorD_SR = AorD_obj.GetComponent<SpriteRenderer>();
        TextMeshProUGUI Move_txt = MoveText_obj.GetComponent<TextMeshProUGUI>();
        yield return StartCoroutine(FadeOutText(Move_txt));
        yield return StartCoroutine(FadeOut(AorD_SR));
        MoveText_obj.SetActive(false);
        AorD_obj.SetActive(false);
        yield return new WaitForSeconds(0);

        //ジャンプの表示
        Jump_obj.SetActive(true);
        TextMeshProUGUI Jump_txt = Jump_obj.GetComponent<TextMeshProUGUI>();
        yield return StartCoroutine(KakuText(Jump_txt, "ジャンプ"));
        yield return new WaitForSeconds(0.5f);
        SpriteRenderer WorSpace_SR = WorSpace_obj.GetComponent<SpriteRenderer>();
        WorSpace_obj.SetActive(true);
        yield return StartCoroutine(FadeIn(WorSpace_SR));

        //deleteArea_obj[1].SetActive(true);
        //プレイヤー動ける
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

        //deleteArea_obj[2].SetActive(true);
        //プレイヤー動作可能
        PC.playerCanMove_b = true;
        PC.KakikomuCan_b = true;
    }

    IEnumerator Stage1_1Scene()
    {
        //プレイヤー一旦止める
        PC.player_anim.SetBool("walk", false);
        PC.playerCanMove_b = false;
        PC.KakikomuCan_b = false;

        //マウスの非表示
        TextMeshProUGUI kakikomu_txt = Kakikomu_obj.GetComponent<TextMeshProUGUI>();
        yield return StartCoroutine(FadeOutText(kakikomu_txt));
        SpriteRenderer mouse_SR = Mouse_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeOut(mouse_SR));
        //日記フェードアウト
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        //ユウちゃん表示
        yield return new WaitForSeconds(1.5f);
        Yuuchan_obj.SetActive(true);
        //色のアルファを戻すスクリプトは、KakuText内に記入
        //日記の内容変更
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[2]));
        //日記フェードアウト
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        //日記の内容変更
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[3]));
        //日記フェードアウト
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        //日記の内容変更
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[4]));
        //日記フェードアウト
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        //ユウちゃんフェードアウト
        Yuuchan_obj.SetActive(false);
        //丘（チュートリアル最後）フェードアウト
        yield return new WaitForSeconds(1.0f);
        SpriteRenderer Oka_stage1_SR = Oka_stage1_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeOut(Oka_stage1_SR));
        Oka_stage1_obj.SetActive(false);

        //Stage1Start
        River_obj.SetActive(true);
        SpriteRenderer River_SR = River_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeIn(River_SR));
        //日記の内容変更
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[5]));
        //日記フェードアウト
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        
        //日記の内容変更
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[6]));

        //deleteArea_obj[3].SetActive(true);
        //ResetPos調整
        PC.RestartPos_obj.transform.position = new Vector3(-5.7255f,-2.95f,0);
        //プレイヤー動作可能
        PC.playerCanMove_b = true;
        PC.KakikomuCan_b = true;
    }

    IEnumerator Stage1_2Scene()
    {
        //playerの動き停止
        PC.player_anim.SetBool("walk", false);
        PC.KakikomuCan_b = false;
        PC.playerCanMove_b = false;
        //　River削除
        //フェードアウト
        SpriteRenderer riverShadow_SR = RiverShadow_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer river_SR = River_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeOut(riverShadow_SR));
        yield return StartCoroutine(FadeOut(river_SR));
        River_obj.SetActive(false);
        //フェードアウト
        yield return new WaitForSeconds(0f);
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));

        //日記の内容変更
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[7]));
        //フェードアウト
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        //日記の内容変更
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[8]));

        //ツリー表示＆カラス
        TreeShadow_obj.SetActive(true);
        SpriteRenderer treeShadow_SR = TreeShadow_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer Tree_SR = Tree_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeIn(treeShadow_SR));
        Tree_obj.SetActive(true);
        yield return StartCoroutine(FadeIn(Tree_SR));
        yield return new WaitForSeconds(1.0f);
        karasu_stand_obj.SetActive(true);
        SpriteRenderer Karasu_stand_SR = karasu_stand_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeIn(Karasu_stand_SR));
        //カラス表示
        karasu_Attack_area_obj.SetActive(true);
        karasu_fly_obj.SetActive(true);
        SpriteRenderer karasu_fly_SR = karasu_fly_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeIn(karasu_fly_SR));  

        //ユウちゃんの表示
        Yuuchan_karasuStage_obj.SetActive(true);
        //リスタートポジ
        PC.RestartPos_obj.transform.position = new Vector3(5.69f,-2.7f,0);
        //プレイヤー動作可能
        PC.playerCanMove_b = true;
        PC.KakikomuCan_b = true;
        
    }


    IEnumerator Stage1_3Scene()
    {
        //playerの動き停止
        PC.player_anim.SetBool("walk", false);
        PC.KakikomuCan_b = false;
        PC.playerCanMove_b = false;

        //フェードアウト
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        //ユウちゃん消える
        Yuuchan_karasuStage_obj.SetActive(false);
        //日記の内容変更
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[9]));
        //フェードアウト
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        //日記の内容変更
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[10]));
        //フェードアウト
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        //Treeフェードアウト
        SpriteRenderer tree_SR = Tree_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer treeShadow_SR = TreeShadow_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer karasu_stand_SR = karasu_stand_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeOut(karasu_stand_SR));
        yield return StartCoroutine(FadeOut(treeShadow_SR));
        yield return StartCoroutine(FadeOut(tree_SR));

        Tree_obj.SetActive(false);
        //日記の内容変更 
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[11]));

        //ステージ表示
        Classroom_obj.SetActive(true);
        SpriteRenderer Classroom_SR = Classroom_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeIn(Classroom_SR));

        CloseDoor_obj.SetActive(true);
        SpriteRenderer CloseDoor_SR = CloseDoor_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeIn(CloseDoor_SR));

        OpenDoor_obj.SetActive(false);

        whiteBoard_obj.SetActive(true);
        SpriteRenderer whiteBoard_SR = whiteBoard_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeIn(whiteBoard_SR));
        //フェードアウト
        yield return new WaitForSeconds(0f);
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        //日記の内容変更
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[12]));
        //フェードアウト
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(FadeOutText(NikkiContent_txt));
        //日記の内容変更
        yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[13]));

        //プレイヤー動作可能
        PC.playerCanMove_b = true;
        PC.KakikomuCan_b = true;

        yield return new WaitForSeconds(2f);
        DoorNobu_obj.SetActive(true);
        TextMeshProUGUI DoorNobu_TMPro = DoorNobu_obj.GetComponent<TextMeshProUGUI>();
        yield return StartCoroutine(KakuText(DoorNobu_TMPro, "ドアのぶ ないね"));
    }

    IEnumerator Stage1_FinalStage()
    {
        //Collider2D classroom_col = Classroom_obj.GetComponent<BoxCollider2D>();
        SpriteRenderer player_SR = Player_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(player_SR));
        SpriteRenderer playerShadow_SR = PlayerShadow_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(playerShadow_SR));
        yield return new WaitForSeconds(2f);
        TextMeshProUGUI DoorNobu_TMPro = DoorNobu_obj.GetComponent<TextMeshProUGUI>();
        StartCoroutine(FadeOutText(DoorNobu_TMPro));
        yield return new WaitForSeconds(4f);
        StartCoroutine(KakuText(DoorNobu_TMPro, "つぎは あなたの ばん"));
        yield return new WaitForSeconds(7f);
        StartCoroutine(FadeOutText(DoorNobu_TMPro));
        StartCoroutine(FadeOutText(Date_txt));
        StartCoroutine(FadeOutText(NikkiContent_txt));
        SpriteRenderer whiteBoard_SR = whiteBoard_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(whiteBoard_SR));
        SpriteRenderer Classroom_SR = Classroom_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(Classroom_SR));
        SpriteRenderer OpenDoor_SR = OpenDoor_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(OpenDoor_SR));
        yield return new WaitForSeconds(0.2f);
        noise_sound.Stop();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Stage4");

        
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