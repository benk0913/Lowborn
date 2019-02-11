using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class CustomAssetLoader
{
    public static List<GameObject> LoadAllPrefabs(string path)
    {
        if (path != "")
        {
            if (path.EndsWith("/"))
            {
                path = path.TrimEnd('/');
            }
        }

        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] fileInf = dirInfo.GetFiles("*.prefab");

        //loop through directory loading the game object and checking if it has the component you want
        List<GameObject> Prefabs = new List<GameObject>();
        foreach (FileInfo fileInfo in fileInf)
        {
            string fullPath = fileInfo.FullName.Replace(@"\", "/");
            string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");
            Prefabs.Add(AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject);

        }

        foreach(DirectoryInfo subDirectory in dirInfo.GetDirectories())
        {
            Prefabs.AddRange(LoadAllPrefabs(subDirectory.FullName));
        }

        return Prefabs;
    }

    public static List<Prop> LoadAllProps(string path)
    {
        if (path != "")
        {
            if (path.EndsWith("/"))
            {
                path = path.TrimEnd('/');
            }
        }

        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] fileInf = dirInfo.GetFiles("*.asset");

        //loop through directory loading the game object and checking if it has the component you want
        List<Prop> Props = new List<Prop>();
        foreach (FileInfo fileInfo in fileInf)
        {
            string fullPath = fileInfo.FullName.Replace(@"\", "/");
            string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");
            Props.Add(AssetDatabase.LoadAssetAtPath(assetPath, typeof(Prop)) as Prop);

        }

        foreach (DirectoryInfo subDirectory in dirInfo.GetDirectories())
        {
            Props.AddRange(LoadAllProps(subDirectory.FullName));
        }

        return Props;
    }

    public static Sprite LoadIcon(string path, string iconName)
    {
        if (path != "")
        {
            if (path.EndsWith("/"))
            {
                path = path.TrimEnd('/');
            }
        }

        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] fileInf = dirInfo.GetFiles("*.png");

        Sprite targetSprite = null;
        foreach (FileInfo fileInfo in fileInf)
        {
            string fullPath = fileInfo.FullName.Replace(@"\", "/");
            string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");
            targetSprite = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite)) as Sprite;

            if(targetSprite != null && targetSprite.name == iconName)
            {
                return targetSprite;
            }
        }

        Sprite subfolderSprite = null;
        foreach (DirectoryInfo subDirectory in dirInfo.GetDirectories())
        {
            subfolderSprite = LoadIcon(subDirectory.FullName, iconName);
        }

        if(subfolderSprite != null)
        {
            return subfolderSprite;
        }

        return targetSprite;
    }

}