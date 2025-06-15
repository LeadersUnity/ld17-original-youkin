using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarasuController : MonoBehaviour
{
    public PlayerController PC;

    public float moveDistance_f = 3f;
    public float moveSpeed_f = 2f;
    public float diveSpeed_f = 5f;

    private Vector2 startPos_vec;
    private bool _bMovingRight = false;
    private bool _bDiving = false;
    private bool _bStopped = false;
    private bool _bRespawning = false;
    private bool _bHasRespawned = false;
    private Vector2 diveDirection_vec;

    private SpriteRenderer sr;
    private Animator anim; // Animatorを制御するための変数を追加

    [Header("サウンド")]
    public AudioSource Karasu_sound;

    void Start()
    {
        PC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        startPos_vec = transform.position;
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>(); // Animatorコンポーネントを取得
        sr.flipX = false;
        // 開始時にサウンドを再生
        Karasu_sound.Play();
    }

    void Update()
    {
        if (_bStopped || _bRespawning) return;

        if (PC.GameOver_b && !_bHasRespawned)
        {
            _bHasRespawned = true;
            StartCoroutine(RespawnRoutine());
            return;
        }

        if (!PC.GameOver_b && _bHasRespawned)
        {
            _bHasRespawned = false;
        }

        if (!_bDiving)
        {
            Patrol();
            if (PC.KarasuAttack_b)
            {
                StartDive();
            }
        }
        else
        {
            DiveTowardsPlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_bStopped || _bRespawning) return;

        if (other.gameObject.tag == "Player")
        {
            Karasu_sound.Stop();
            _bStopped = true;
            StartCoroutine(ResetAfterHitPlayer());
        }
        else{
            
            Karasu_sound.Stop();
            _bStopped = true;
            StartCoroutine(FadeOutOnly(sr)); 
        }
    }

    IEnumerator ResetAfterHitPlayer()
    {
        // アニメーションの競合を防ぐため、Animatorを一時的に無効化
        if (anim != null) anim.enabled = false;

        // 1. プレイヤーに当たったその場でフェードアウトする
        yield return StartCoroutine(FadeOut(sr));

        // 2. フェードアウト後（透明な間）に、状態をリセットして元の位置に戻る
        transform.position = startPos_vec;
        _bMovingRight = false;
        _bDiving = false;
        PC.KarasuAttack_b = false;
        sr.flipX = false;

        // 3. 元の位置でフェードインする
        yield return StartCoroutine(FadeIn(sr));
        
        // 処理が終わったのでAnimatorを有効に戻す
        if (anim != null) anim.enabled = true;

        // 4. サウンドを再生する
        Karasu_sound.Play();

        // 5. 全ての処理が完了してから、Update()の処理を再開させる
        _bStopped = false;
    }

    void Patrol()
    {
        float direction_f = _bMovingRight ? 1f : -1f;
        transform.Translate(Vector2.right * moveSpeed_f * Time.deltaTime * direction_f);

        if (transform.position.x > startPos_vec.x + moveDistance_f)
        {
            _bMovingRight = false;
            sr.flipX = false;
        }
        else if (transform.position.x < startPos_vec.x - moveDistance_f)
        {
            _bMovingRight = true;
            sr.flipX = true;
        }
    }

    void StartDive()
    {
        _bDiving = true;
        Vector2 targetPos_vec = PC.transform.position;
        diveDirection_vec = (targetPos_vec - (Vector2)transform.position).normalized;
        sr.flipX = (targetPos_vec.x < transform.position.x) ? false : true;
    }

    void DiveTowardsPlayer()
    {
        transform.Translate(diveDirection_vec * diveSpeed_f * Time.deltaTime);
    }

    IEnumerator FadeOut(SpriteRenderer SR)
    {
        float FinishTime_f = 1f;
        float NowTime_f = 0f;
        Color c = SR.color;

        while (NowTime_f < FinishTime_f)
        {
            NowTime_f += Time.deltaTime;
            c.a = Mathf.Clamp01(1 - (NowTime_f / FinishTime_f));
            SR.color = c;
            yield return null;
        }
    }

    IEnumerator FadeOutOnly(SpriteRenderer SR)
    {
        if (anim != null) anim.enabled = false;
        yield return StartCoroutine(FadeOut(SR));
        this.gameObject.SetActive(false);
    }

    IEnumerator FadeIn(SpriteRenderer SR)
    {
        float FinishTime_f = 1f;
        float NowTime_f = 0f;
        Color c = SR.color;
        c.a = 0f;
        SR.color = c;

        while (NowTime_f < FinishTime_f)
        {
            NowTime_f += Time.deltaTime;
            c.a = Mathf.Clamp01(NowTime_f / FinishTime_f);
            SR.color = c;
            yield return null;
        }
    }

    IEnumerator RespawnRoutine()
    {
        Karasu_sound.Stop();
        _bRespawning = true;

        // アニメーションの競合を防ぐため、Animatorを一時的に無効化
        if (anim != null) anim.enabled = false;

        // 1. その場でフェードアウトする
        yield return StartCoroutine(FadeOut(sr));

        // 2. 透明な間に元の位置に戻し、状態をリセット
        transform.position = startPos_vec;
        _bMovingRight = false;
        _bDiving = false;
        _bStopped = false;
        PC.KarasuAttack_b = false;
        sr.flipX = false;

        // 3. 元の位置でフェードインする
        yield return StartCoroutine(FadeIn(sr));

        // 処理が終わったのでAnimatorを有効に戻す
        if (anim != null) anim.enabled = true;

        // 4. サウンドを再生し、リスポーン完了
        Karasu_sound.Play();
        _bRespawning = false;
    }
}