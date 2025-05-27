using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Playerコンポーネント情報")]
    public Animator player_anim;
    public Rigidbody2D player_rb;

    [Header("Player移動情報")]
    public float walkSpeed_f = 2;
    public float jumpPower_f = 50;
    public bool isJumping_b = false;

    [Header("その他の情報")]
    public GameObject playerShadow_obj;
    
    
    void Start()
    {
        player_anim = this.GetComponent<Animator>();
        player_rb = this.GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        //移動
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += new Vector3(walkSpeed_f, 0, 0);
            this.transform.localScale = new Vector3(0.15f,0.15f,0.15f);
            player_anim.SetBool("walk", true);
            //Shadow
            playerShadow_obj.transform.position += new Vector3(walkSpeed_f, 0, 0);
        }else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position += new Vector3(-walkSpeed_f, 0, 0);
            this.transform.localScale = new Vector3(-0.15f,0.15f,0.15f);
            player_anim.SetBool("walk", true);
            //Shadow
            playerShadow_obj.transform.position += new Vector3(-walkSpeed_f, 0, 0);
        }else{
            player_anim.SetBool("walk", false);
        }
        
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(!isJumping_b)
            {
                isJumping_b = true;
                player_rb.AddForce(new Vector2(0,jumpPower_f));
                //Shadow
                float x = this.transform.position.x;
                playerShadow_obj.transform.position = new Vector3(x,-3.115063f,0);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "floor")
        {
            if(isJumping_b)
            {
                isJumping_b = false;
            }
        }
    }
}
