using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UObject = UnityEngine.Object;

public class ResourceManager : MonoBehaviour {
    internal class BundleInfo
    {
        public string AssetsName;
        public string BundleName;
        public List<string> Dependences;
    }

    private Dictionary<string, BundleInfo> m_BundleInfos = new Dictionary<string, BundleInfo>();

    /// <summary>
    /// 解析版本文件
    /// </summary>
    public void ParesVersionFile()
    {
        string url = Path.Combine(PathTool.BundleResourcePath, AppConst.FileListName);
        string[] data = File.ReadAllLines(url);

        for(int i = 0; i < data.Length; i++)
        {
            BundleInfo bundleInfo = new BundleInfo();
            string[] info = data[i].Split('|');
            bundleInfo.AssetsName = info[0];
            bundleInfo.BundleName = info[1];

            bundleInfo.Dependences = new List<string>(info.Length - 2);
            for(int j = 2; j < info.Length; j++)
            {
                bundleInfo.Dependences.Add(info[j]);
            }
            m_BundleInfos.Add(bundleInfo.AssetsName, bundleInfo);
            if(info[0].IndexOf("LuaScripts") > 0)
            {
                Manager.Lua.LuaNames.Add(info[0]);
            }
        }
    }

    /// <summary>
    /// 异步载入资源
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="action">回调</param>
    /// <returns></returns>
    IEnumerator LoadBundleAsync(string assetName, Action<UObject> action = null)
    {
        string bundleName = m_BundleInfos[assetName].BundleName;
        string bundlePath = Path.Combine(PathTool.BundleResourcePath, bundleName);
        List<string> dependences = m_BundleInfos[assetName].Dependences;
        if(dependences != null && dependences.Count > 0)
        {
            for(int i = 0; i < dependences.Count; i++)
            {
                yield return LoadBundleAsync(dependences[i]);
            }
        }
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return request;

        AssetBundleRequest bundleRequest = request.assetBundle.LoadAssetAsync(assetName);
        yield return bundleRequest;
        
        if (action != null && bundleRequest != null)
        {
            action.Invoke(bundleRequest.asset);
        }
            
    }
#if UNITY_EDITOR
    void EditorLoadAsset(string assetName, Action<UObject> action = null)
    {
        UObject obj = UnityEditor.AssetDatabase.LoadAssetAtPath(assetName, typeof(UObject));
        if (obj == null)
        {
            Debug.LogError("assets name is not exist:" + "assetName");
        }
        if (action != null)
            action.Invoke(obj);
    }
#endif


    public void LoadAsset(string assetName, Action<UObject> action)
    {
        StartCoroutine(LoadBundleAsync(assetName, action));
    }

    


    public void LoadUI(string assetName, Action<UObject> action = null)
    {
        LoadAsset(PathTool.GetUIPath(assetName), action);
    }

    public void LoadScene(string assetName, Action<UObject> action = null)
    {
        LoadAsset(PathTool.GetScenePath(assetName), action);
    }

    public void LoadLua(string assetName, Action<UObject> action = null)
    {
        LoadAsset(assetName, action);
    }

    public void LoadAudio(string assetName, Action<UObject> action = null)
    {
        LoadAsset(PathTool.GetAudioPath(assetName), action);
    }

 

    

}
