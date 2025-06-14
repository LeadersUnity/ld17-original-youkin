using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarasuHantenController : MonoBehaviour
{
    public int karasuNum_i;
    [Header("カラス情報")]
    public SpriteRenderer karasu_SR;
    public float speed_f = 3f;
    private Transform player_trans;
    private Vector2 moveDirection_vec;
    private bool hasAttackDirection_b = false; // 攻撃方向を一度だけ計算したかどうかのフラグ
    private Vector3 initialScale_vec;
    public bool move_b;
    // Start is called before the first frame update
    void Start()
    {
        player_trans = GameObject.FindWithTag("Player").GetComponent<Transform>();
        karasu_SR = GetComponent<SpriteRenderer>();
        move_b = true;
        StartCoroutine(FadeIn(karasu_SR));
    }

    // Update is called once per frame
    void Update()
    {
        FlipTowardsPlayer();

        if (move_b)
        {
            switch (karasuNum_i)
            {
                case 0:
                    StartCoroutine(Case0());
                    break;      
                case 1:

                    break;
            }
        }
    }

    IEnumerator Case0()
    {
        yield return new WaitForSeconds(1f);
        // まだ攻撃方向を決定していない場合
        if (!hasAttackDirection_b)
        {

            // プレイヤーの現在位置への方角を計算し、変数に保存する
            moveDirection_vec = (player_trans.position - transform.position).normalized;

            // 「方向を決定済み」のフラグを立てる
            // これにより、このif文の中は一度しか実行されなくなる
            hasAttackDirection_b = true;
        }
                    
                    // 一度決めた方角に向かって、毎フレーム直進する
                    transform.position += (Vector3)moveDirection_vec * speed_f * Time.deltaTime;
    }

    void FlipTowardsPlayer()
    {
        // プレイヤーの参照がなければ何もしない
        if (player_trans == null) return;

        // プレイヤーがカラスの右側にいる場合
        if (player_trans.position.x > this.transform.position.x)
        {
            // 右を向かせる（元の画像が左向きなので、Xスケールをマイナスにする）
            this.transform.localScale = new Vector3(-0.1f,0.1f, 0.1f);
        }
        // プレイヤーがカラスの左側にいる場合
        else
        {
            // 左を向かせる（元の向きなので、Xスケールをプラスにする）
            this.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        move_b = false;
        StartCoroutine(FadeOut(karasu_SR));
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
        this.gameObject.SetActive(false);   
    }

    
}
