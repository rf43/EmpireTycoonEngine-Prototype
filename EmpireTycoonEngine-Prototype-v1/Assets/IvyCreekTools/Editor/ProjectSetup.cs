using System.IO;
using UnityEditor;
using UnityEngine;

namespace IvyCreek.IvyCreekTools
{
    public static class ProjectSetup
    {
        [UnityEditor.MenuItem("IvyCreek/Project Setup/Create Default Folders")]
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
                "Scripts/ScriptableObjects",
                "Scripts/UI",
                "Settings",
                "Shaders",
                "StreamingAssets",
                "Textures"
            );
            
            AssetDatabase.Refresh();
        }

        private static class Folders {
            public static void CreateDefault(string root, params string[] folders) {
                var fullpath = Path.Combine(Application.dataPath, root);
                if (!Directory.Exists(fullpath)) {
                    Directory.CreateDirectory(fullpath);
                }
                foreach (var folder in folders) {
                    CreateSubFolders(fullpath, folder);
                }
            }
    
            private static void CreateSubFolders(string rootPath, string folderHierarchy) {
                var folders = folderHierarchy.Split('/');
                var currentPath = rootPath;
                foreach (var folder in folders) {
                    currentPath = Path.Combine(currentPath, folder);
                    if (!Directory.Exists(currentPath)) {
                        Directory.CreateDirectory(currentPath);
                    }
                }
            }
        }
    }
}