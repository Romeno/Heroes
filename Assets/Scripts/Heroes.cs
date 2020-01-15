using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Heroes : MonoBehaviour
{
    public Battlefield bf;

    public GameObject smallObstacle;
    public GameObject largeObstacle;

    void Start()
    {
        CreateObstacles();

        StartCoroutine(GenerateGraph());
        
    }

    void CreateObstacles()
    {
        Vector3 bottomLeft = new Vector3(
            bf.center.x - (float)bf.effectiveWidth / 2f * bf.GetCellSize(),
            bf.center.y - (float)bf.effectiveHeight / 2f * bf.GetCellSize(), 
            0
        );

        InstantiateAt(smallObstacle, bottomLeft, 3, 0);
        InstantiateAt(smallObstacle, bottomLeft, 3, 1);
        InstantiateAt(smallObstacle, bottomLeft, 3, 2);
        InstantiateAt(smallObstacle, bottomLeft, 3, 3);
        InstantiateAt(smallObstacle, bottomLeft, 3, 4);
        InstantiateAt(smallObstacle, bottomLeft, 3, 5);

        InstantiateAt(largeObstacle, bottomLeft, 6, 2);
        InstantiateAt(largeObstacle, bottomLeft, 8, 2);
        InstantiateAt(largeObstacle, bottomLeft, 10, 2);
        InstantiateAt(largeObstacle, bottomLeft, 12, 2);
        InstantiateAt(largeObstacle, bottomLeft, 12, 4);
    }

    void InstantiateAt(GameObject what, Vector3 bottomLeft, int x, int y)
    {
        Vector3 extents = what.GetComponent<SpriteRenderer>().sprite.bounds.extents;
        Vector3 pos = new Vector3(
            bottomLeft.x + x * bf.GetCellSize() + extents.x * what.transform.localScale.x,
            bottomLeft.y + y * bf.GetCellSize() + extents.y * what.transform.localScale.y,
            0);

        GameObject obs = Instantiate(what, bf.transform);
        obs.transform.localPosition = pos;
    }

    IEnumerator GenerateGraph()
    {
        yield return new WaitForSeconds(0);

        GridGraph gg = (GridGraph)AstarPath.active.graphs[0];

        gg.SetDimensions(bf.effectiveWidth, bf.effectiveHeight, bf.GetCellSize());

        gg.center = bf.center;

        AstarPath.active.Scan();
    }

    void Update()
    {
        
    }
}
