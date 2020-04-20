using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill: ICloneable
{
    public List<SkillLevel> levels;

    public Skill()
    {
        levels = new List<SkillLevel>();
    }

    public object Clone()
    {
        return new Skill()
        {
            levels = levels.Clone()
        };
    }

    public Skill CloneT()
    {
        return (Skill)Clone();
    }
}


public class SkillLevel : ICloneable
{
    public string name;
    public string description;

    public object Clone()
    {
        return new SkillLevel()
        {
            name = name,
            description = description
        };
    }

    public SkillLevel CloneT()
    {
        return (SkillLevel)Clone();
    }
}
