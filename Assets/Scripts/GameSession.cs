using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* A game session */
public class GameSession : ICloneable
{
    public Map map;

    public List<List<Player.Relations>> playerRelations;

    /* GAME INTERFACE VARIABLES */
    public List<Player> players;

    /* END GAME INTERFACE VARIABLES */

    public GameSession()
    {
        
    }

    public void Init()
    {
        InitMap();

        InitPlayers();
    }


    public virtual void InitMap()
    {

    }


    public virtual void InitPlayers()
    {
        
    }

    
    public object Clone()
    {
        return new GameSession()
        {
            /* TODO: implement */
        };
    }

    public GameSession CloneT()
    {
        return (GameSession)Clone();
    }
}