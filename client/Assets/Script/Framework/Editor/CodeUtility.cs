using System;
using System.IO;
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
    }
}
