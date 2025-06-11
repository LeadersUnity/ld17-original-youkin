using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageFourController : MonoBehaviour
{
    [Header("stage1管理")]
    public int stage4Num_i = 0;

    [Header("UI関連")]
    public GameObject Date_obj;
    public TextMeshProUGUI Date_txt;
    public string[] Date_string;
    public GameObject NikkiContent_obj;
    public TextMeshProUGUI NikkiContent_txt;
    public string[] NikkiContent_string;

    [Header("サウンド")]
    public AudioSource noise_sound;
    public AudioSource writing_sound;
    [Header("ステージ4オブジェクト")]
    public GameObject room_stage4_obj;
    public GameObject roomShadow_obj;
    [Header("プレイヤー情報")]
    public GameObject Player_obj;
    public GameObject PlayerShadow_obj;
    public PlayerController PC;
    // Start is called before the first frame update
    void Start()
    {
        Player_obj = GameObject.FindWithTag("Player");
        PC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        noise_sound.Play();
        StartCoroutine(StartScene());
    }

    IEnumerator StartScene()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
