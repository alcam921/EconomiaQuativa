using UnityEngine;
using UnityEngine.UI;

public class NeoGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns,
    }
    public FitType Type;
    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;
    public bool fitX;
    public bool fitY;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        if (Type == FitType.Uniform || Type == FitType.Width || Type == FitType.Height)
        {
            fitX = true;
            fitY = true;
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);

            if (Type == FitType.Width || Type == FitType.FixedColumns)
            {
                rows = Mathf.FloorToInt(transform.childCount / (float)columns);
            }
            if (Type == FitType.Height || Type == FitType.FixedRows)
            {
                columns = Mathf.FloorToInt(transform.childCount / (float)rows);
            }
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float)columns) - ((spacing.x / (float)columns) * (columns - 1)) - (padding.left / (float)columns) - (padding.right / (float) columns);
        float cellHeight = (parentHeight / (float)rows) - ((spacing.y / (float)rows) * (rows - 1)) - (padding.top / (float)rows) - (padding.bottom / (float)rows);
        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int collumnCount = 0;
        int rowCount = 0;
        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            collumnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * collumnCount) + (spacing.x * collumnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item,0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);

        }
    }
    public override void CalculateLayoutInputVertical()
    {

    }
    public override void SetLayoutHorizontal()
    {
        
    }
    public override void SetLayoutVertical()
    {

    }
}
