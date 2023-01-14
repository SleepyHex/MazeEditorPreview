using UnityEngine;

public static class TransformExtensions
{
    public static void DestroyAllChildren(this Transform t, bool useDestroyImmediate)
    {
        for(int i = t.childCount - 1; i >= 0; i--)
        {
            if(useDestroyImmediate)
                GameObject.DestroyImmediate(t.GetChild(i).gameObject);
            else
                GameObject.Destroy(t.GetChild(i).gameObject);
        }
    }
}
