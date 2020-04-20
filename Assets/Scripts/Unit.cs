using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class Unit : ICloneable
{
    public UnitType _type;

    public UnitData originalData;
    public UnitData curData;

    public GameObject go;

    public int quantity
    {
        get
        {
            return curData.quantity;
        }
        set
        {
            curData.quantity = value;
        }
    }

    public Unit(UnitType t, int quantity)
    {
        type = t;
        originalData.quantity = quantity;
        curData.quantity = quantity;
    }

    public UnitType type
    {
        get { return _type; }
        set
        {
            _type = value;
            curData = value.data.CloneT();
            //healthRemaining = value.data.health;
        }
    }


    /* Orders "walks" to the astarTarget position*/
    public void OrderWalk()
    {
        go.GetComponent<AIDestinationSetter>().target = G.battlefield.astarTarget.transform;
        go.GetComponent<Animator>().SetTrigger("walk");
    }

    public object Clone()
    {
        return new Unit(type, curData.quantity)
        {
            //quantity = quantity,
            //healthRemaining = healthRemaining,
            // do not deep clone GameObject
            go = go
        };
    }

    public object CloneT()
    {
        return (Unit)Clone();
    }
}
