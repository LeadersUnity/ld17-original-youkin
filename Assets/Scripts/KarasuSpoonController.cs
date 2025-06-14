/*
    KarasuSpoonControllerが満たす要件：
    ・カラスの出る順番を管理 → Spoon()コルーチンで実現
    ・PlayerControllerのGameOver_bがtrueになったら、最初からの出る順番になるように → Update()内のリセット処理で実現
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarasuSpoonController : MonoBehaviour
{
    [Header("出現させるカラスのプレハブ配列")]
    public GameObject[] karasuPrefabs;

    [Header("プレイヤー情報")]
    private PlayerController PC;

    // 内部管理用変数
    private Coroutine runningSpoonCoroutine;
    private bool isResetting = false;

    void Start()
    {
        // プレイヤーコントローラーの参照を取得
        PC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (PC == null)
        {
            Debug.LogError("PlayerControllerが見つかりません！");
            this.enabled = false;
            return;
        }

        // 開始時に出現シーケンスを開始
        runningSpoonCoroutine = StartCoroutine(SpoonSequence());
    }

    void Update()
    {
        // --- ゲームオーバー時のリセット処理 ---
        
        // プレイヤーがゲームオーバーになり、かつまだリセット処理を開始していない場合
        if (PC.GameOver_b && !isResetting)
        {
            // 「リセット処理中」のフラグを立て、処理の重複を防ぐ
            isResetting = true;
            Debug.Log("ゲームオーバーを検知。カラスの出現をリセットします。");

            // 実行中の出現コルーチンがあれば停止
            if (runningSpoonCoroutine != null)
            {
                StopCoroutine(runningSpoonCoroutine);
            }

            // 全てのカラスを非表示にする
            foreach (GameObject karasu in karasuPrefabs)
            {
                if (karasu != null) karasu.SetActive(false);
            }
            
            // 新しく出現シーケンスを開始し、その参照を保存
            runningSpoonCoroutine = StartCoroutine(SpoonSequence());
        }

        // ゲームオーバー状態でなくなったら、次のリセットに備えてフラグを戻す
        if (!PC.GameOver_b && isResetting)
        {
            isResetting = false;
        }
    }

    // カラスの出現タイミングと位置を管理するコルーチン
    IEnumerator SpoonSequence()
    {
        // 念のため、開始前に全てのカラスを非表示にする
        foreach (GameObject karasu in karasuPrefabs)
        {
            if (karasu != null) karasu.SetActive(false);
        }

        // --- ここから出現シーケンス ---

        yield return new WaitForSeconds(2.0f);
        
            // メモ：localPositionは親オブジェクトからの相対位置。ワールド座標ならpositionを使う。
            karasuPrefabs[0].transform.localPosition = new Vector3(0, 0, 0); 
            karasuPrefabs[0].SetActive(true);
        

        yield return new WaitForSeconds(5.0f);
        
            karasuPrefabs[1].transform.localPosition = new Vector3(-11.43f, 2.33f, 0); 
            karasuPrefabs[1].SetActive(true);
        

        yield return new WaitForSeconds(9.0f);
        
            karasuPrefabs[2].transform.localPosition = new Vector3(-8.65f, 1.83f, 0);
            karasuPrefabs[2].SetActive(true);
        
    }
}