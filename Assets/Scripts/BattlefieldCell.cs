using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BattlefieldCell : MonoBehaviour
{
    public int x;
    public int y;

    public GraphNode node;
    public bool canMoveHereNow;
    public Unit unit;

    void OnMouseDown()
    {
        Camera.main.GetComponent<Heroes>().battlefield.CellClicked(this);
    }
}
