using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tools
{
    [MenuItem("Tools/Clear playerprefs")]
    public static void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
