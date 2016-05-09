using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Framework
{
    public class UICodeGen : MonoBehaviour
    {

        // 可被生成器识别的类型

        public const string OutputPath = "Assets/Script/UI";


        [MenuItem("UITools/生成当前场景UI绑定代码")]
        public static void GenCode()
        {
            var rootObj = Selection.activeGameObject;
            if (rootObj == null)
                return;

            var viewContext = rootObj.GetComponent<DataContext>();
            if (viewContext == null)
            {
                Debug.LogError("根对象组件类型没有发现DataContext组件");
                return;
            }


            if ( viewContext.Type != WidgetType.View )
            {
                Debug.LogError("根对象组件类型必须是View类型");
                return;
            }

            var ctxList = new List<DataContext>();

            foreach (Transform childTrans in rootObj.transform)
            {
                IterateDataContext(childTrans, ref ctxList);
            }
            

            var gen = new CodeGenerator();

            ViewTemplate.Class(gen, viewContext, ctxList);

            

            //var rootObject = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

            //// 暂时只支持1个Canvas情况
            //var canvasObj = FindGameObjectByType(rootObject, WidgetType.Canvas);
            //ScanWindowObject(canvasObj);

            //// 生成完成后, 刷新数据库, 显示出来
            //AssetDatabase.Refresh();
        }
        


        static void IterateDataContext(Transform trans, ref List<DataContext> ctxList )
        {
            var childContext = trans.GetComponent<DataContext>();
            if (childContext != null)
            {
                ctxList.Add(childContext);
            }

            foreach (Transform childTrans in trans)
            {
                IterateDataContext(childTrans, ref ctxList);
            }
        }

        ///// <summary>
        ///// 找到给定的绑定器上的所有顶级子窗口对象
        ///// </summary>
        ///// <param name="canvas"></param>
        //static void ScanWindowObject(DataContext canvas)
        //{
        //    if (canvas == null)
        //        return;

        //    foreach (Transform trans in canvas.transform)
        //    {
        //        var binder = trans.GetComponent<DataContext>();
        //        if (binder == null)
        //            continue;

        //        if (binder.Type != WidgetTypeWindow)
        //        {
        //            continue;
        //        }

        //        var win = new UIGenWindow(binder);

        //        // 代码目录预创建
        //        win.PrepareFolder();

        //        // 绑定代码
        //        {
        //            var text = win.PrintAutoBindCode();
        //            win.WriteFile(string.Format("{0}_AutoBind.cs", win.Name), text);
        //        }

        //        // 当主逻辑文件存在时, 不覆盖
        //        if (!win.MainLogicFileExists)
        //        {
        //            var text = win.PrintMainLogicCode();
        //            win.WriteFile(string.Format("{0}.cs", win.Name), text);
        //        }
        //    }
        //}
    }

}