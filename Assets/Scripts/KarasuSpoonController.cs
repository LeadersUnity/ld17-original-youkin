using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarasuSpoonController : MonoBehaviour
{
    [Header("カラス情報")]
    public GameObject[] karasu_obj;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spoon());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Spoon()
    {
        yield return new WaitForSeconds(1.0f);
        karasu_obj[0].SetActive(true);
    }

    

}
