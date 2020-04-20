using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSlot : ICloneable
{
    public Player player;
    public bool onlyAI;

    public object Clone()
    {
        return new PlayerSlot()
        {
            /* TODO: implement */
        };
    }

    public PlayerSlot CloneT()
    {
        return (PlayerSlot)Clone();
    }
}

