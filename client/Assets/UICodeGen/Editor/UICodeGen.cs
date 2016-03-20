using UnityEngine;
using UnityEditor;


public class UICodeGen : MonoBehaviour
{

    // 可被生成器识别的类型

    public const string OutputPath = "Assets/Script/UI";


    [MenuItem("UICodeGen/生成代码")]
    public static void GenCode( )
    {
        var rootObject = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

         // 暂时只支持1个Canvas情况
        var canvasObj = FindGameObjectByType(rootObject, CodeGenObjectType.GenAsCanvas );
        ScanWindowObject(canvasObj);

        // 生成完成后, 刷新数据库, 显示出来
        AssetDatabase.Refresh();
    }

    static UIBinder FindGameObjectByType( GameObject[] objlist, CodeGenObjectType type )
    {
        foreach( GameObject go in objlist )
        {
            var binder = go.GetComponent<UIBinder>();
            if (binder == null)
                continue;

            if (binder.Type == type)
            {
                return binder;
            }
        }

        return null;
    }

    static void ScanWindowObject(UIBinder canvas)
    {
        if (canvas == null)
            return;

        foreach (Transform trans in canvas.transform)
        {
            var binder = trans.GetComponent<UIBinder>();
            if (binder == null)
                continue;

            if (binder.Type != CodeGenObjectType.GenAsWindow )
            {
                continue;
            }

            var win = new UIGenWindow(binder);
            
            // 代码目录预创建
            win.PrepareFolder();

            // 绑定代码
            {
                var text = win.PrintAutoBindCode();
                win.WriteFile(string.Format("{0}_AutoBind.cs", win.Name), text);
            }
            
            // 当主逻辑文件存在时, 不覆盖
            if ( !win.MainLogicFileExists )
            {
                var text = win.PrintMainLogicCode();
                win.WriteFile(string.Format("{0}.cs", win.Name), text);
            }
        }
    }




}
