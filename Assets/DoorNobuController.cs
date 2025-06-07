using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DoorNobuController : MonoBehaviour
{
    public Collider2D doorNobuCollider;
    public GameObject OpenDoor_obj;
    public Stage1Controller SOC;
    void Start()
    {
        doorNobuCollider = GetComponent<Collider2D>();
        SOC = GameObject.FindWithTag("Stage1Controller").GetComponent<Stage1Controller>();
    }

    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            Debug.Log("ドアノブに線が接触しました！");
            OpenDoorMethod();
        }
    }

    public void OpenDoorMethod()
    {
        StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(4f);
        SpriteRenderer closeDoor_SR = GetComponent<SpriteRenderer>();
        closeDoor_SR.enabled = false;
        OpenDoor_obj.SetActive(true);
        SpriteRenderer openDoor_SR = OpenDoor_obj.GetComponent<SpriteRenderer>();
        yield return StartCoroutine(FadeIn(openDoor_SR));
        SOC.classroomCollider_obj.SetActive(false);

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
    }
    
}

