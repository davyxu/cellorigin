using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{

    public enum ObjectDetectType
    {
        Unknown,
        GenAsCanvas,
        GenAsButton,
        GenAsInputField,
        GenAsText,
        GenAsImage,
        GenAsWindow,
        GenAsDropdown,
    }

    public enum DataSyncType
    {
        PresenterToView, // 数据变化同步到View
        ViewToPresenter,// 控件变化时同步到Presenter, 多见于输入框        
        TwoWay,
    }

    /// <summary>
    /// 将这个类添加到控件上, 代码生成系统就可以识别该类
    /// </summary>
   // [ExecuteInEditMode]
    public class DataContext : MonoBehaviour
    {

        public ObjectDetectType Type;

        // 控件设置

        public string NameInView;

        public string NameInPresenter;

        public string NameInModel;

        public DataSyncType SyncType;       

        // View设置
        public Framework.BaseView View;
        public UnityEngine.Object Presenter;
        public UnityEngine.Object Model;

        public string CodeGenPath; // 代码生成路径


        public void SmartFill()
        {
            // 启动探测类型
            Type = DetectType(gameObject);

            if (!CheckName(gameObject.name))
            {
                Type = ObjectDetectType.Unknown;
                Debug.LogError(string.Format("UIBinder: 对象名包含非法字符(不能以数字开头), 生成代码会导致错误, {0} ", gameObject.name));
            }

            var name = gameObject.name;
            NameInView = name;
            NameInPresenter = name;
            NameInModel = name;

            SyncType = Framework.DataSyncType.PresenterToView;
            CodeGenPath = "Assets/Script/UI";

        }




        /// <summary>
        /// // 防止命名不规范, 导致代码生成错误
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CheckName(string name)
        {
            var c = name[0];
            return Char.IsLetter(c) || c == '_';
        }

        /// <summary>
        /// 将本类添加到所有的顶级子对象中
        /// </summary>
        public void AddBinderToAllTopChild()
        {
            foreach (Transform trans in transform)
            {
                // 子控件是可探测类型, 自动添加
                if (DetectType(trans.gameObject) != ObjectDetectType.Unknown)
                {
                    if (trans.gameObject.GetComponent<DataContext>() == null)
                    {
                        trans.gameObject.AddComponent<DataContext>();
                    }
                }
            }
        }

        /// <summary>
        /// 从顶级子对象中移除本类
        /// </summary>
        public void RemoveAllTopChildBinder()
        {
            foreach (Transform trans in transform)
            {
                GameObject.DestroyImmediate(trans.gameObject.GetComponent<DataContext>());
            }
        }


        /// <summary>
        /// // 探测组件构成, 以决定这个对象的本身可能的用途
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        static ObjectDetectType DetectType(GameObject go)
        {
            if (go.GetComponent<Canvas>() != null &&
                    go.GetComponent<CanvasScaler>() != null &&
                    go.GetComponent<GraphicRaycaster>() != null)
            {
                return ObjectDetectType.GenAsCanvas;
            }

            // 父级是canvas, 且带有UI尾缀, 识别为窗口
            if (go.transform.parent != null &&
                go.transform.parent.GetComponent<Canvas>() &&
                go.name.EndsWith("UI"))
            {
                return ObjectDetectType.GenAsWindow;
            }


            if (go.GetComponent<Button>() != null)
            {
                return ObjectDetectType.GenAsButton;
            }

            if (go.GetComponent<Dropdown>() != null)
            {
                return ObjectDetectType.GenAsDropdown;
            }

            if (go.GetComponent<InputField>() != null)
            {
                return ObjectDetectType.GenAsInputField;
            }

            if (go.GetComponent<Text>() != null)
            {
                return ObjectDetectType.GenAsText;
            }

            if (go.GetComponent<Image>() != null)
            {
                return ObjectDetectType.GenAsImage;
            }

            return ObjectDetectType.Unknown;
        }
    }


}