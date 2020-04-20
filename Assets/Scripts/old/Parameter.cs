using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameter<T>
{
    public T baseValue;

    public T value
    {
        get
        {
            T resValue = baseValue;

            foreach (var mod in modifiers.Values)
            {
                resValue = mod.Apply(resValue);
            }

            return resValue;
        }
    }

    SortedList<int, Modifier<T>> modifiers;
}
