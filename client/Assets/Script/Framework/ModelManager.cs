using System;
using System.Collections.Generic;

namespace Framework
{
    public class ModelManager : Singleton<ModelManager>
    {
        Dictionary<Type, BaseModel> _modelMap = new Dictionary<Type, BaseModel>();

        void Register<T>() where T : BaseModel, new()
        {

            _modelMap.Add(typeof(T), new T());
        }

        public T Get<T>() where T : BaseModel
        {
            Init();

            BaseModel m;
            if (_modelMap.TryGetValue(typeof(T), out m))
            {
                return m as T;
            }

            return default(T);
        }

        static bool _initDone;

        void Init()
        {
            if (_initDone)
                return;

            // TODO 从配置表导入初始化

            Register<LoginModel>();
            Register<CharListModel>();

            _initDone = true;
        }
    }


}