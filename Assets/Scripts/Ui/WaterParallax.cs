using System.Collections.Generic;
using UnityEngine;

public class WaterParallax : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float speed = 50f; // pixels por segundo
    [SerializeField] private RectTransform[] waterPieces;
    [SerializeField] private ScrollDirection direction = ScrollDirection.Left;

    private List<RectTransform> pieces;
    private float pieceWidth;

    private void Start()
    {
        if (waterPieces == null || waterPieces.Length == 0)
        {
            Debug.LogError("WaterParallax_UI: Nenhuma peça atribuída.");
            enabled = false;
            return;
        }

        pieces = new List<RectTransform>(waterPieces);
        pieceWidth = pieces[0].rect.width;

        // reposiciona as peças em fila
        float baseX = pieces[0].anchoredPosition.x;
        float y = pieces[0].anchoredPosition.y;
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].anchoredPosition = new Vector2(baseX + i * pieceWidth, y);
        }
    }

    private void Update()
    {
        float delta = speed * Time.deltaTime;

        Vector2 moveDir = (direction == ScrollDirection.Left ? Vector2.left : Vector2.right);
        foreach (var piece in pieces)
            piece.anchoredPosition += moveDir * delta;

        if (direction == ScrollDirection.Left)
            CheckLeftScroll();
        else
            CheckRightScroll();
    }

    private void CheckLeftScroll()
    {
        RectTransform leftMost = pieces[0];
        float minX = leftMost.anchoredPosition.x;
        int minIndex = 0;

        for (int i = 1; i < pieces.Count; i++)
        {
            if (pieces[i].anchoredPosition.x < minX)
            {
                minX = pieces[i].anchoredPosition.x;
                leftMost = pieces[i];
                minIndex = i;
            }
        }

        if (minX <= -pieceWidth)
        {
            float maxX = float.MinValue;
            foreach (var p in pieces)
                if (p.anchoredPosition.x > maxX) maxX = p.anchoredPosition.x;

            leftMost.anchoredPosition = new Vector2(maxX + pieceWidth, leftMost.anchoredPosition.y);
            pieces.RemoveAt(minIndex);
            pieces.Add(leftMost);
        }
    }

    private void CheckRightScroll()
    {
        RectTransform rightMost = pieces[0];
        float maxX = rightMost.anchoredPosition.x;
        int maxIndex = 0;

        for (int i = 1; i < pieces.Count; i++)
        {
            if (pieces[i].anchoredPosition.x > maxX)
            {
                maxX = pieces[i].anchoredPosition.x;
                rightMost = pieces[i];
                maxIndex = i;
            }
        }

        if (maxX >= pieceWidth)
        {
            float minX = float.MaxValue;
            foreach (var p in pieces)
                if (p.anchoredPosition.x < minX) minX = p.anchoredPosition.x;

            rightMost.anchoredPosition = new Vector2(minX - pieceWidth, rightMost.anchoredPosition.y);
            pieces.RemoveAt(maxIndex);
            pieces.Insert(0, rightMost);
        }
    }
}

public enum ScrollDirection { Left, Right }