using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 追加: TextMeshProUGUIを使用しないが、元のスクリプトに含まれていたため保持
using TMPro; // 追加: TextMeshProUGUIを使用しないが、元のスクリプトに含まれていたため保持

public class PlayerController : MonoBehaviour
{
    [Header("ステージNUM")]
    public int stageNum;
    [Header("プレイヤー状態情報")]
    public bool isHanten_b;
    [Header("Playerコンポーネント情報")]
    public Animator player_anim;
    public Rigidbody2D player_rb;
    public float player_x, player_y;

    [Header("Player移動情報")]
    public bool playerCanMove_b;
    public bool AorDCan_b;
    public bool jumpCan_b;
    public bool KakikomuCan_b;
    public float walkSpeed_f = 0.0015f;
    public float jumpPower_f = 50;
    public bool isJumping_b = false;
    [Header("サウンド関連")]
    public AudioSource audioSource;
    public AudioClip walk_sound;

    [Header("その他の情報")]
    public Scene1Controller SOC;
    public StageFourController SFC;
    public GameObject playerShadow_obj;
    public GameObject GameOverShadow_obj;
    public GameObject GameOverShadow_hanten_obj;
    public SpriteRenderer GameOverShadow_SR;
    public GameObject RestartPos_obj;
    public bool GameOver_b = false;
    public bool KarasuAttack_b = false;
    private bool isGameOverRoutineRunning = false; // ★追加: GameOverコルーチンが実行中かどうかのフラグ
    [Header("パートナー情報")]
    public YuuchanController yuuchan_scr;
    public GameObject Yuuchan_obj;

