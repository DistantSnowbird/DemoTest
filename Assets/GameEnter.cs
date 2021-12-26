using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnter : MonoBehaviour {

    public GameMode GameMode;

    void Awake()
    {
        AppConst.gameMode = this.GameMode;
        DontDestroyOnLoad(this);

        Manager.Resource.ParesVersionFile();
        Manager.Lua.Init();
        
    }
}
