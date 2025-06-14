using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarasuHantenController : MonoBehaviour
{
    public int karasuNum_i;
    [Header("カラス情報")]
    public float speed_f = 3f;
    private Transform player_trans;
    private Vector2 moveDirection_vec;
    private bool hasAttackDirection_b = false; // 攻撃方向を一度だけ計算したかどうかのフラグ
    private Vector3 initialScale_vec;
    // Start is called before the first frame update
    void Start()
    {
        player_trans = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //FlipTowardsPlayer();

        switch (karasuNum_i)
        {
            case 0:
                moveDirection_vec = (player_trans.position - transform.position).normalized;
                transform.position += (Vector3)moveDirection_vec * speed_f * Time.deltaTime;
                break;
                break;

            case 1:
                break;
        }
    }

    
}
