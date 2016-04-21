using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseModel
{

}

public class ModelManager : MonoBehaviour
{
    static ModelManager _instance;

    public static ModelManager Instance
    {
        get{return _instance;}
    }

    Dictionary<Type, BaseModel> _modelMap = new Dictionary<Type,BaseModel>();

    void Register<T>() where T : BaseModel, new()
    {

        _modelMap.Add( typeof(T), new T() );
    }

    public T Get<T>( ) where T:BaseModel
    {
        BaseModel m;
        if ( _modelMap.TryGetValue(typeof(T), out m) )
        {
            return m as T;
        }

        return default(T);
    }

    /// <summary>
    /// 需要将ModelManager初始化设为使用前的所有MonoBehavior
    /// </summary>
    void Awake( )
    {
        _instance = this;

        Register<LoginModel>();
        Register<CharListModel>();

    }
}

