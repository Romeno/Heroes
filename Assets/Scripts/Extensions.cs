using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class RomenoExtensions
{
    #region Bounds Extenstions 

    public static Vector3 GetTopLeft(this Bounds b)
    {
        return new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z);
    }

    public static Vector3 GetBottomRight(this Bounds b)
    {
        return new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z);
    }

    #endregion

    #region Array Extenstions 

    public static T[] Fill<T>(this T[] arr, T value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = value;
        }

        return arr;
    }

    #endregion

    #region GameObject Extenstions 

    public static void DestroyChildrenImmediate(this GameObject go
#if UNITY_EDITOR
        , bool undoable
#endif
        )
    {
        if (go != null)
        {
            while (go.transform.childCount > 0)
            {
#if UNITY_EDITOR
                if (undoable)
                {
                    Undo.DestroyObjectImmediate(go.transform.GetChild(0).gameObject);
                }
                else
                {
#endif
                    Object.DestroyImmediate(go.transform.GetChild(0).gameObject);
#if UNITY_EDITOR
                }
#endif
            }
        }
    }

    #endregion

    #region Image Extensions

    public static IEnumerator FadeIn(this Image img, float value, float fadeTime)
    {
        float startAlpha = img.color.a;

        while (img.color.a < value)
        {
            float newAlpha = img.color.a + (value - startAlpha) * Time.deltaTime / fadeTime;
            Debug.Log("FadeIn: newAlpha = " + newAlpha);
            img.color = new Color(img.color.r, img.color.g, img.color.b, newAlpha);

            yield return null;
        }

        img.color = new Color(img.color.r, img.color.g, img.color.b, value);
    }

    public static IEnumerator FadeOut(this Image img, float value, float fadeTime)
    {
        float startAlpha = img.color.a;

        while (img.color.a > value)
        {
            float newAlpha = img.color.a - (startAlpha - value) * Time.deltaTime / fadeTime;
            Debug.Log("FadeOut: newAlpha = " + newAlpha);
            img.color = new Color(img.color.r, img.color.g, img.color.b, newAlpha);

            yield return null;
        }

        img.color = new Color(img.color.r, img.color.g, img.color.b, value);


        //float startVolume = audioSource.volume;

        //while (audioSource.volume > 0)
        //{
        //    audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

        //    yield return null;
        //}

        //audioSource.Stop();
        //audioSource.volume = startVolume;
    }

    #endregion
}
