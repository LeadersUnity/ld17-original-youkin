using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Playerコンポーネント情報")]
    public Animator player_anim;
    [Header("Player移動情報")]
    public float walkSpeed_f = 2;
    
    
    void Start()
    {
        player_anim = this.GetComponent<Animator>();
    }

    
    void Update()
    {
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += new Vector3(walkSpeed_f, 0, 0);
            this.transform.localScale = new Vector3(0.15f,0.15f,0.15f);
            player_anim.SetBool("walk", true);
        }else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position += new Vector3(-walkSpeed_f, 0, 0);
            this.transform.localScale = new Vector3(-0.15f,0.15f,0.15f);
            player_anim.SetBool("walk", true);
        }else{
            player_anim.SetBool("walk", false);
        }
    }
}
