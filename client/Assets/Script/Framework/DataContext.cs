using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{

    public enum WidgetType
    {
        Unknown,        
        Button,
        InputField,
        Text,
        Image,
        View,        
        ScrollRect,
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
        [HideInInspector]
        public WidgetType Type;

        // 数据同步类型
        [HideInInspector]
        public DataSyncType SyncType;     

        // 控件设置

        // 控件名称
        public string Name 
        { 
            get { return gameObject.name; } 
        }

        // 控件做Item时的名称
        public string ItemName
        {
            get
            {
                var listctrl = gameObject.GetComponent<ListControl>();
                if (listctrl == null)
                    return "ListControlLost";

                if (listctrl.ItemPrefab == null)
                    return "ItemPrefabNotSet";

                return listctrl.ItemPrefab.name;
            }
        }

  


        public void Detect()
        {
            // 启动探测类型
            Type = DetectType(gameObject);

            if (!CheckName(Name))
            {
                Type = WidgetType.Unknown;
                Debug.LogError(string.Format("UIBinder: 对象名包含非法字符(不能以数字开头), 生成代码会导致错误, {0} ", gameObject.name));
            }

            SyncType = Framework.DataSyncType.PresenterToView;
        }

        public void DetectAllChild( )
        {
            foreach (Transform trans in transform)
            {
                var ctx = trans.GetComponent<DataContext>();
                if (ctx == null)
                    continue;

                ctx.Detect();
            }
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
        public void AddToTopChild()
        {
            foreach (Transform trans in transform)
            {
                // 子控件是可探测类型, 自动添加
                if (trans.gameObject.GetComponent<DataContext>() == null)
                {
                    var ctx = trans.gameObject.AddComponent<DataContext>();
                    ctx.Detect();
                }   
            }
        }

        /// <summary>
        /// 从顶级子对象中移除本类
        /// </summary>
        public void RemoveAllChild()
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
        static WidgetType DetectType(GameObject go)
        {
            if (go.GetComponent<Button>() != null)
            {
                return WidgetType.Button;
            }

            if (go.GetComponent<ScrollRect>() != null)
            {
                return WidgetType.ScrollRect;
            }

            if (go.GetComponent<InputField>() != null)
            {
                return WidgetType.InputField;
            }

            if (go.GetComponent<Text>() != null)
            {
                return WidgetType.Text;
            }

            if (go.GetComponent<Image>() != null)
            {
                return WidgetType.Image;
            }

            return WidgetType.Unknown;
        }
    }


}