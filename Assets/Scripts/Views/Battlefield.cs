using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Pathfinding;

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
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft,
        Center,
    }

    public SpriteRenderer background;

    public GameObject gridParent;
    public BattlefieldCell cellPfb;

    public GameObject astarTarget;

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
    [HideInInspector]
    public Rect battlefieldRect;

    public bool fillFreeSpace;
    public float desiredCellSize;

    public const float pixelsPerUnit = 100f;

    private Resolution2 res;

    private int round = 1;

    private int attackingPlayer = 0;

    private Heroes game;

    private Unit currentTurnUnit;

    private bool playerHasControl;

    private BattlefieldCell[,] cells;

    [Tooltip("UI view for battle phase of the game.")]
    public GameObject uiView;

    BattlefieldCell GetCell(int x, int y)
    {
        return cells[y, x];
    }

    public void Activate(bool value)
    {
        gameObject.SetActive(value);
        uiView.gameObject.SetActive(value);
    }

    void Start()
    {
        game = Camera.main.GetComponent<Heroes>();

        res = GetCurrentResolution();

        Camera.main.orthographicSize = (float)res.height / pixelsPerUnit / 2f;

        float scale = res.width / (float)background.sprite.textureRect.width;
        background.transform.localScale = new Vector3(scale, scale, 1.0f);

        float cameraTopLeftX = Camera.main.transform.position.x - Camera.main.orthographicSize / res.aspectRatio;
        float cameraTopLeftY = Camera.main.transform.position.y + Camera.main.orthographicSize;

        // background consists of "empty" part, which is used for battlefield. And "busy" part which is used for art
        // The "busy" part should note take more than busyBackgroundRatio part of the screen
        float offsetY = 0;
        if (res.height - background.sprite.textureRect.height * background.transform.localScale.y < 0)
        {
            offsetY = busyBackgroundRatio * (background.sprite.textureRect.height * background.transform.localScale.y - res.height) / pixelsPerUnit;
        }

        background.transform.position = new Vector3(
            cameraTopLeftX + background.sprite.bounds.extents.x * background.transform.localScale.x,
            cameraTopLeftY - background.sprite.bounds.extents.y * background.transform.localScale.y + offsetY,
            0f);

        CreateBattlefield();
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
        }
        else if (desiredWidth == 0)
        {
            // fill free space if battlefieldWidth is 0
            effectiveWidth = GetMaxCellsWidth(cellSize);
        }

        Vector2 alignmentOffset = GetAlignmentOffset(cellSize, effectiveWidth, effectiveHeight);

        center = GetBattlefieldCenter(cellSize, effectiveWidth, effectiveHeight);

        battlefieldRect = new Rect(center.x - effectiveWidth / 2.0f, center.y + effectiveHeight / 2.0f, effectiveWidth, effectiveHeight);

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
                BattlefieldCell cell = Instantiate(cellPfb, rowgo.transform);
                cell.transform.localPosition = new Vector3(j * cellSize, 0f, 0f);
                cell.transform.localScale = new Vector3(cellSize, cellSize, 0f);
                cell.x = j;
                cell.y = effectiveHeight - i - 1;
                cells[i, j] = cell;
            }
        }
    }


    Vector3 GetBattlefieldCenter(float cellSize, int widthInCells, int heightInCells)
    {
        float cameraBottomLeftX = game.GetComponent<Camera>().transform.position.x - Camera.main.orthographicSize / res.aspectRatio;
        float cameraBottomLeftY = game.GetComponent<Camera>().transform.position.y - Camera.main.orthographicSize;

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
                    return game.GetComponent<Camera>().transform.position;
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
            return new Vector2(-game.GetComponent<Camera>().orthographicSize / res.aspectRatio, -game.GetComponent<Camera>().orthographicSize);
        }
        else
        {
            switch (alignment)
            {
                case Alignment.Center:
                    return new Vector2(-cellSize * widthInCells / 2f, -cellSize * heightInCells / 2f);
                case Alignment.BottomLeft:
                    return new Vector2(-game.GetComponent<Camera>().orthographicSize / res.aspectRatio, -game.GetComponent<Camera>().orthographicSize);
                default:
                    Debug.Log("Unknown alignment of the battlefield");
                    return Vector2.zero;
            }
        }
    }


    public void InitBattle()
    {
        round = 1;

        attackingPlayer = 0;

        CreateObstacles();

        StartCoroutine(GenerateGraph());

        CreatePlayerArmy();

        CreateOpponentArmy();

        currentTurnUnit = FindNextTurnUnit();

        HighlightMovementArea();
    }


    void CreateObstacles()
    {
        Vector3 bottomLeft = new Vector3(
            center.x - (float)effectiveWidth / 2f * GetCellSize(),
            center.y - (float)effectiveHeight / 2f * GetCellSize(),
            0
        );

        InstantiateAt(game.smallObstacle, bottomLeft, 3, 0);
        InstantiateAt(game.smallObstacle, bottomLeft, 3, 1);
        InstantiateAt(game.smallObstacle, bottomLeft, 3, 2);
        InstantiateAt(game.smallObstacle, bottomLeft, 3, 3);
        InstantiateAt(game.smallObstacle, bottomLeft, 3, 4);
        InstantiateAt(game.smallObstacle, bottomLeft, 3, 5);

        InstantiateAt(game.largeObstacle, bottomLeft, 6, 2);
        InstantiateAt(game.largeObstacle, bottomLeft, 8, 2);
        InstantiateAt(game.largeObstacle, bottomLeft, 10, 2);
        InstantiateAt(game.largeObstacle, bottomLeft, 12, 2);
        InstantiateAt(game.largeObstacle, bottomLeft, 12, 4);
    }


    void InstantiateAt(GameObject what, Vector3 bottomLeft, int x, int y)
    {
        Vector3 extents = what.GetComponent<SpriteRenderer>().sprite.bounds.extents;
        Vector3 pos = new Vector3(
            bottomLeft.x + x * GetCellSize() + extents.x * what.transform.localScale.x,
            bottomLeft.y + y * GetCellSize() + extents.y * what.transform.localScale.y,
            0);

        GameObject obs = Instantiate(what, transform);
        obs.transform.localPosition = pos;
    }


    IEnumerator GenerateGraph()
    {
        yield return new WaitForSeconds(0);

        GridGraph gg = (GridGraph)AstarPath.active.graphs[0];

        gg.SetDimensions(effectiveWidth, effectiveHeight, GetCellSize());

        gg.center = center;

        AstarPath.active.Scan();
    }


    void CreatePlayerArmy()
    {
        float cellSize = GetCellSize();

        int i = 0;
        foreach (Unit u in G.session.playerArmy.units)
        {
            UnitFE goUnit = Instantiate(u.type.gamePrefab, gridParent.transform);
            goUnit.unit = u;
            u.go = goUnit.gameObject;
            goUnit.transform.position = new Vector3(battlefieldRect.xMin + cellSize / 2.0f, battlefieldRect.yMin - cellSize / 2.0f, 0.0f);
            goUnit.transform.Translate(new Vector3(0, -cellSize * i * 2, 0));
            i += 1;
        }
    }


    void CreateOpponentArmy()
    {
        float cellSize = GetCellSize();

        int i = 0;
        foreach (Unit u in G.session.opponentArmy.units)
        {
            UnitFE goUnit = Instantiate(u.type.gamePrefab, gridParent.transform);
            goUnit.unit = u;
            u.go = goUnit.gameObject;
            goUnit.transform.position = new Vector3(battlefieldRect.xMin + cellSize / 2.0f, battlefieldRect.yMin - cellSize / 2.0f, 0.0f);
            goUnit.transform.Translate(new Vector3(cellSize * (effectiveWidth - 1), -cellSize * i * 2, 0));
            i += 1;
        }
    }


    private int overallArmyCount = 0;
    Unit FindNextTurnUnit()
    {
        overallArmyCount++;
        if (overallArmyCount >= G.session.playerArmy.units.Count + G.session.opponentArmy.units.Count)
        {
            overallArmyCount = 0;
            return G.session.playerArmy.units[overallArmyCount];
        }
        else if (overallArmyCount >= G.session.playerArmy.units.Count)
        {
            return G.session.opponentArmy.units[overallArmyCount - G.session.playerArmy.units.Count];
        }
        else
        {
            return G.session.playerArmy.units[overallArmyCount];
        }
    }


    void HighlightMovementArea()
    {

    }


    // Get maximum cell size that will fit the screen given battlefield dimensions
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


    // Get maximum number of cells that fit horizontally
    int GetMaxCellsWidth(float cellSize)
    {
        return (int)(((float)res.width - 2 * padding * pixelsPerUnit) / (cellSize * pixelsPerUnit));
    }


    // Get maximum number of cells that fit vertically 
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
#if UNITY_EDITOR
        Debug.Log(Screen.currentResolution.width + " " + Screen.currentResolution.height);
        Debug.Log(Camera.main.pixelWidth + " " + Camera.main.pixelHeight);

        string[] res = UnityStats.screenRes.Split('x');
        Debug.Log(int.Parse(res[0]) + " " + int.Parse(res[1]));
#endif
    }

    public void CellClicked(BattlefieldCell cell)
    {
        if (cell.unit.curData.owner != currentTurnUnit.curData.owner)
        {

        }

        if (cell.canMoveHereNow)
        {
            float cellSize = GetCellSize();

            astarTarget.transform.position = new Vector3(battlefieldRect.xMin + cellSize / 2.0f + cell.x * cellSize, battlefieldRect.yMin - cellSize / 2.0f - cellSize * cell.y);

            playerHasControl = false;
            currentTurnUnit.OrderWalk();
        }
    }

    public void UnitFinishedMoving()
    {
        
    }

    void Update()
    {

    }
}
