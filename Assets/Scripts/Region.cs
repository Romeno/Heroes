using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Represents a region on the map */
public class Region : ICloneable
{
    public string name;

    public Region()
    {

    }

    public object Clone()
    {
        return new Region()
        {
            /* TODO: implement */
        };
    }

    public Region CloneT()
    {
        return (Region)Clone();
    }
}