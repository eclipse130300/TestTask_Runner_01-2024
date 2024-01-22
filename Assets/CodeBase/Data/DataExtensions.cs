using UnityEngine;

public static class DataExtensions
{
    public static Vector3Data AsVector3Data(this Vector3 vector) => new Vector3Data(vector.x, vector.y, vector.z);

    public static Vector3 AsUnityVector(this Vector3Data vector3Data) => new Vector3(vector3Data.X, vector3Data.Y, vector3Data.Z);

    public static T ToDeserialized<T>(this string json) =>
        JsonUtility.FromJson<T>(json);

    public static Vector3 AddY(this Vector3 vector, float yOffset)
    {
        vector.y += yOffset;
        return vector;
    }

    public static bool VectorLengthIsGreaterThan(this Vector3 basePoint, float units)
    {
        return Vector3.Magnitude(basePoint) > units;
    }

    public static string ToJson<T>(this T objectToConvert) =>
        JsonUtility.ToJson(objectToConvert);
}