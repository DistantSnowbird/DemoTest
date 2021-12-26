using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class BuildTool : Editor {

    [MenuItem("Tools/Build Windows Bundle")]
    static void WindowsBundleBuild()
    {
        Build(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Tools/Build IOS Bundle")]
    static void IOSBundleBuild()
    {
        Build(BuildTarget.iOS);
    }
    [MenuItem("Tools/Build Android Bundle")]
    static void AndroidBundleBuild()
    {
        Build(BuildTarget.Android);
    }
    static void Build(BuildTarget target)
    {
        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();

        List<string> bundleInfos = new List<string>();

        string[] files = Directory.GetFiles(PathTool.BuildBundlePath, "*", SearchOption.AllDirectories);
        for(int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".meta"))
            {
                continue;
            }
            
            AssetBundleBuild assetBundle = new AssetBundleBuild();

            string fileName = PathTool.GetStandardPath(files[i]);

            string assetName = PathTool.GetUnityPath(fileName);
            Debug.Log("file:" + fileName);
            assetBundle.assetNames = new string[] { assetName};
            string bundleName = fileName.Replace(PathTool.BuildBundlePath, "").ToLower();
            assetBundle.assetBundleName = bundleName + ".ab";
            assetBundleBuilds.Add(assetBundle);

            List<string> dependenceInfo = GetDependence(assetName);
            string bundleInfo = assetName + "|" + bundleName + ".ab";

            if(dependenceInfo.Count > 0)
            {
                bundleInfo = bundleInfo + "|" + string.Join("|", dependenceInfo.ToArray());
            }

            bundleInfos.Add(bundleInfo);

        }
        File.WriteAllLines(PathTool.OutBundlePath + "/" + AppConst.FileListName, bundleInfos.ToArray());

        if(Directory.Exists(PathTool.OutBundlePath))
        {
            Directory.Delete(PathTool.OutBundlePath, true);
        }
        Directory.CreateDirectory(PathTool.OutBundlePath);



        BuildPipeline.BuildAssetBundles(PathTool.OutBundlePath, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, target);
        File.WriteAllLines(PathTool.OutBundlePath + "/" + AppConst.FileListName, bundleInfos.ToArray());
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 获取依赖列表
    /// </summary>
    /// <param name="curFile"></param>
    /// <returns></returns>
    static List<string> GetDependence(string curFile)
    {
        List<string> dependence = new List<string>();
        string[] files = AssetDatabase.GetDependencies(curFile);
        dependence = files.Where(File => !File.EndsWith(".cs") && !File.Equals(curFile)).ToList();
        return dependence;
    }



}
