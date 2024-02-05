using System.IO;
using UnityEditor;
using UnityEngine;

namespace IvyCreek.IvyCreekTools
{
    public static class ProjectSetup
    {
        [MenuItem("IvyCreek/Project Setup/Create Default Folders")]
        public static void CreateDefaultFolders()
        {
            Folders.CreateDefault("EmpireTycoonEngine",
                "Animation",
                "Art",
                "Audio",
                "Editor",
                "Fonts",
                "Input",
                "Materials",
                "Models",
                "Plugins",
                "Prefabs",
                "Resources",
                "Scenes",
                "Scripts",
                "Scripts/Core",
                "ScriptableObjects",
                "Settings",
                "Shaders",
                "StreamingAssets",
                "Textures"
            );
            
            UnityEditor.AssetDatabase.Refresh();
        }
        
        static class Folders
        {
            public static void CreateDefault(string root, params string[] folders)
            {
                var fullpath = Path.Combine(Application.dataPath, root);
                foreach (var folder in folders)
                {
                    var path = Path.Combine(fullpath, folder);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
            }
        }
    }
}