#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

namespace Assets.Editor.Tools
{
    public static class AssetLoader
    {
        public static T LoadByType<T>() where T : UnityEngine.Object
        {
            var path = GetAssetPath<T>();
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        public static string GetAssetPath<T>()
        {
            return AssetDatabase.FindAssets($"t:{typeof(T).Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .FirstOrDefault();
        }
    }
}
#endif