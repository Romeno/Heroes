using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitType : ICloneable
{
    public string name;
    public GameObject unitCardPrefab;
    public UnitFE gamePrefab;
    public UnitData data;

    // no need for deep copy as unitType should not change after initialization
    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public object CloneT()
    {
        return (UnitType)Clone();
    }
}
