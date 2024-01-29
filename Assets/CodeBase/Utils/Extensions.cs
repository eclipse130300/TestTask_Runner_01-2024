using UnityEngine;

public static class Extensions
{

    public static T ToDeserialized<T>(this string json) =>
        JsonUtility.FromJson<T>(json);

    public static string ToJson<T>(this T objectToConvert) =>
        JsonUtility.ToJson(objectToConvert);

    public static float Remap(
        this float value,
        float start1,
        float stop1,
        float start2,
        float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }

    /// <summary>
    /// Remaps value -1/1f to 0,1,2 index 
    /// </summary>
    /// <returns></returns>
    public static int AsInteger02Index(this float value) => 
        Mathf.RoundToInt(value + 1);
}