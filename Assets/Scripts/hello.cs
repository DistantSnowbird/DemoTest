using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLua;

public class hello : MonoBehaviour
{
    private static LuaState ls_state;
    // Start is called before the first frame update
    void Start()
    {
        ls_state = new LuaState();
        ls_state.doString("print(\"Hello Lua!\")");
    }

    // Update is called once per frame
    void Update()
    {

    }
}