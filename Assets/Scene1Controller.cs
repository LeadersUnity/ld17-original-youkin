using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Scene1Controller : MonoBehaviour
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
    public AudioSource YuuchanVoice_sound;
    
    [Header("チュートリアルオブジェクト")]
    public GameObject parents_obj;
    public GameObject GenkanDoor_obj;
    public GameObject GenkanDoorShadow_obj;
    public GameObject GenkanDoorFloorShadow_obj;
    public GameObject GenkanDoorOpen_obj;
    public GameObject GenkanDoorOpenShadow_obj;
    public GameObject Oka_obj;
    [Header("ステージ1ゲームオブジェクト")]
    public GameObject[] deleteArea_obj = new GameObject[10];
    public GameObject River_obj;
    public GameObject RiverShadow_obj;
    public GameObject RistartPos_obj;
    public GameObject Tree_obj;
    public GameObject TreeShadow_obj;
    public GameObject Karasu_obj;
    public GameObject Karasu_fly_obj;
    public GameObject Karasu_AttackArea_obj;
    public GameObject Yuuchan_karasuStage_obj;
    public GameObject RoadInForest_obj;
    public GameObject RoadInForestShadow_obj;

    public GameObject NextScene_obj;
    [Header("ステージ1変数")]
    public bool ScrollStart_b;

    [Header("チュートリアルの文字オブジェクト")]
    public GameObject Enter_obj;
    public GameObject MoveText_obj; //いどう
    public GameObject AorD_obj;
    public GameObject WorSpace_obj;
    public GameObject Jump_obj;
    public GameObject Kakikomu_obj;
    public GameObject Mouse_obj;

    [Header("その他の情報")]
    public GameObject Player_obj;
    public GameObject PlayerShadow_obj;
    public PlayerController PC;
    public GameObject Yuuchan_obj;

    // --- ここから追加した変数 ---
    [Header("シナリオ進行管理")]
    public int CanReadNikkiContent_num = 0; // 読むことができる日記の最大インデックス
    public int currentTextIndex = 0;       // 現在表示している日記のインデックス

    private bool isTyping = false;    // テキストが1文字ずつ表示されている最中か
    private bool skipTyping = false;  // テキスト表示をスキップするか
    private bool isNikkiContentTyping = false; // NikkiContentの表示中かどうかを判定

    public int SceneInt_skip_i;
    // --- ここまで追加した変数 ---


    // Start is called before the first frame update
    void Start()
    {
        Player_obj = GameObject.FindWithTag("Player");
        PC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        noise_sound.Play();
        //チュートリアル文字系
        Enter_obj.SetActive(false);
        Date_obj.SetActive(false);
        NikkiContent_obj.SetActive(false);
        MoveText_obj.SetActive(false);
        AorD_obj.SetActive(false);
        WorSpace_obj.SetActive(false);
        Jump_obj.SetActive(false);
        Kakikomu_obj.SetActive(false);
        Mouse_obj.SetActive(false);
        //チュートリアルオブジェクト系
        parents_obj.SetActive(false);
        GenkanDoor_obj.SetActive(false);
        GenkanDoorFloorShadow_obj.SetActive(false);
        GenkanDoorShadow_obj.SetActive(false);
        GenkanDoorOpen_obj.SetActive(false);
        GenkanDoorOpenShadow_obj.SetActive(false);
        Oka_obj.SetActive(false);

        //ステージ1オブジェクト系
        River_obj.SetActive(false);
        RiverShadow_obj.SetActive(false);
        RistartPos_obj.SetActive(false);
        Tree_obj.SetActive(false);
        TreeShadow_obj.SetActive(false);
        Karasu_obj.SetActive(false);
        Karasu_fly_obj.SetActive(false);
        Karasu_AttackArea_obj.SetActive(false);
        Yuuchan_karasuStage_obj.SetActive(false);
        RoadInForest_obj.SetActive(false);
        RoadInForestShadow_obj.SetActive(false);

        //NExtSceneに進む仮オブジェクト
        NextScene_obj.SetActive(false);

        //DeleteAreaのSetActiveをfalseに
        for (int i = 0; i < 10; i++)
        {
            if (deleteArea_obj[i] != null)
            {
                deleteArea_obj[i].SetActive(false);
            }

        }
        StartCoroutine(AorDScene());
    }

    // --- ここから修正したUpdateメソッド ---
    // Update is called once per frame
    void Update()
    {
        //スキップ機能

        if (Input.GetKeyDown(KeyCode.R))
        {
            Player_obj.transform.position = deleteArea_obj[SceneInt_skip_i].transform.position;
            SceneInt_skip_i += 1;


        }
        // NikkiContentが表示されている最中にEnterキーが押されたら
        if (isTyping && isNikkiContentTyping && Input.GetKeyDown(KeyCode.Return))
        {
            // スキップフラグを立てる
            skipTyping = true;
        }

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
        if (ScrollStart_b)
        {
            ScrollStage();
        }
        
    }
    // --- ここまで修正したUpdateメソッド ---
    public void ScrollStage()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            RoadInForest_obj.transform.position += new Vector3(-1f * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {  
            RoadInForest_obj.transform.position += new Vector3(1f * Time.deltaTime, 0, 0);
        }
    }

    // --- ここから修正したAorDSceneメソッド ---
    IEnumerator AorDScene()
    {
        yield return new WaitForSeconds(2f);
        CanReadNikkiContent_num = 2;

        bool dateSetActive_b = false;
        // 日記の表示ループ
        while (true)
        {
            // 配列の範囲外になったらループを抜ける
            if (currentTextIndex >= Date_string.Length || currentTextIndex >= NikkiContent_string.Length)
            {
                break;
            }

            // --- 日付表示 ---
            if (dateSetActive_b == false)
            {
                Date_obj.SetActive(true);
                Date_txt.text = ""; // 表示前にクリア
                yield return StartCoroutine(KakuText(Date_txt, Date_string[currentTextIndex]));

                // 日付から日記本文へは自動で進むように少し待機
                yield return new WaitForSeconds(1f);
                dateSetActive_b = true;
            }

            // --- 日記内容表示 ---
            yield return new WaitForSeconds(0.2f);
                NikkiContent_obj.SetActive(true);
            NikkiContent_txt.text = ""; // 表示前にクリア
            isNikkiContentTyping = true; // スキップ機能を有効にする
            yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[currentTextIndex]));
            isNikkiContentTyping = false; // スキップ機能を無効にする


            // --- 読める上限に達したかチェック ---
            if (currentTextIndex >= CanReadNikkiContent_num)
            {
                // 上限に達した。ループを抜けて、テキストは表示したままにする
                break;
            }

            // 上限に達していない場合は、Enterキー入力で次の日記へ進む
            yield return StartCoroutine(WaitForEnterKey());

            // 次の日記へ
            currentTextIndex++;
        }

        // ループを抜けた後、チュートリアルが始まるまで少し待つ
        yield return new WaitForSeconds(1f);

        //AorD表示
        TextMeshProUGUI Move_txt = MoveText_obj.GetComponent<TextMeshProUGUI>();
        MoveText_obj.SetActive(true);
        yield return StartCoroutine(KakuText(Move_txt, "い ど う"));
        // --- チュートリアルテキストは入力待ちしない ---

        yield return new WaitForSeconds(0.5f);
        SpriteRenderer AorD_SR = AorD_obj.GetComponent<SpriteRenderer>();
        AorD_obj.SetActive(true);
        yield return StartCoroutine(FadeIn(AorD_SR,1));
        
        SpriteRenderer parents_SR = parents_obj.GetComponent<SpriteRenderer>();
        parents_obj.SetActive(true);
        yield return StartCoroutine(FadeIn(parents_SR,1));
        deleteArea_obj[0].SetActive(true);
        PC.playerCanMove_b = true;
        PC.AorDCan_b = true;
    }

    // --- ここから修正したJumpSceneメソッド ---
    IEnumerator JumpScene()
    {
        yield return new WaitForSeconds(0);
        PC.player_anim.SetBool("walk", false);
        PC.playerCanMove_b = false;

        //AorD消す
        TextMeshProUGUI Move_txt = MoveText_obj.GetComponent<TextMeshProUGUI>();
        StartCoroutine(FadeOutText(Move_txt));
        SpriteRenderer AorD_SR = AorD_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(AorD_SR));

        //両親を消す
        SpriteRenderer parents_SR = parents_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeOut(parents_SR));

        // --- ここから日記表示処理 ---
        CanReadNikkiContent_num = 3; // 新しい日記を読めるように設定
        currentTextIndex++; // 次の日記に進む

        // 日記の表示ループ
        while (true)
        {
            // 配列の範囲外になったらループを抜ける
            if (currentTextIndex >= NikkiContent_string.Length)
            {
                break;
            }
            /*
            // --- 日付表示 ---
            Date_obj.SetActive(true);
            Date_txt.text = ""; // 表示前にクリア
            yield return StartCoroutine(KakuText(Date_txt, Date_string[currentTextIndex]));
            */

            // 日付から日記本文へは自動で進むように少し待機
            yield return new WaitForSeconds(0f);

            // --- 日記内容表示 ---
            NikkiContent_obj.SetActive(true);
            NikkiContent_txt.text = ""; // 表示前にクリア
            isNikkiContentTyping = true; // スキップ機能を有効にする
            yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[currentTextIndex]));
            isNikkiContentTyping = false; // スキップ機能を無効にする


            // --- 読める上限に達したかチェック ---
            if (currentTextIndex >= CanReadNikkiContent_num)
            {
                // 上限に達した。ループを抜けて、テキストは表示したままにする
                break;
            }

            // 上限に達していない場合は、Enterキー入力で次の日記へ進む
            yield return StartCoroutine(WaitForEnterKey());

            // 次の日記へ
            currentTextIndex++;
        }

        yield return new WaitForSeconds(1f);
        //ジャンプUI表示
        Jump_obj.SetActive(true);
        TextMeshProUGUI Jump_txt = Jump_obj.GetComponent<TextMeshProUGUI>();
        yield return StartCoroutine(KakuText(Jump_txt, "ジャンプ"));
        yield return new WaitForSeconds(0.5f);
        SpriteRenderer WorSpace_SR = WorSpace_obj.GetComponent<SpriteRenderer>();
        WorSpace_obj.SetActive(true);
        yield return StartCoroutine(FadeIn(WorSpace_SR,1));

        //ステージ表示(玄関ドア)
        GenkanDoor_obj.SetActive(true);
        SpriteRenderer GenkanDoor_SR = GenkanDoor_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn(GenkanDoor_SR,1));
        GenkanDoorShadow_obj.SetActive(true);
        SpriteRenderer GenkanDoorShadow_SR = GenkanDoorShadow_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn(GenkanDoorShadow_SR,1));
        GenkanDoorFloorShadow_obj.SetActive(true);
        SpriteRenderer GenkanDoorFloorShadow_SR = GenkanDoorFloorShadow_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn(GenkanDoorFloorShadow_SR,1));

        //プレイヤー動ける
        PC.playerCanMove_b = true;
        PC.jumpCan_b = true;


    }
    // --- ここまで修正したJumpSceneメソッド ---
    IEnumerator DrawLineScene()
    {
        PC.player_anim.SetBool("walk", false);
        PC.playerCanMove_b = false;
        yield return new WaitForSeconds(1f);
        SpriteRenderer GenkanDoorShadow_SR = GenkanDoorShadow_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer GenkanDoor_SR = GenkanDoor_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer GenkanDoorFloorShadow_SR = GenkanDoorFloorShadow_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(GenkanDoorShadow_SR));
        StartCoroutine(FadeOut(GenkanDoor_SR));
        StartCoroutine(FadeOut(GenkanDoorFloorShadow_SR));
        GenkanDoor_obj.SetActive(false);
        SpriteRenderer GenkanDoorOpen_SR = GenkanDoorOpen_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer GenkanDoorOpenShadow_SR = GenkanDoorOpenShadow_obj.GetComponent<SpriteRenderer>();
        GenkanDoorOpen_obj.SetActive(true);
        GenkanDoorOpenShadow_obj.SetActive(true);
        StartCoroutine(FadeIn(GenkanDoorOpen_SR,1));
        StartCoroutine(FadeIn(GenkanDoorOpenShadow_SR,1));

        SpriteRenderer WorSpace_SR = WorSpace_obj.GetComponent<SpriteRenderer>();
        TextMeshProUGUI Jump_TMPro = Jump_obj.GetComponent<TextMeshProUGUI>();
        StartCoroutine(FadeOut(WorSpace_SR));
        StartCoroutine(FadeOutText(Jump_TMPro));

        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOut(GenkanDoorOpen_SR));
        StartCoroutine(FadeOut(GenkanDoorOpenShadow_SR));

        yield return new WaitForSeconds(1f);
        GenkanDoorOpen_obj.SetActive(false);

        yield return new WaitForSeconds(2f);
        // --- ここから日記表示処理 ---
        CanReadNikkiContent_num = 10; // 新しい日記を読めるように設定
        currentTextIndex++; // 次の日記に進む

        // 日記の表示ループ
        while (true)
        {
            // 配列の範囲外になったらループを抜ける
            if (currentTextIndex >= NikkiContent_string.Length)
            {
                break;
            }
            /*
            // --- 日付表示 ---
            Date_obj.SetActive(true);
            Date_txt.text = ""; // 表示前にクリア
            yield return StartCoroutine(KakuText(Date_txt, Date_string[currentTextIndex]));
            */

            // 日付から日記本文へは自動で進むように少し待機
            yield return new WaitForSeconds(0f);

            // --- 日記内容表示 ---
            NikkiContent_obj.SetActive(true);
            NikkiContent_txt.text = ""; // 表示前にクリア
            isNikkiContentTyping = true; // スキップ機能を有効にする
            yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[currentTextIndex]));
            isNikkiContentTyping = false; // スキップ機能を無効にする


            // --- 読める上限に達したかチェック ---
            if (currentTextIndex >= CanReadNikkiContent_num)
            {
                // 上限に達した。ループを抜けて、テキストは表示したままにする
                break;
            }

            // 上限に達していない場合は、Enterキー入力で次の日記へ進む
            yield return StartCoroutine(WaitForEnterKey());

            // 次の日記へ
            currentTextIndex++;
        }

        yield return new WaitForSeconds(1f);
        SpriteRenderer Mouse_SR = Mouse_obj.GetComponent<SpriteRenderer>();
        TextMeshProUGUI Kakikomu_SR = Kakikomu_obj.GetComponent<TextMeshProUGUI>();
        SpriteRenderer Oka_SR = Oka_obj.GetComponent<SpriteRenderer>();
        Mouse_obj.SetActive(true);
        Kakikomu_obj.SetActive(true);
        StartCoroutine(FadeInText(Kakikomu_SR));
        StartCoroutine(FadeIn(Mouse_SR,1));
        yield return new WaitForSeconds(1f);
        Oka_obj.SetActive(true);
        StartCoroutine(FadeIn(Oka_SR,1));

        PC.playerCanMove_b = true;
        PC.KakikomuCan_b = true;

    }

    IEnumerator Stage1_1Scene()
    {
        PC.player_anim.SetBool("walk", false);
        PC.playerCanMove_b = false;

        SpriteRenderer Mouse_SR = Mouse_obj.GetComponent<SpriteRenderer>();
        TextMeshProUGUI Kakikomu_TMPro = Kakikomu_obj.GetComponent<TextMeshProUGUI>();
        StartCoroutine(FadeOut(Mouse_SR));
        StartCoroutine(FadeOutText(Kakikomu_TMPro));

        yield return new WaitForSeconds(1.5f);

        SpriteRenderer Yuuchan_SR = Yuuchan_obj.GetComponent<SpriteRenderer>();
        Yuuchan_obj.SetActive(true);
        yield return new WaitForSeconds(1f);
        // --- ここから日記表示処理 ---
        CanReadNikkiContent_num = 12; // 新しい日記を読めるように設定
        currentTextIndex++; // 次の日記に進む

        // 日記の表示ループ
        while (true)
        {
            // 配列の範囲外になったらループを抜ける
            if (currentTextIndex >= NikkiContent_string.Length)
            {
                break;
            }
            /*
            // --- 日付表示 ---
            Date_obj.SetActive(true);
            Date_txt.text = ""; // 表示前にクリア
            yield return StartCoroutine(KakuText(Date_txt, Date_string[currentTextIndex]));
            */

            // 日付から日記本文へは自動で進むように少し待機
            yield return new WaitForSeconds(0f);

            // --- 日記内容表示 ---
            NikkiContent_obj.SetActive(true);
            NikkiContent_txt.text = ""; // 表示前にクリア
            isNikkiContentTyping = true; // スキップ機能を有効にする
            yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[currentTextIndex]));
            isNikkiContentTyping = false; // スキップ機能を無効にする


            // --- 読める上限に達したかチェック ---
            if (currentTextIndex >= CanReadNikkiContent_num)
            {
                // 上限に達した。ループを抜けて、テキストは表示したままにする
                break;
            }

            // 上限に達していない場合は、Enterキー入力で次の日記へ進む
            yield return StartCoroutine(WaitForEnterKey());

            // 次の日記へ
            currentTextIndex++;
        }

        yield return new WaitForSeconds(1f);
        Yuuchan_obj.SetActive(false);
        yield return new WaitForSeconds(1f);
        SpriteRenderer Oka_SR = Oka_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(Oka_SR));
        yield return new WaitForSeconds(1f);
        Oka_obj.SetActive(false);

        //ここに
        // --- ここから日記表示処理 ---
        CanReadNikkiContent_num = 14; // 新しい日記を読めるように設定
        currentTextIndex++; // 次の日記に進む

        // 日記の表示ループ
        while (true)
        {
            // 配列の範囲外になったらループを抜ける
            if (currentTextIndex >= NikkiContent_string.Length)
            {
                break;
            }
            /*
            // --- 日付表示 ---
            Date_obj.SetActive(true);
            Date_txt.text = ""; // 表示前にクリア
            yield return StartCoroutine(KakuText(Date_txt, Date_string[currentTextIndex]));
            */

            // 日付から日記本文へは自動で進むように少し待機
            yield return new WaitForSeconds(0f);

            // --- 日記内容表示 ---
            NikkiContent_obj.SetActive(true);
            NikkiContent_txt.text = ""; // 表示前にクリア
            isNikkiContentTyping = true; // スキップ機能を有効にする
            yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[currentTextIndex]));
            isNikkiContentTyping = false; // スキップ機能を無効にする


            // --- 読める上限に達したかチェック ---
            if (currentTextIndex >= CanReadNikkiContent_num)
            {
                // 上限に達した。ループを抜けて、テキストは表示したままにする
                break;
            }

            // 上限に達していない場合は、Enterキー入力で次の日記へ進む
            yield return StartCoroutine(WaitForEnterKey());

            // 次の日記へ
            currentTextIndex++;
        }


        yield return new WaitForSeconds(1f);
        SpriteRenderer River_SR = River_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer RiverShadow_SR = RiverShadow_obj.GetComponent<SpriteRenderer>();
        River_obj.SetActive(true);
        RiverShadow_obj.SetActive(true);
        RistartPos_obj.SetActive(true);
        StartCoroutine(FadeIn(River_SR,1));
        StartCoroutine(FadeIn(RiverShadow_SR,1));
        yield return new WaitForSeconds(0.5f);
        deleteArea_obj[3].SetActive(true);
        PC.playerCanMove_b = true;
        
    }

    IEnumerator Stage1_2Scene()
    {
        yield return new WaitForSeconds(0);
        PC.player_anim.SetBool("walk", false);
        PC.playerCanMove_b = false;

        SpriteRenderer river_SR = River_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer riverShadow_SR = RiverShadow_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(river_SR));
        StartCoroutine(FadeOut(riverShadow_SR));
        yield return new WaitForSeconds(1f);
        River_obj.SetActive(false);
        RiverShadow_obj.SetActive(false);

        // --- ここから日記表示処理 ---
        CanReadNikkiContent_num = 16; // 新しい日記を読めるように設定
        currentTextIndex++; // 次の日記に進む

        // 日記の表示ループ
        while (true)
        {
            // 配列の範囲外になったらループを抜ける
            if (currentTextIndex >= NikkiContent_string.Length)
            {
                break;
            }
            /*
            // --- 日付表示 ---
            Date_obj.SetActive(true);
            Date_txt.text = ""; // 表示前にクリア
            yield return StartCoroutine(KakuText(Date_txt, Date_string[currentTextIndex]));
            */

            // 日付から日記本文へは自動で進むように少し待機
            yield return new WaitForSeconds(0f);

            // --- 日記内容表示 ---
            NikkiContent_obj.SetActive(true);
            NikkiContent_txt.text = ""; // 表示前にクリア
            isNikkiContentTyping = true; // スキップ機能を有効にする
            yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[currentTextIndex]));
            isNikkiContentTyping = false; // スキップ機能を無効にする


            // --- 読める上限に達したかチェック ---
            if (currentTextIndex >= CanReadNikkiContent_num)
            {
                // 上限に達した。ループを抜けて、テキストは表示したままにする
                break;
            }

            // 上限に達していない場合は、Enterキー入力で次の日記へ進む
            yield return StartCoroutine(WaitForEnterKey());

            // 次の日記へ
            currentTextIndex++;
        }

        yield return new WaitForSeconds(1f);
        SpriteRenderer Tree_SR = Tree_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer TreeShadow_SR = TreeShadow_obj.GetComponent<SpriteRenderer>();
        Tree_obj.SetActive(true);
        TreeShadow_obj.SetActive(true);
        StartCoroutine(FadeIn(Tree_SR, 1));
        StartCoroutine(FadeIn(TreeShadow_SR, 1));

        SpriteRenderer Karasu_SR = Karasu_obj.GetComponent<SpriteRenderer>();
        Karasu_obj.SetActive(true);
        StartCoroutine(FadeIn(Karasu_SR, 1));

        SpriteRenderer Karasu_fly_SR = Karasu_fly_obj.GetComponent<SpriteRenderer>();
        Karasu_fly_obj.SetActive(true);
        StartCoroutine(FadeIn(Karasu_fly_SR, 1));
        Karasu_AttackArea_obj.SetActive(true);
        RistartPos_obj.transform.position = new Vector3(5.77f, -2.7f, 0);
        yield return new WaitForSeconds(0.5f);
        Yuuchan_karasuStage_obj.SetActive(true);
        deleteArea_obj[4].SetActive(true);
        PC.playerCanMove_b = true;
    }

    IEnumerator Stage1_3Scene()
    {
        PC.player_anim.SetBool("walk", false);
        PC.playerCanMove_b = false;
        yield return new WaitForSeconds(1f);
        //ゆうちゃん削除
        Yuuchan_obj.SetActive(false);
        Yuuchan_karasuStage_obj.SetActive(false);
        ///木の枝消える
        yield return new WaitForSeconds(1);
        SpriteRenderer Tree_SR = Tree_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer TreeShadow_SR = TreeShadow_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer Karasu_stand_SR = Karasu_obj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(Tree_SR));
        StartCoroutine(FadeOut(TreeShadow_SR));
        StartCoroutine(FadeOut(Karasu_stand_SR));
        Tree_obj.SetActive(false);
        yield return new WaitForSeconds(1);

        // --- ここから日記表示処理 ---
        CanReadNikkiContent_num = 18; // 新しい日記を読めるように設定
        currentTextIndex++; // 次の日記に進む

        // 日記の表示ループ
        while (true)
        {
            // 配列の範囲外になったらループを抜ける
            if (currentTextIndex >= NikkiContent_string.Length)
            {
                break;
            }
            /*
            // --- 日付表示 ---
            Date_obj.SetActive(true);
            Date_txt.text = ""; // 表示前にクリア
            yield return StartCoroutine(KakuText(Date_txt, Date_string[currentTextIndex]));
            */

            // 日付から日記本文へは自動で進むように少し待機
            yield return new WaitForSeconds(0f);

            // --- 日記内容表示 ---
            NikkiContent_obj.SetActive(true);
            NikkiContent_txt.text = ""; // 表示前にクリア
            isNikkiContentTyping = true; // スキップ機能を有効にする
            yield return StartCoroutine(KakuText(NikkiContent_txt, NikkiContent_string[currentTextIndex]));
            isNikkiContentTyping = false; // スキップ機能を無効にする


            // --- 読める上限に達したかチェック ---
            if (currentTextIndex >= CanReadNikkiContent_num)
            {
                // 上限に達した。ループを抜けて、テキストは表示したままにする
                break;
            }

            // 上限に達していない場合は、Enterキー入力で次の日記へ進む
            yield return StartCoroutine(WaitForEnterKey());

            // 次の日記へ
            currentTextIndex++;
        }

        //ステージの表示
        SpriteRenderer RoadInForest_SR = RoadInForest_obj.GetComponent<SpriteRenderer>();
        SpriteRenderer RoadInForestShadow_SR = RoadInForestShadow_obj.GetComponent<SpriteRenderer>();
        RoadInForestShadow_obj.SetActive(true);
        RoadInForest_obj.SetActive(true);
        StartCoroutine(FadeIn(RoadInForest_SR, 1));
        StartCoroutine(FadeIn(RoadInForestShadow_SR,1));

        yield return new WaitForSeconds(1f);
        PC.playerCanMove_b = true;

        NextScene_obj.SetActive(true);
    }


    // --- ここから修正したメソッド ---
    /// <summary>
    /// テキスト表示が完了してから、Enterキーが押されるのを待つコルーチン
    /// </summary>
    IEnumerator WaitForEnterKey()
    {
        // isTypingがfalseになるまで（＝テキスト表示が終わるまで）待つ
        yield return new WaitUntil(() => !isTyping);

        // --- Enterオブジェクトを表示 ---
        // 最後の文章でなければEnterアイコンを表示
        if (currentTextIndex < CanReadNikkiContent_num)
        {
            Enter_obj.SetActive(true);
            SpriteRenderer Enter_SR = Enter_obj.GetComponent<SpriteRenderer>();
            StartCoroutine(FadeIn(Enter_SR,0.3f));
        }

        // Enterキーが押されるまで待つ
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

        // --- Enterオブジェクトを非表示 ---
        Enter_obj.SetActive(false);
    }
    // --- ここまで修正したメソッド ---


    IEnumerator FadeIn(SpriteRenderer SR, float FadeInTime_f)
    {
        float FinishTime_f = FadeInTime_f;
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

    // --- ここから修正したKakuTextメソッド ---
    IEnumerator KakuText(TextMeshProUGUI targetText, string content)
    {
        // --- 状態フラグを初期化 ---
        isTyping = true;
        skipTyping = false;

        // 最初にテキストコンポーネント自体のアルファを1（不透明）にしておく
        Color baseColor = targetText.color;
        baseColor.a = 1f;
        targetText.color = baseColor;

        float waitTime = 0.1f;

        // サウンドを再生
        if (writing_sound != null && !writing_sound.isPlaying)
        {
            writing_sound.Play();
        }

        // 1. 全文をテキストコンポーネントにセットする
        targetText.text = content;

        // 2. テキスト情報を強制的に更新して、文字ごとの情報を生成させる
        targetText.ForceMeshUpdate();

        TMP_TextInfo textInfo = targetText.textInfo;
        int totalCharacters = textInfo.characterCount;

        // 3. 全ての文字の頂点カラーを透明にする
        for (int i = 0; i < totalCharacters; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;
            Color32 transparentColor = vertexColors[vertexIndex + 0];
            transparentColor.a = 0;

            vertexColors[vertexIndex + 0] = transparentColor;
            vertexColors[vertexIndex + 1] = transparentColor;
            vertexColors[vertexIndex + 2] = transparentColor;
            vertexColors[vertexIndex + 3] = transparentColor;
        }
        targetText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        // 4. 1文字ずつ透明度を戻していく
        for (int i = 0; i < totalCharacters; i++)
        {
            // --- Enterが押されてスキップフラグが立ったら、ループを抜ける ---
            if (skipTyping)
            {
                break;
            }

            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;

            Color32 opaqueColor = vertexColors[vertexIndex + 0];
            opaqueColor.a = 255;
            vertexColors[vertexIndex + 0] = opaqueColor;
            vertexColors[vertexIndex + 1] = opaqueColor;
            vertexColors[vertexIndex + 2] = opaqueColor;
            vertexColors[vertexIndex + 3] = opaqueColor;

            targetText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            yield return new WaitForSeconds(waitTime);
        }

        // --- スキップされた場合でも、全ての文字を不透明にする処理 ---
        // これにより、全文が瞬時に表示される
        for (int i = 0; i < totalCharacters; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;
            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;
            Color32 opaqueColor = vertexColors[vertexIndex + 0];
            opaqueColor.a = 255;
            vertexColors[vertexIndex + 0] = opaqueColor;
            vertexColors[vertexIndex + 1] = opaqueColor;
            vertexColors[vertexIndex + 2] = opaqueColor;
            vertexColors[vertexIndex + 3] = opaqueColor;
        }
        targetText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);


        // サウンドを停止
        if (writing_sound != null && writing_sound.isPlaying)
        {
            writing_sound.Stop();
        }

        // --- 状態フラグを戻す ---
        isTyping = false;
    }
    // --- ここまで修正したKakuTextメソッド ---
}

