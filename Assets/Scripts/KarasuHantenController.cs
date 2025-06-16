/*
    KarasuHantenControllerが満たす要件：
    ・プレイヤーを発見したら、最初の位置を記憶し、そこに向かって直進 → OnEnable() と Case0() で実現
    ・Enemyタグ以外に当たったら、FadeOutするように → OnCollisionEnter2D() で実現
    ・PlayerControllerのGameOver_bがtrueになったら、再度攻撃 → Spawnerに再起動された際のOnEnable()で実現
    ・（元の位置に戻る処理はSpawner側が担当します）
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarasuHantenController : MonoBehaviour
{
    // --- インスペクターで設定 ---
    [Header("カラスのタイプ")]
    public int karasuNum_i; // 0: 直進タイプ

    [Header("カラスの性能")]
    public float speed_f = 3f;

    // --- 内部で自動取得・管理 ---
    private PlayerController PC;
    private Transform player_trans;
    private SpriteRenderer karasu_SR;
    private AudioSource karasu_audio;
    
    private Vector3 initialPosition_vec;
    private Vector3 initialScale_vec;
    private Vector2 moveDirection_vec;
    private bool hasAttackDirection_b = false;
    private bool move_b = false;

    // ゲーム開始時に一度だけ呼ばれる
    void Awake()
    {
        // 自身のコンポーネントを一度だけ取得
        karasu_SR = GetComponent<SpriteRenderer>();
        karasu_audio = GetComponent<AudioSource>();

        // Playerの情報を取得
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            PC = playerObj.GetComponent<PlayerController>();
            player_trans = playerObj.transform;
        }

        // 初期スケールを記憶
        initialScale_vec = transform.localScale;
    }

    // Spawnerによってアクティブにされるたびに呼ばれる
    void OnEnable()
    {
        // 初期位置を記憶（Spawnerによって再配置された位置）
        initialPosition_vec = transform.position;
        // 攻撃状態にリセット
        ResetAttack();
    }

    void Update()
    {
        // 移動が許可されている場合のみ処理
        if (move_b)
        {
            FlipTowardsPlayer();

            if (player_trans == null) return;

            switch (karasuNum_i)
            {
                case 0:
                    Attack_Type0();
                    break;
                // 他の攻撃タイプが必要な場合はここに追加
            }
        }
    }
    
    // 直進攻撃の処理
    void Attack_Type0()
    {
        // 攻撃方向を一度だけ決める
        if (!hasAttackDirection_b)
        {
            moveDirection_vec = (player_trans.position - transform.position).normalized;
            hasAttackDirection_b = true;
        }
        // その方向に直進
        transform.position += (Vector3)moveDirection_vec * speed_f * Time.deltaTime;
    }

    // プレイヤーの方向を向く処理
    void FlipTowardsPlayer()
    {
        if (player_trans == null) return;

        // 移動方向に基づいて向きを決める
        if (moveDirection_vec.x > 0) // 右へ移動中
        {
            transform.localScale = new Vector3(-Mathf.Abs(initialScale_vec.x), initialScale_vec.y, initialScale_vec.z);
        }
        else if (moveDirection_vec.x < 0) // 左へ移動中
        {
            transform.localScale = new Vector3(Mathf.Abs(initialScale_vec.x), initialScale_vec.y, initialScale_vec.z);
        }
    }
    
    // 衝突判定
    private void OnCollisionEnter2D(Collision2D other)
    {
        // 敵自身や他のカラス以外に当たったら消える
        if (other.gameObject.tag != "Enemy")
        {
            move_b = false;
            StartCoroutine(FadeOut());
        }
    }

    // 攻撃状態にリセットする処理
    public void ResetAttack()
    {
        // フラグを初期化
        move_b = true;
        hasAttackDirection_b = false;
        
        // 見た目と音をリセット
        if(karasu_SR != null) StartCoroutine(FadeIn());
        if(karasu_audio != null) karasu_audio.Play();
    }

    // フェードイン演出
    IEnumerator FadeIn()
    {
        Color c = karasu_SR.color;
        float timer = 0f;
        while(timer < 1f)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Clamp01(timer / 1f);
            karasu_SR.color = c;
            yield return null;
        }
    }   

    // フェードアウト演出
    IEnumerator FadeOut()
    {
        Color c = karasu_SR.color;
        float timer = 0f;
        while(timer < 1f)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Clamp01(1f - (timer / 1f));
            karasu_SR.color = c;
            yield return null;
        }
        if(karasu_audio != null) karasu_audio.Stop();
        gameObject.SetActive(false); // 自分自身を非表示にする
    }
}