using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ListUtil
{
    public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
    {
        return listToClone.ConvertAll<T>(new Converter<T, T>(x => (T)x.Clone()));
    }
}
