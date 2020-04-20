using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public string statsName;
    public float priority;

    public delegate float Effect(float value);
    public Effect effect;
}