    void Start()
    {
        player_anim = this.GetComponent<Animator>();
        player_rb = this.GetComponent<Rigidbody2D>();
        switch (stageNum)
        {
            case 1:
                SOC = GameObject.FindWithTag("Scene1Controller").GetComponent<Scene1Controller>();
                break;
            case 4:
                SFC = GameObject.FindWithTag("Stage4Controller").GetComponent<StageFourController>();

                // ★追加: ステージ4の場合のみ、シーン内のYuuchanを探して変数に格納します。
                yuuchan_scr = FindObjectOfType<YuuchanController>();
                if (yuuchan_scr == null)
                {
                    // 見つからなくてもエラーにはせず、警告を出すに留めます。
                    Debug.LogWarning("ステージ4ですが、YuuchanControllerが見つかりません！");
                }
                break;

        }

        GameOverShadow_obj.SetActive(false);
        if (GameOverShadow_hanten_obj != null)
        {
            GameOverShadow_hanten_obj.SetActive(false);
        }
        //サウンド関連
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 移動
        if (playerCanMove_b)
        {
            if (AorDCan_b)
            {
                bool isMoving = false;
                /*
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    this.transform.position += new Vector3(walkSpeed_f, 0, 0);
                    this.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                    player_anim.SetBool("walk", true);
                    isMoving = true;
                }
                else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    this.transform.position += new Vector3(-walkSpeed_f, 0, 0);
                    this.transform.localScale = new Vector3(-0.15f, 0.15f, 0.15f);
                    player_anim.SetBool("walk", true);
                    isMoving = true;
                }
                else
                {
                    player_anim.SetBool("walk", false);
                }
                */
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    // ★★★ 対策 ★★★
                    // Time.deltaTimeを掛けてフレームレートに依存しないようにする
                    this.transform.position += new Vector3(walkSpeed_f * Time.deltaTime, 0, 0);
                    this.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                    player_anim.SetBool("walk", true);
                    isMoving = true;
                }
                else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    // ★★★ 対策 ★★★
                    this.transform.position += new Vector3(-walkSpeed_f * Time.deltaTime, 0, 0);
                    this.transform.localScale = new Vector3(-0.15f, 0.15f, 0.15f);
                    player_anim.SetBool("walk", true);
                    isMoving = true;
                }
                else
                {
                    player_anim.SetBool("walk", false);
                }

                // 足音再生処理（移動中かつジャンプしていないときのみ）
                if (isMoving && !isJumping_b)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = walk_sound;
                        audioSource.loop = true; // 足音をループ再生するなら true
                        audioSource.Play();
                    }
                }
                else
                {
                    if (audioSource.isPlaying && audioSource.clip == walk_sound)
                    {
                        audioSource.Stop();
                    }
                }
            }

            if (jumpCan_b)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (!isJumping_b)
                    {
                        isJumping_b = true;
                        player_rb.AddForce(new Vector2(0, jumpPower_f));
                        float x = this.transform.position.x;
                        playerShadow_obj.transform.position = new Vector3(x, -3.115063f, 0);
                    }
                }
            }
        }
        else
        {
            audioSource.Stop();
        }

        Shadow(0);

        //ゲームオーバー処理開始（1回だけ実行）
        // ★修正点: GameOver_bがtrue かつ GameOverコルーチンが実行中でない場合のみ開始
        if (GameOver_b && !isGameOverRoutineRunning)
        {
            // ステージ4かつ、yuuchan_scrが見つかっている場合
            if (stageNum == 4 && Yuuchan_obj != null && Yuuchan_obj.activeInHierarchy)
            {
                Debug.Log("withYuuchanGameOverを呼び出す");
                StartCoroutine(withYuuchanGameOver());
            }
            else
            {
                Debug.Log("SoloGameOver を呼び出す");
                StartCoroutine(SoloGameOver());
            }
        }

        //プレイヤー反転
        if (isHanten_b)
        {
            player_anim.SetBool("hanten", true);
        }
        else
        {
            player_anim.SetBool("hanten", false);
        }


    }

    public void Shadow(float Floor_y)
    {
        player_x = this.transform.position.x;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 12f, LayerMask.GetMask("Default"));

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("floor"))
            {
                Floor_y = hit.point.y + 0.85f;
                Vector3 shadow_vec = new Vector3(player_x, Floor_y, 0);
                playerShadow_obj.transform.position = shadow_vec;
                playerShadow_obj.SetActive(true);
            }
        }
    }

    IEnumerator SoloGameOver()
    {

        isGameOverRoutineRunning = true;
        playerCanMove_b = false;

        // Restart位置確認
        if (RestartPos_obj == null)
        {
            RestartPos_obj = GameObject.FindWithTag("RestartPos");
        }

        // GameOver影の表示＋アルファ初期化
        if (!isHanten_b)
        {
            GameOverShadow_SR = GameOverShadow_obj.GetComponent<SpriteRenderer>();
        }
        else
        {
            GameOverShadow_SR = GameOverShadow_hanten_obj.GetComponent<SpriteRenderer>();
        }
        if (GameOverShadow_SR != null || GameOverShadow_hanten_obj != null)
        {
            Color c = GameOverShadow_SR.color;
            c.a = 1f; // 完全表示
            GameOverShadow_SR.color = c;
        }

        if (!isHanten_b)
        {
            GameOverShadow_obj.SetActive(true);
        }
        else
        {
            GameOverShadow_hanten_obj.SetActive(true);
        }
        SpriteRenderer player_SR = GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeOut(player_SR));

        // リスポーン
        yield return new WaitForSeconds(0f);
        this.transform.position = RestartPos_obj.transform.position;


        // フェードアウト処理
        yield return StartCoroutine(FadeOutShadowandFadeInPlayer(GameOverShadow_SR, player_SR));

        playerCanMove_b = true;
        GameOver_b = false;
        isGameOverRoutineRunning = false; // ★追加: コルーチン終了時にフラグを解除
    }

    // ★追加: ステージ4専用の、Yuuchanと連携するマスターゲームオーバー処理です。
    IEnumerator withYuuchanGameOver()
    {
        isGameOverRoutineRunning = true;
        playerCanMove_b = false;

        // --- 両キャラクターのSpriteRendererを取得 ---
        SpriteRenderer player_sr = GetComponent<SpriteRenderer>();
        SpriteRenderer yuuchan_sr = yuuchan_scr.GetComponent<SpriteRenderer>();

        // --- PlayerのGameOver影の準備 ---
        if (!isHanten_b)
        {
            GameOverShadow_SR = GameOverShadow_obj.GetComponent<SpriteRenderer>();
            GameOverShadow_obj.SetActive(true);
        }
        else
        {
            GameOverShadow_SR = GameOverShadow_hanten_obj.GetComponent<SpriteRenderer>();
            GameOverShadow_hanten_obj.SetActive(true);
        }
        if (GameOverShadow_SR != null)
        {
            Color c = GameOverShadow_SR.color;
            c.a = 1f;
            GameOverShadow_SR.color = c;
        }

        // --- YuuchanのGameOver影の準備 ---
        SpriteRenderer yuuchanShadow_sr = yuuchan_scr.yuuchanGameOverShadow_obj.GetComponent<SpriteRenderer>();
        yuuchan_scr.yuuchanGameOverShadow_obj.SetActive(true);
        if (yuuchanShadow_sr != null)
        {
            Color c_y = yuuchanShadow_sr.color;
            c_y.a = 1f;
            yuuchanShadow_sr.color = c_y;
        }

        // --- 両キャラクターを同時にフェードアウト ---
        StartCoroutine(FadeOut(player_sr));
        yield return StartCoroutine(FadeOut(yuuchan_sr));

        // --- 両キャラクターをリスポーン位置へ移動 ---
        if (RestartPos_obj == null) RestartPos_obj = GameObject.FindWithTag("RestartPos");
        this.transform.position = RestartPos_obj.transform.position;
        if (yuuchan_scr.yuuchanRestartPos_obj != null)
        {
            yuuchan_scr.transform.position = yuuchan_scr.yuuchanRestartPos_obj.transform.position;
        }

        // --- 連携用のフェードイン処理を呼び出し ---
        yield return StartCoroutine(FadeOutShadowsAndFadeInCharacters_co(GameOverShadow_SR, yuuchanShadow_sr, player_sr, yuuchan_sr));

        // --- 状態をリセット ---
        playerCanMove_b = true;
        GameOver_b = false;
        isGameOverRoutineRunning = false;
    }

    // ★追加: 2キャラ同時にフェードイン・影をフェードアウトさせる連携用コルーチンです。
    IEnumerator FadeOutShadowsAndFadeInCharacters_co(SpriteRenderer playerShadow_sr, SpriteRenderer yuuchanShadow_sr, SpriteRenderer player_sr, SpriteRenderer yuuchan_sr)
    {
        float FinishTime_f = 1f;
        float NowTime_f = 0f;
        Color c_p_shadow = playerShadow_sr.color;
        Color c_y_shadow = yuuchanShadow_sr.color;
        Color c_p = player_sr.color;
        Color c_y = yuuchan_sr.color;
        c_p.a = 0f; c_y.a = 0f;
        player_sr.color = c_p;
        yuuchan_sr.color = c_y;
        while (NowTime_f < FinishTime_f)
        {
            NowTime_f += Time.deltaTime;
            float progress = NowTime_f / FinishTime_f;
            c_p_shadow.a = Mathf.Clamp01(1 - progress); playerShadow_sr.color = c_p_shadow;
            c_y_shadow.a = Mathf.Clamp01(1 - progress); yuuchanShadow_sr.color = c_y_shadow;
            c_p.a = Mathf.Clamp01(progress); player_sr.color = c_p;
            c_y.a = Mathf.Clamp01(progress); yuuchan_sr.color = c_y;
            yield return null;
        }
        c_p_shadow.a = 0f; playerShadow_sr.color = c_p_shadow;
        c_y_shadow.a = 0f; yuuchanShadow_sr.color = c_y_shadow;
        GameOverShadow_obj.SetActive(false);
        GameOverShadow_hanten_obj.SetActive(false);
        if (yuuchan_scr != null) yuuchan_scr.yuuchanGameOverShadow_obj.SetActive(false);
    }

    IEnumerator FadeIn(SpriteRenderer SR)
    {
        float FinishTime_f = 1f;
        float NowTime_f = 0f;

        Color c = SR.color;
        c.a = 1f;
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
    IEnumerator FadeOutShadowandFadeInPlayer(SpriteRenderer SR, SpriteRenderer player_SR)
    {
        float FinishTime_f = 1f;
        float NowTime_f = 0f;

        Color c = SR.color;
        c.a = 1f;
        SR.color = c;
        Color c_p = player_SR.color;
        c_p.a = 0f;
        player_SR.color = c_p;

        while (NowTime_f < FinishTime_f)
        {
            NowTime_f += Time.deltaTime;
            c.a = Mathf.Clamp01(1 - (NowTime_f / FinishTime_f));
            SR.color = c;
            c_p.a = Mathf.Clamp01(NowTime_f / FinishTime_f);
            player_SR.color = c_p;
            yield return null;

        }
        c.a = 0f;
        SR.color = c;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "floor")
        {
            if (isJumping_b)
            {
                isJumping_b = false;
            }
        }

        if (other.gameObject.tag == "Enemy")
        {
            GameOver_b = true;
        }

        if (other.gameObject.tag == "OpenDoor")
        {
            playerCanMove_b = false;
            player_anim.SetBool("walk", false);
            KakikomuCan_b = false;
            //SOC.stage1Finish_b = true;
        }

        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Stage1_Area")
        {
            //ステージ1
            if (other.gameObject.name == "AorD_Delete_Area")
            {
                SOC.stage1Num_i = 1;

            }
            else if (other.gameObject.name == "Jump_Delete_Area")
            {
                SOC.stage1Num_i = 2;

            }
            else if (other.gameObject.name == "kakikomi_delete_Area")
            {
                SOC.stage1Num_i = 3;

            }
            else if (other.gameObject.name == "River_delete_Area")
            {
                SOC.stage1Num_i = 4;

            }
            else if (other.gameObject.name == "Tree_karasu_delete_Area ")
            {
                SOC.stage1Num_i = 5;
            }

            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "ScrollCollider")
        {

            //コライダーに触れたとき
            //スクロールスタートがfalseなら、スクロールスタートをtrueに&speed を。スクロールスタートがtrueならfalseに,speed をもとに戻す。
            if (!SOC.ScrollStart_b)
            {
                SOC.ScrollStart_b = true;
                walkSpeed_f = 0;
            }
            /*
            else
            {
                SOC.ScrollStart_b = false;
                walkSpeed_f = 0.0015f;
            }
            */
        }

        //ステージ4
        if (other.gameObject.tag == "Stage4_Area")
        {
            if (other.gameObject.name == "RoomDelete_Area")
            {
                SFC.stage4Num_i = 1;
            }
            else if (other.gameObject.name == "TreeHantenDelete_Area")
            {
                SFC.stage4Num_i = 3;
            }
            else if (other.gameObject.name == "RiverDelete_Area")
            {
                SFC.stage4Num_i = 4;
            }
            else if (other.gameObject.name == "OkaDelete_Area")
            {
                SFC.stage4Num_i = 5;
            }
            else if (other.gameObject.name == "bedDelete_Area")
            {
                SFC.stage4Num_i = 6;
            }

            other.gameObject.SetActive(false);
        }


        if (other.gameObject.tag == "KarasuAttackArea")
        {
            KarasuAttack_b = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "ScrollCollider")
        {
            if (SOC.ScrollStart_b)
            {
                SOC.ScrollStart_b = false;
                walkSpeed_f = 0.8f;
            }
        }
    }
}