using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public FloatingText floatingTextObject;
    private static FloatingText _floatingTextObject;

    public static void CreateFloatingText(Vector3 position, string text, Color? color = null, Color? outlineColor = null)
    {
        var instance = Instantiate(_floatingTextObject);
        instance.transform.position = position + Vector3.up * 3.0f;
        instance.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
        instance.Setup(text, color, outlineColor);
    }

    private void Awake()
    {
        _floatingTextObject = floatingTextObject;
    }
}
