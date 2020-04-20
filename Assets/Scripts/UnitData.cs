using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : ICloneable
{
    /* both config & runtime*/
    public float cost;
    public float speed;
    public float attack;
    public float defence;
    public float minDamage;
    public float maxDamage;
    public float health;

    /* runtime */
    public Player owner;
    public float healthRemaining;
    public int quantity;

    public object Clone()
    {
        return this.MemberwiseClone();
    }


    public UnitData CloneT()
    {
        return (UnitData)Clone();
    }
}
