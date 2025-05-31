using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Playerコンポーネント情報")]
    public Animator player_anim;
    public Rigidbody2D player_rb;
    public float player_x, player_y;




    [Header("Player移動情報")]
    public float walkSpeed_f = 2;
    public float jumpPower_f = 50;
    public bool isJumping_b = false;

    [Header("その他の情報")]
    public Stage1Controller SOC;
    public GameObject playerShadow_obj;


    void Start()
    {
        player_anim = this.GetComponent<Animator>();
        player_rb = this.GetComponent<Rigidbody2D>();
        SOC = GameObject.FindWithTag("Stage1Controller").GetComponent<Stage1Controller>();
    }


    void Update()
    {
        //移動
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += new Vector3(walkSpeed_f, 0, 0);
            this.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            player_anim.SetBool("walk", true);
            //Shadow
            //playerShadow_obj.transform.position += new Vector3(walkSpeed_f, 0, 0);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position += new Vector3(-walkSpeed_f, 0, 0);
            this.transform.localScale = new Vector3(-0.15f, 0.15f, 0.15f);
            player_anim.SetBool("walk", true);
            //Shadow
            //playerShadow_obj.transform.position += new Vector3(-walkSpeed_f, 0, 0);
        }
        else
        {
            player_anim.SetBool("walk", false);
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isJumping_b)
            {
                isJumping_b = true;
                player_rb.AddForce(new Vector2(0, jumpPower_f));
                //Shadow
                float x = this.transform.position.x;
                playerShadow_obj.transform.position = new Vector3(x, -3.115063f, 0);
            }
        }

        //かげ
        Shadow(0);
        
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
                Debug.Log("floorに当たった: " + hit.collider.name);
            }
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
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Stage1_Area")
        {
            Debug.Log("stage1Area");
            SOC.stage1Num_i++;
            Destroy(other.gameObject);
        }
    }
}
