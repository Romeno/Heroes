using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Army : ICloneable
{
    public Army(Hero hero)
    {
        this.hero = hero;
        units = new List<Unit>();
    }

    public object Clone()
    {
        return new Army(hero)
        {
            // we do not need deep copy
            units = units.Clone()
        };
    }

    public Army CloneT()
    {
        return (Army)Clone();
    }

    public Hero hero;
    public List<Unit> units;
}
