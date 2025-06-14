using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuuchanController : MonoBehaviour
{
    [Header("ユウちゃん情報")]
    public Animator yuuchan_anim;
    public Rigidbody2D Yuuchan_rb;
    public float walkSpeed_f = 0.0015f;
    public float jumpPower_f = 50;
    public bool isJumping_b = false;
    [Header("プレイヤー情報")]
    public PlayerController PC;
    [Header("ゲームオーバー関連")]
    public GameObject yuuchanGameOverShadow_obj;
    public GameObject yuuchanRestartPos_obj;
    

    void Start()
    {
        yuuchan_anim = GetComponent<Animator>();
        yuuchan_anim.SetBool("hanten", true);
        Yuuchan_rb = GetComponent<Rigidbody2D>();
        PC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (PC.playerCanMove_b)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                this.transform.position += new Vector3(-walkSpeed_f, 0, 0);
                this.transform.localScale = new Vector3(-0.15f, 0.15f, 0.15f);
                yuuchan_anim.SetBool("walk", true);
                //isMoving = true;
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                this.transform.position += new Vector3(walkSpeed_f, 0, 0);
                this.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                yuuchan_anim.SetBool("walk", true);
                //isMoving = true;
            }
            else
            {
                yuuchan_anim.SetBool("walk", false);
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (!isJumping_b)
                {
                    isJumping_b = true;
                    Yuuchan_rb.AddForce(new Vector2(0, jumpPower_f));
                    float x = this.transform.position.x;
                    //playerShadow_obj.transform.position = new Vector3(x, -3.115063f, 0);
                }
            }
        }

        if (PC != null && PC.stageNum == 4)
        {
            // プレイヤーが動ける時だけ、Yuuchanも動ける
            if (PC.playerCanMove_b)
            {
                // ...（ここにあった移動処理は変更なし）...
            }
            else // プレイヤーが動けないときはYuuchanも歩きアニメーションを止める
            {
                yuuchan_anim.SetBool("walk", false);
            }
        }
        else if(PC == null || PC.stageNum != 4)
        {
            // ★追加: ステージ4以外では、安全のために歩きアニメーションを止めます。
            yuuchan_anim.SetBool("walk", false);
        }
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
            // ステージ4の場合のみ、PlayerControllerにゲームオーバーを通知する
            if (PC != null && PC.stageNum == 4)
            {
                PC.GameOver_b = true;
            }
        }
    }
}
