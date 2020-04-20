using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : ICloneable
{
    public enum Controller
    {
        AI,
        Player,
        Max
    }

    public enum Faction
    {
        Human,
        Undead,
        Demon,
        Elemental,
        Max
    }

    public enum Relations
    {
        Enemy,
        Ally,
        Max
    }

    public string name;

    /* Resources */
    /* General */
    public int gold;
    public int lumber;
    public int stone;
    public int metal;
    public int golden_sand;
    public int magic_elixir;
    public int gems;
    public int crystal;

    /* Human */
    public int food;

    /* Elemental */
    public int frozen_flame;
    public int breathing_earth;

    /* Necromancers */
    public int life_force;
    public int bone;
    public int body;

    /* END Resources */

    public Controller controlled_by;
    public Color color;

    /* Player armies */
    public List<Army> armies;
    /* Owned sites */
    public List<Site> sites;

    public Player()
    {
        armies = new List<Army>();
        sites = new List<Site>();
    }

    public void IsAlly(Player other)
    {

    }

    public void IsEnemy(Player other)
    {

    }

    public object Clone()
    {
        return new Player()
        {
            /* TODO: implement */
        };
    }

    public Player CloneT()
    {
        return (Player)Clone();
    }
}