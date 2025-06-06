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

    void Start()
    {
        PC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        startPos_vec = transform.position;
        sr = GetComponent<SpriteRenderer>();
        sr.flipX = false;
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
        if (other.gameObject.tag == "Player")
        {
            _bStopped = true;
            StartCoroutine(ResetAfterHitPlayer());
        }
        else
        {
            _bStopped = true;
            StartCoroutine(FadeOutOnly(sr)); // フェードアウトだけする
        }
    }

    IEnumerator ResetAfterHitPlayer()
    {
        yield return StartCoroutine(FadeOut(sr));

        // 元の状態に戻す
        transform.position = startPos_vec;
        _bMovingRight = false;
        _bDiving = false;
        _bStopped = false;
        PC.KarasuAttack_b = false;

        // 左向きに戻す
        sr.flipX = false;

        yield return StartCoroutine(FadeIn(sr));
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

    // SetActive(false) を削除したバージョン
    IEnumerator FadeOutOnly(SpriteRenderer SR)
    {
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
        _bRespawning = true;

        yield return StartCoroutine(FadeOut(sr));

        transform.position = startPos_vec;
        _bMovingRight = false;
        _bDiving = false;
        _bStopped = false;
        PC.KarasuAttack_b = false;

        // 左向きに初期化
        sr.flipX = false;

        yield return StartCoroutine(FadeIn(sr));

        _bRespawning = false;
    }
}
