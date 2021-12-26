using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class AppConst
{
    public const string BundleExtension = ".ab";
    public const string FileListName = "filelist.txt";
    public static GameMode gameMode = GameMode.EditorMode;
    public const string ResourcesUrl = "http://127.0.0.1/AssetBundles";//热更地址
}

public enum GameMode
{
    EditorMode,
    PackgeMode,
    UpdateMode,
}

