using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Framework
{
    class CodeUtility
    {
        public static void WriteFile(string filename, string text)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filename));            

            try
            {
                System.IO.File.WriteAllText(filename, text, System.Text.Encoding.UTF8);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        public static bool MethodExists( string className, string methodName )
        {
            // 加载游戏的二进制
            var ass = Assembly.LoadFile(@"Library\ScriptAssemblies\Assembly-CSharp.dll");

            // 取到类信息
            var classInfo = ass.GetType(className);

            if (classInfo == null)
                return false;

            // 取方法, 查方法是否存在            
            return classInfo.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance) != null;
        }
    }
}
