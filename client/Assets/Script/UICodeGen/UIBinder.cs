using System;
using UnityEngine;
using UnityEngine.UI;

public enum CodeGenObjectType{    Unknown,    GenAsCanvas,    GenAsButton,    GenAsInputField,    GenAsText,    GenAsImage,    GenAsWindow,    GenAsDropdown,}/// <summary>
/// 将这个类添加到控件上, 代码生成系统就可以识别该类
/// </summary>[ExecuteInEditMode]public class UIBinder : MonoBehaviour {    public CodeGenObjectType Type;        /// <summary>
    /// 组件添加到对象时, 自动决定类型
    /// </summary>    void OnEnable( )    {        // 启动探测类型        Type = DetectType(gameObject);        if ( !CheckName( gameObject.name ) )
        {
            Type = CodeGenObjectType.Unknown;
            Debug.LogError(string.Format("UIBinder: Invalid object name to generated code, {0} ", gameObject.name));
        }    }    /// <summary>
    /// // 防止命名不规范, 导致代码生成错误
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>    public static bool CheckName( string name )
    {
        var c = name[0];
        return Char.IsLetter( c) || c == '_' ;        
    }    /// <summary>
    /// 将本类添加到所有的顶级子对象中
    /// </summary>    public void AddBinderToAllTopChild( )    {        foreach( Transform trans in transform )        {            // 子控件是可探测类型, 自动添加            if ( DetectType( trans.gameObject ) != CodeGenObjectType.Unknown )            {
                if (trans.gameObject.GetComponent<UIBinder>() == null)
                {
                    trans.gameObject.AddComponent<UIBinder>();
                }            }        }    }    /// <summary>
    /// 从顶级子对象中移除本类
    /// </summary>    public void RemoveAllTopChildBinder( )    {        foreach (Transform trans in transform)        {            GameObject.DestroyImmediate(trans.gameObject.GetComponent<UIBinder>());        }    }    /// <summary>
    /// // 探测组件构成, 以决定这个对象的本身可能的用途
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>    static CodeGenObjectType DetectType(GameObject go)    {        if (go.GetComponent<Canvas>() != null &&                go.GetComponent<CanvasScaler>() != null &&                go.GetComponent<GraphicRaycaster>() != null)        {            return CodeGenObjectType.GenAsCanvas;        }        // 父级是canvas, 且带有UI尾缀, 识别为窗口        if (go.transform.parent != null &&             go.transform.parent.GetComponent<Canvas>() &&            go.name.EndsWith("UI"))        {            return CodeGenObjectType.GenAsWindow;        }        if (go.GetComponent<Button>() != null)        {            return CodeGenObjectType.GenAsButton;        }

        if (go.GetComponent<Dropdown>() != null)
        {
            return CodeGenObjectType.GenAsDropdown;
        }        if (go.GetComponent<InputField>() != null)        {            return CodeGenObjectType.GenAsInputField;        }        if (go.GetComponent<Text>() != null)        {            return CodeGenObjectType.GenAsText;        }        if (go.GetComponent<Image>() != null)        {            return CodeGenObjectType.GenAsImage;        }        return CodeGenObjectType.Unknown;    }}