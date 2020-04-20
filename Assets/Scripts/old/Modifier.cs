using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier<T>
{
    public int priority;

    public delegate T ApplyDelegate(T value);
    public ApplyDelegate Apply;

}
