using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : ICloneable
{
    public string name;

    public int attack;
    public int defence;
    public int spellpower;
    public int knowledge;
    public int curMana;
    public int experince;

    public List<Skill> skills;

    public Hero()
    {
        skills = new List<Skill>();
    }

    public object Clone()
    {
        return new Hero()
        {
            name = name,
            attack = attack,
            defence = defence,
            spellpower = spellpower,
            knowledge = knowledge,
            curMana = curMana,
            experince = experince,

            skills = skills.Clone()
        };
    }

    public Hero CloneT()
    {
        return (Hero)Clone();
    }
}
