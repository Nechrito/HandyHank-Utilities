using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Assets.Scripts.Utilities
{
    public static class ResourceUtility
    {
        private static readonly Dictionary<string, Object[]> cache = new Dictionary<string, Object[]>();

        public static T[] GetAtPath<T>(string path, out bool cached)
        {
#if UNITY_EDITOR
            cached = false;

            if (typeof(T) != typeof(Sprite)) 
                return GetAtPathAssetDatabase<T>($"{path}");

            var textures = GetAtPathAssetDatabase<Texture2D>($"Resources/{path}");
            var sprites  = new List<Sprite>();
            foreach (var texture in textures)
            {
                string spriteSheet= AssetDatabase.GetAssetPath(texture);
                var singleSprites= AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToArray();
                sprites.AddRange(singleSprites);
            }
            return sprites.ToArray() as T[];
#else
            return GetAtPathResources<T>(path, out cached);
#endif
        }

        public static T[] GetAtPathResources<T>(string path, out bool cached)
        {
            Object[] obj;

            if (cache.ContainsKey(path))
            {
                //HankDebug.Log($"Using cache for {path}");
                obj    = cache[path];
                cached = true;
            }
            else
            {
                obj = Resources.LoadAll(path, typeof(T));
                cache.Add(path, obj);
                cached = false;
            }

            var al = new ArrayList();
            al.AddRange(obj);

            var result = new T[al.Count];
            for (int i = 0; i < al.Count; i++)
                result[i] = (T)al[i];

            return result;
        }

        public static T[] GetAtPathAssetDatabase<T>(string path)
        {
#if UNITY_EDITOR
            var al = new ArrayList();

            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);

            foreach (string fileName in fileEntries)
            {
                //HankDebug.Log($"Loaded {fileName}");
                int assetPathIndex = fileName.IndexOf("Assets", StringComparison.Ordinal);

                string localPath = fileName.Substring(assetPathIndex);

                var t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

                if (t != null)
                {
                    al.Add(t);
                }
            }

            var result = new T[al.Count];
            for (int i = 0; i < al.Count; i++)
                result[i] = (T)al[i];

            return result;
#else
            return new ArrayList() as T[];
#endif
        }


        public static void DumpCache()
        {
            cache.Clear();
        }
    }
}
