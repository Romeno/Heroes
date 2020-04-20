using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* A site on the map, a map object*/
public class Map : ICloneable
{
    public string name;
    public List<Region> regions;
    public List<PlayerSlot> playerSlots;

    public Map()
    {
        regions = new List<Region>();
        playerSlots = new List<PlayerSlot>();
    }

    public object Clone()
    {
        return new Map()
        {
            /* TODO: implement */
        };
    }

    public Map CloneT()
    {
        return (Map)Clone();
    }
}