using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLua;
using System.IO;
using System;

public class LuaManager : MonoBehaviour {

	public List<string> LuaNames = new List<string>();
	private Dictionary<string, byte[]> m_LuaScripts;

	public LuaSvr svr; 


    private LuaFunction _luaStart = null;
	private LuaFunction _luaUpdate = null;
	private LuaFunction _luaLateUpdate = null;
	private LuaFunction _luaFixedUpdate = null;
	private LuaFunction _luaAwake = null;
	private LuaFunction _luaOnDisable = null;
	private LuaFunction _luaOnDestroy = null;

	private void Awake()
    {
		
    }

	private void Start()
    {
		if (_luaStart != null)
        {
			_luaStart.call();
        }
    }
	/*
	private static byte[] LuaResourcesFileLoader(string strFile, ref string fn)
    {
		string filename = Application.dataPath + "/Scripts/Lua/" + strFile.Replace('.', '/') + ".txt";
		return File.ReadAllBytes(filename);
	}
	*/
	public byte[] Loader(string n,ref string name)
	{
		return GetLuaScript(name);
	}
	public byte[] GetLuaScript(string name)
	{
		name = name.Replace(".", "/");
		string fileName = PathTool.GetLuaPath(name);

		byte[] luaScript = null;
		if (!m_LuaScripts.TryGetValue(fileName, out luaScript))
		{
			Debug.LogError("this lua scripts is not exists");
		}
		return luaScript;
	}

	void LoadLuaScript()
    {
		foreach(string name in LuaNames)
        {
			Manager.Resource.LoadLua(name, (UnityEngine.Object obj) =>
			{
				AddLuaScript(name, (obj as TextAsset).bytes);
				if(m_LuaScripts.Count >= LuaNames.Count)
                {
					LuaNames.Clear();
					LuaNames = null;
                }
			});
        }
    }

#if UNITY_EDITOR
	void EditorLoadLuaScript()
    {
		string[] luaFiles = Directory.GetFiles(PathTool.LuaPath, "*bytes", SearchOption.AllDirectories);
		for(int i = 0; i < luaFiles.Length; i++)
        {
			string fileName = PathTool.GetStandardPath(luaFiles[i]);
			byte[] file = File.ReadAllBytes(fileName);
			AddLuaScript(PathTool.GetUnityPath(fileName), file);
		}
		
    }
#endif

	private void AddLuaScript(string assetsName, byte[] lua)
	{
		m_LuaScripts[assetsName] = lua;
	}


	public void Init()
    {
		svr = new LuaSvr();

		LuaSvr.mainState.loaderDelegate += Loader;
		svr.init(null, () =>
		{
			svr.start("Main");
			
			_luaAwake = LuaSvr.mainState.getFunction("Awake");
			_luaStart = LuaSvr.mainState.getFunction("Start");
			_luaFixedUpdate = LuaSvr.mainState.getFunction("FixedUpdate");
			_luaUpdate = LuaSvr.mainState.getFunction("Update");
			_luaLateUpdate = LuaSvr.mainState.getFunction("LateUpdate");
			_luaOnDestroy = LuaSvr.mainState.getFunction("OnDestroy");
			_luaOnDisable = LuaSvr.mainState.getFunction("OnDisable");
			
		});
		
		if (_luaAwake != null)
		{
			_luaAwake.call();
		}
		
		m_LuaScripts = new Dictionary<string, byte[]>();
		if (AppConst.gameMode != GameMode.EditorMode)
			LoadLuaScript();
		else
        {
#if UNITY_EDITOR
			EditorLoadLuaScript();
#endif
		}
			
    }

	void FixedUpdate()
    {
		if(_luaFixedUpdate != null)
        {
			_luaFixedUpdate.call();
        }
    }
	void Update()
	{
		if (_luaUpdate != null)
		{
			_luaUpdate.call();
		}
	}
	void LateUpdate()
	{
		if (_luaLateUpdate != null)
		{
			_luaFixedUpdate.call();
		}
	}
	void OnDestroy()
	{
		if (_luaOnDestroy != null)
		{
			_luaOnDestroy.call();
		}
	}

	void OnDisable()
	{
		if (_luaOnDisable != null)
		{
			_luaOnDestroy.call();
		}
	}
}
