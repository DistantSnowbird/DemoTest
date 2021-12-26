using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLua;
using System.IO;

public delegate byte[] LuaResourcesFileLoader(string fn, ref string absoluteFn);
public class openmylua : MonoBehaviour {
	void Start () {
		LuaSvr svr = new LuaSvr();
		LuaSvr.mainState.loaderDelegate += LuaResourcesFileLoder;
		svr.init(null, () =>
		{
			svr.start("luaTest");
		});

	}

	private static byte[] LuaResourcesFileLoder(string strFile, ref string fn)
    {
		string filename = Application.dataPath + "/Scripts/Lua/" + strFile.Replace('.', '/') + ".txt";
		return File.ReadAllBytes(filename);
    }


}
