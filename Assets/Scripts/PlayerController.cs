using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 追加: TextMeshProUGUIを使用しないが、元のスクリプトに含まれていたため保持
using TMPro; // 追加: TextMeshProUGUIを使用しないが、元のスクリプトに含まれていたため保持

public class PlayerController : MonoBehaviour
{
    [Header("Playerコンポーネント情報")]
    public Animator player_anim;
    public Rigidbody2D player_rb;
    public float player_x, player_y;

    [Header("Player移動情報")]
    public bool playerCanMove_b;
    public bool AorDCan_b;
    public bool jumpCan_b;
    public bool KakikomuCan_b;
    public float walkSpeed_f = 2;
    public float jumpPower_f = 50;
    public bool isJumping_b = false;
    [Header("サウンド関連")]
    AudioSource audioSource;
    public AudioClip walk_sound;

    [Header("その他の情報")]
    public Stage1Controller SOC;
    public GameObject playerShadow_obj;
    public GameObject GameOverShadow_obj;
    public GameObject RestartPos_obj;
    public bool GameOver_b = false;
    public bool KarasuAttack_b = false;
    private bool isGameOverRoutineRunning = false; // ★追加: GameOverコルーチンが実行中かどうかのフラグ


    void Start()
    {
        player_anim = this.GetComponent<Animator>();
        player_rb = this.GetComponent<Rigidbody2D>();
        SOC = GameObject.FindWithTag("Stage1Controller").GetComponent<Stage1Controller>();
        GameOverShadow_obj.SetActive(false);

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
            StartCoroutine(GameOver());
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

    IEnumerator GameOver()
    {
        isGameOverRoutineRunning = true; // ★追加: コルーチン開始時にフラグを立てる
        playerCanMove_b = false;

        // Restart位置確認
        if (RestartPos_obj == null)
        {
            RestartPos_obj = GameObject.FindWithTag("RestartPos");
        }

        // GameOver影の表示＋アルファ初期化
        SpriteRenderer GameOverShadow_SR = GameOverShadow_obj.GetComponent<SpriteRenderer>();
        if (GameOverShadow_SR != null)
        {
            Color c = GameOverShadow_SR.color;
            c.a = 1f; // 完全表示
            GameOverShadow_SR.color = c;
        }

        GameOverShadow_obj.SetActive(true);
        SpriteRenderer player_SR = GetComponent <SpriteRenderer>();
        yield return StartCoroutine(FadeOut(player_SR));

        // リスポーン
        yield return new WaitForSeconds(0f);
        this.transform.position = RestartPos_obj.transform.position;


        // フェードアウト処理
        yield return StartCoroutine(FadeOutShadowandFadeInPlayer(GameOverShadow_SR,player_SR));
        
        playerCanMove_b = true;
        GameOver_b = false;
        isGameOverRoutineRunning = false; // ★追加: コルーチン終了時にフラグを解除
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
    IEnumerator FadeOutShadowandFadeInPlayer(SpriteRenderer SR,SpriteRenderer player_SR)
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
            SOC.stage1Finish_b = true;
            
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Stage1_Area")
        {
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

        if (other.gameObject.tag == "KarasuAttackArea")
        {
            KarasuAttack_b = true;
        }
    }
}