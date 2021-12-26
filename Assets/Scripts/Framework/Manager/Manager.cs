using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	private static ResourceManager _resource;
	public static ResourceManager Resource
    {
        get { return _resource; }
    }

    private static LuaManager _luaManager;

    public static LuaManager Lua
    {
        get { return _luaManager; }
    }

    private void Awake()
    {
        _resource = this.gameObject.AddComponent<ResourceManager>();
        _luaManager = this.gameObject.AddComponent<LuaManager>();
    }
}
