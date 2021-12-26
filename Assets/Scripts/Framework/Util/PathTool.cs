using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTool {
	//根路径、打包路径、输出路径
	public static readonly string AssetsPath = Application.dataPath;
	public static readonly string BuildBundlePath = AssetsPath + "/BuildResources/";
	public static readonly string OutBundlePath = Application.streamingAssetsPath;

	public static readonly string ReadPath = Application.streamingAssetsPath;
	public static readonly string ReadWritePath = Application.persistentDataPath;
	public static readonly string LuaPath = "Assets/BuildResources/LuaScripts";
	public static string BundleResourcePath
    {
        get
		{
			if (AppConst.gameMode == GameMode.UpdateMode)
				return ReadWritePath;
			return ReadPath;
		}
    }
	
	/// <summary>
	/// 获取unity的相对路径
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static string GetUnityPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
			return string.Empty;
        }
		return path.Substring(path.IndexOf("Assets"));
    }

	/// <summary>
	/// 获取标准的路径格式
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static string GetStandardPath(string path)
    {
		if (string.IsNullOrEmpty(path))
		{
			return string.Empty;
		}
		return path.Trim().Replace("\\", "/");
	}
	 
	public static string GetLuaPath(string name)
    {
		return string.Format("Assets/BuildResources/LuaScripts/{0}.bytes", name);
    }

	public static string GetUIPath(string name)
	{
		return string.Format("Assets/BuildResources/UI/Prefabs/{0}.prefab", name);
	}

	public static string GetAudioPath(string name)
	{
		return string.Format("Assets/BuildResources/Audio/{0}", name);
	}

	public static string GetScenePath(string name)
	{
		return string.Format("Assets/BuildResources/Scene/{0}.unity", name);
	}

}
