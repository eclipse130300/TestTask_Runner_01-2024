using UnityEngine;

public static class CollisionDetectionUtils
{
    public static bool InsideLayerMask(this GameObject gameObject, LayerMask layerMask)
    {
        int collidingObjLayer = 1 << gameObject.layer;

        if ((collidingObjLayer & layerMask) != 0)
        {
            return true;
        }

        return false;
    }
}