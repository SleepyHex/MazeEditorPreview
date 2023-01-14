using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class GameObjectExtras
    {
        public static GameObject AssignNewGameObjAndGiveParent(string name, Transform parent)
        {
            var newObj = new GameObject();
            newObj.name = name;
            newObj.transform.parent = parent;
            return newObj;
        }
    }
}
