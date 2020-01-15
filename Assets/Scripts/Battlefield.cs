using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Battlefield: MonoBehaviour
{
    public struct Resolution2
    {
        public int width { get; set; }
        public int height { get; set; }
        public int refreshRate { get; set; }
        public float aspectRatio;
    }

    public enum Alignment
    {
        BottomLeft,
        Center,
    }

    public SpriteRenderer background;

    public GameObject gridParent;
    public GameObject cellPfb;

    public float busyBackgroundRatio = 0.2f;

    public float padding = 0.5f;

    public int desiredHeight = 10;
    public int desiredWidth = 10;
    public Alignment alignment = Alignment.Center;

    [HideInInspector]
    public int effectiveHeight;
    [HideInInspector]
    public int effectiveWidth;
    [HideInInspector]
    public Vector3 center;

    public bool fillFreeSpace;
    public float desiredCellSize;

    public const float pixelsPerUnit = 100f;

    private Resolution2 res;

    void Start()
    {
        res = GetCurrentResolution();

        Camera.main.orthographicSize = (float)res.height / pixelsPerUnit / 2f;

        float scale = res.width / (float)background.sprite.textureRect.width;
        background.transform.localScale = new Vector3(scale, scale, 1.0f);

        float cameraTopLeftX = Camera.main.transform.position.x - Camera.main.orthographicSize / res.aspectRatio;
        float cameraTopLeftY = Camera.main.transform.position.y + Camera.main.orthographicSize;

        // background consists of "empty" part and "busy" art part 
        // which should note take more than busyBackgroundRatio part of the screen
        float offsetY = 0;
        if (res.height - background.sprite.textureRect.height * background.transform.localScale.y < 0)
        {
            offsetY = busyBackgroundRatio * (background.sprite.textureRect.height * background.transform.localScale.y - res.height) / pixelsPerUnit;
        }

        background.transform.position = new Vector3(
            cameraTopLeftX + background.sprite.bounds.extents.x * background.transform.localScale.x,
            cameraTopLeftY - background.sprite.bounds.extents.y * background.transform.localScale.y + offsetY, 
            0f);

        InvokeRepeating("PrintScreenRes", 0, 2);

        CreateBattlefield();

        //Debug.Log(background.sprite.bounds.center);
        //Debug.Log(background.sprite.bounds.size);

        //Screen.currentResolution.height * 0.2f;
        //Screen.currentResolution.width
    }

    void CreateBattlefield()
    {
        float cellSize = GetCellSize();

        effectiveHeight = desiredHeight;
        if (fillFreeSpace)
        {
            effectiveHeight = GetMaxCellsHeight(cellSize);
        }

        effectiveWidth = desiredWidth;
        if (fillFreeSpace)
        {
            effectiveWidth = GetMaxCellsWidth(cellSize);
        } else if (desiredWidth == 0)
        {
            // fill free space if battlefieldWidth is 0
            effectiveWidth = GetMaxCellsWidth(cellSize);
        }

        Vector2 alignmentOffset = GetAlignmentOffset(cellSize, effectiveWidth, effectiveHeight);

        center = GetBattlefieldCenter(cellSize, effectiveWidth, effectiveHeight);

        for (int i = 0; i < effectiveHeight; i++)
        {
            GameObject rowgo = new GameObject("Row");
            rowgo.transform.SetParent(transform);

            rowgo.transform.localPosition = new Vector3(
                cellSize / 2 + alignmentOffset.x + padding,
                cellSize / 2 + alignmentOffset.y + padding + i * cellSize,
                0f);

            for (int j = 0; j < effectiveWidth; j++)
            {
                GameObject cell = Instantiate(cellPfb, rowgo.transform);
                cell.transform.localPosition = new Vector3(j * cellSize, 0f, 0f);
                cell.transform.localScale = new Vector3(cellSize, cellSize, 0f);
            }
        }
    }

    Vector3 GetBattlefieldCenter(float cellSize, int widthInCells, int heightInCells)
    {
        float cameraBottomLeftX = Camera.main.transform.position.x - Camera.main.orthographicSize / res.aspectRatio;
        float cameraBottomLeftY = Camera.main.transform.position.y - Camera.main.orthographicSize;

        if (fillFreeSpace)
        {
            return new Vector3(
                cameraBottomLeftX + padding + widthInCells * cellSize / 2f,
                cameraBottomLeftY + padding + heightInCells * cellSize / 2f,
                0
            );
        }
        else
        {
            switch (alignment)
            {
                case Alignment.Center:
                    return Camera.main.transform.position;
                case Alignment.BottomLeft:
                    return new Vector3(
                        cameraBottomLeftX + padding + widthInCells * cellSize / 2f,
                        cameraBottomLeftY + padding + heightInCells * cellSize / 2f,
                        0
                    );
                default:
                    Debug.Log("Unknown alignment of the battlefield");
                    return Vector3.zero;
            }
        }
    }

    // offset from bottom left of the screen
    Vector2 GetAlignmentOffset(float cellSize, int widthInCells, int heightInCells)
    {
        if (fillFreeSpace)
        {
            return new Vector2(-Camera.main.orthographicSize / res.aspectRatio, -Camera.main.orthographicSize);
        }
        else
        {
            switch (alignment)
            {
                case Alignment.Center:
                    return new Vector2(-cellSize * widthInCells / 2f, -cellSize * heightInCells / 2f);
                case Alignment.BottomLeft:
                    return new Vector2(-Camera.main.orthographicSize / res.aspectRatio, -Camera.main.orthographicSize);
                default:
                    Debug.Log("Unknown alignment of the battlefield");
                    return Vector2.zero;
            }
        }
    }

    // Get maximum cell size to fit the screen given battlefield dimensions
    public float GetCellSize()
    {
        if (fillFreeSpace)
        {
            return desiredCellSize;
        }
        else
        {
            float cellSize1 = ((float)res.width - 2 * padding * pixelsPerUnit) / (float)desiredWidth / pixelsPerUnit;
            float cellSize2 = ((float)res.height * (1 - busyBackgroundRatio) - 2 * padding * pixelsPerUnit) / (float)desiredHeight / pixelsPerUnit;

            return Mathf.Min(cellSize1, cellSize2);
        }
    }

    // Get maximum cells in width
    int GetMaxCellsWidth(float cellSize)
    {
        return (int)(((float)res.width - 2 * padding * pixelsPerUnit) / (cellSize * pixelsPerUnit));
    }

    // Get maximum cells in height
    int GetMaxCellsHeight(float cellSize)
    {
        return (int)(((float)res.height * (1 - busyBackgroundRatio) - 2 * padding * pixelsPerUnit) / (cellSize * pixelsPerUnit));
    }

    Resolution2 GetCurrentResolution()
    {
#if UNITY_EDITOR
        return new Resolution2
        {
            width = Camera.main.pixelWidth,
            height = Camera.main.pixelHeight,
            refreshRate = Screen.currentResolution.refreshRate,
            aspectRatio = (float)Camera.main.pixelHeight / (float)Camera.main.pixelWidth
        };
#else
        return new Resolution2
        {
            width = Screen.currentResolution.width,
            height = Screen.currentResolution.height,
            refreshRate = Screen.currentResolution.refreshRate,
            aspectRatio = (float)Screen.currentResolution.height / (float)Screen.currentResolution.width
        };
#endif
    }

    void PrintScreenRes()
    {
        Debug.Log(Screen.currentResolution.width + " " + Screen.currentResolution.height);
        Debug.Log(Camera.main.pixelWidth + " " + Camera.main.pixelHeight);

        string[] res = UnityStats.screenRes.Split('x');
        Debug.Log(int.Parse(res[0]) + " " + int.Parse(res[1]));
    }
}
