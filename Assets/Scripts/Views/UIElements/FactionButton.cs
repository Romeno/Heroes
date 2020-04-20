using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* A site on the map, a map object*/
public class FactionButton : MonoBehaviour
{
    public void OnClick()
    {
        G.game.StartNewSession();
    }
}