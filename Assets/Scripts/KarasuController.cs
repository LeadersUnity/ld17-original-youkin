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

    private Coroutine soundLoopCoroutine; // 4秒おきサウンド用コルーチン

    void Start()
    {
        PC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        startPos_vec = transform.position;
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        sr.flipX = false;
    }

    void OnEnable()
    {
        if (Karasu_sound != null)
        {
            Karasu_sound.Play();
        }
        soundLoopCoroutine = StartCoroutine(PlaySoundLoop());
    }

    void OnDisable()
    {
        if (soundLoopCoroutine != null)
        {
            StopCoroutine(soundLoopCoroutine);
            soundLoopCoroutine = null;
        }
    }

    IEnumerator PlaySoundLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);
            if (Karasu_sound != null)
            {
                Karasu_sound.Play();
            }
        }
    }

    void Update()
    {
        // ▼▼▼ 修正点 1: ゲームオーバー処理を最優先にする ▼▼▼
        // ゲームオーバー状態になったら、リスポーン処理を一度だけ呼び出す
        if (PC.GameOver_b && !_bHasRespawned)
        {
            _bHasRespawned = true;
            // 他のコルーチンが動いている可能性を考慮し、全て停止してからリスポーンを開始する
            StopAllCoroutines();
            StartCoroutine(RespawnRoutine());
            return; // リスポーン処理中は他の動作をさせない
        }

        // ゲームオーバー状態から復帰した際のフラグ管理
        if (!PC.GameOver_b && _bHasRespawned)
        {
            _bHasRespawned = false;
        }
        // ▲▲▲ 修正ここまで ▲▲▲

        if (_bStopped || _bRespawning) return;

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
            // ▼▼▼ 修正点 2: プレイヤーに当たったら停止するだけにする ▼▼▼
            // リセット処理はUpdate内のRespawnRoutineに任せる
            Karasu_sound.Stop();
            _bStopped = true;
            // ▲▲▲ 修正ここまで ▲▲▲
        }
        else
        {
            Karasu_sound.Stop();
            _bStopped = true;
            StartCoroutine(FadeOutOnly(sr));
        }
    }

    // ▼▼▼ 修正点 3: 不要になったためメソッドごと削除 ▼▼▼
    // IEnumerator ResetAfterHitPlayer() { ... }
    // ▲▲▲ 修正ここまで ▲▲▲

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

        if (anim != null) anim.enabled = false;

        // ① その場でだんだん消える
        yield return StartCoroutine(FadeOut(sr));

        // ② 消えた後に初期位置へ戻し、各種フラグをリセット
        transform.position = startPos_vec;
        _bMovingRight = false;
        _bDiving = false;
        _bStopped = false;
        PC.KarasuAttack_b = false;
        sr.flipX = false;

        // ③ 初期位置でだんだん見えてくる
        yield return StartCoroutine(FadeIn(sr));

        if (anim != null) anim.enabled = true;
        
        // ④ サウンドループを再開し、復帰完了
        soundLoopCoroutine = StartCoroutine(PlaySoundLoop());
        _bRespawning = false;
    }
}