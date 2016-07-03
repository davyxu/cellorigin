using LuaInterface;
using UnityEngine;

//use menu Lua->Copy lua files to Resources. 之后才能发布到手机
public class CellLuaLoader : LuaClient
{
    static string CellLuaDir;


    protected override LuaFileUtils InitLoader()
    {
        return new LuaResLoader();
    }

    protected override void CallMain()
    {
        LuaFunction func = luaState.GetFunction("Main");
        func.Call();
        func.Dispose();
    }

    protected override void StartMain()
    {
        CellLuaDir = Application.dataPath + "/LuaSupport/Cell";        //tolua lua文件目录
        luaState.AddSearchPath( CellLuaDir);

        luaState.Require("Class");

        luaState.DoFile("Main.lua");
        CallMain();
    }


}
