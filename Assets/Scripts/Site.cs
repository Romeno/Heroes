using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* A site on the map, a map object*/
public class Site: ICloneable
{
    public bool canCaptrure;
    public Player owner;
    public GameObject go;

    public Site()
    {

    }

    public object Clone()
    {
        return new Site()
        {
            /* TODO: implement */
        };
    }

    public Site CloneT()
    {
        return (Site)Clone();
    }
}