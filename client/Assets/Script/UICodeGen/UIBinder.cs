﻿using System;
using UnityEngine;
using UnityEngine.UI;

public enum CodeGenObjectType
/// 将这个类添加到控件上, 代码生成系统就可以识别该类
/// </summary>
    /// 组件添加到对象时, 自动决定类型
    /// </summary>
        {
            Type = CodeGenObjectType.Unknown;
            Debug.LogError(string.Format("UIBinder: Invalid object name to generated code, {0} ", gameObject.name));
        }
    /// // 防止命名不规范, 导致代码生成错误
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    {
        var c = name[0];
        return Char.IsLetter( c) || c == '_' ;        
    }
    /// 将本类添加到所有的顶级子对象中
    /// </summary>
                if (trans.gameObject.GetComponent<UIBinder>() == null)
                {
                    trans.gameObject.AddComponent<UIBinder>();
                }
    /// 从顶级子对象中移除本类
    /// </summary>
    /// // 探测组件构成, 以决定这个对象的本身可能的用途
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>

        if (go.GetComponent<Dropdown>() != null)
        {
            return CodeGenObjectType.GenAsDropdown;
        }