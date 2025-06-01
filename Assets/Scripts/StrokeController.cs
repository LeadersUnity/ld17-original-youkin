using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrokeController : MonoBehaviour
{
    [Header("描画設定")]
    [SerializeField] Material lineMaterial;
    [SerializeField] Color lineColor = new Color(1f, 1f, 1f, 0.5f); // 薄めの色
    [Range(0.05f, 0.5f)]
    [SerializeField] float lineWidth = 0.1f;

    [Header("寿命設定")]
    [SerializeField] float lineLifetime = 5f; // 線が残る時間（秒）

    private GameObject lineObj;
    private LineRenderer lineRenderer;
    private List<Vector2> linePoints;
    [Header("その他のおぶえオブジェクト")]
    public PlayerController PC;

    void Start()
    {
        linePoints = new List<Vector2>();
        PC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (PC.playerCanMove_b)
        {
            if (PC.KakikomuCan_b)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _addLineObject();
                }

                if (Input.GetMouseButton(0))
                {
                    _addPositionDataToLineRenderer();
                }
            }
        }
    }

    // 線オブジェクトを作成
    void _addLineObject()
    {
        lineObj = new GameObject("Line");
        lineObj.tag = "floor";

        lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.sortingOrder = 10;

        lineObj.AddComponent<EdgeCollider2D>();
        lineObj.transform.SetParent(transform);

        linePoints = new List<Vector2>();

        // 一定時間後に線を削除
        StartCoroutine(_removeLineAfterSeconds(lineObj, lineRenderer, lineLifetime));
    }

    // 線の描画とコライダーの更新
    void _addPositionDataToLineRenderer()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 1f;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // 範囲制限
        if (worldPos.x < -6.85f || worldPos.x > 6.85f || worldPos.y < -4.3f || worldPos.y > 4.4f)
        {
            return;
        }

        if (lineRenderer == null) return;

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, worldPos);

        linePoints.Add(worldPos);
        lineObj.GetComponent<EdgeCollider2D>().SetPoints(linePoints);
    }

    // 線を一定時間後に削除 + フェードアウト演出
    IEnumerator _removeLineAfterSeconds(GameObject targetLine, LineRenderer renderer, float seconds)
    {
        float fadeDuration = 1f;
        float waitTime = seconds - fadeDuration;

        if (waitTime > 0)
            yield return new WaitForSeconds(waitTime);

        // フェードアウト処理
        float elapsed = 0f;
        Color startColor = renderer.startColor;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(1 - (elapsed / fadeDuration));
            Color faded = new Color(startColor.r, startColor.g, startColor.b, startColor.a * t);
            renderer.startColor = faded;
            renderer.endColor = faded;
            yield return null;
        }

        Destroy(targetLine);
    }
}