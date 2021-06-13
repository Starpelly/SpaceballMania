/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using TMPro;

public class Grid
{
    public int width;
    public int height;
    private float cellSize;
    private Vector3 originPosition;
    private int[,] gridArray;

    private TextMesh[,] debugTextArray;
    private TextMesh[,] debugTextArray2;
    private SpriteRenderer[,] debugImageArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Transform parentObj, Sprite sprite)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];
        debugTextArray2 = new TextMesh[width, height];
        debugImageArray = new SpriteRenderer[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                if (y == 0)
                {
                    debugTextArray[x, y] = CreateWorldText(parentObj, null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 32, 0.16f, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                    debugTextArray2[x, y] = CreateWorldText(parentObj, null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .9f, 18, 0.16f, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                }
                debugImageArray[x, y] = CreateSprite(parentObj, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 0.5f, sprite);


                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 990f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 900f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 990f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 900f);

        //SetValueXY(4, 3, 69);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetValueXY(int x, int y, int value, Sprite sprite)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugImageArray[x, y].sprite = sprite;
            //debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, int value, Sprite sprite)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValueXY(x, y, value, sprite);
    }

    public void SetValueXYInt(int x, int y, int value, Sprite sprite, bool remove)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugImageArray[x, y].sprite = sprite;

            if (remove)
            {
                debugTextArray[x, y].text = null;
                debugTextArray2[x, y].text = null;
            }
            else
            {
                debugTextArray[x, y].text = gridArray[x, y].ToString();
                debugTextArray2[x, y].text = gridArray[x, y].ToString();
            }
        }
    }

    public void SetValueInt(Vector3 worldPosition, int value, Sprite sprite, bool remove)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        if (remove)
            SetValueXYInt(x, y, value, sprite, true);
        else
            SetValueXYInt(x, y, value, sprite, false);
    }

    public int GetValueX(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return x;
    }
    public int GetValueY(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return y;
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        } else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, float charSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.characterSize = charSize;
        textMesh.color = color;
        return textMesh;
    }

    public SpriteRenderer CreateSprite(Transform parent, Vector3 localPosition, float size, Sprite sprite)
    {
        GameObject gameObject = new GameObject("Sprite");
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        transform.localScale = new Vector2(size, size);
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        SpriteRenderer spriteOBJ = gameObject.GetComponent<SpriteRenderer>();
        return spriteOBJ;
    }
}
*/