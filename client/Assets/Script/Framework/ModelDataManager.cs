using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    class ModelContext
    {
        public Action<gamedef.ModelSyncBehavior> Changed;
        public gamedef.ModelSyncBehavior SyncBehavior = gamedef.ModelSyncBehavior.MSB_None;

        public Type OutputType;
    }

    class IntegerModel : ModelContext
    {
        public int Data;

        public override string ToString()
        {
            // 普通整形类型
            if ( OutputType == null )
            {
                return Data.ToString();
            }

            // 转换枚举
            return Convert.ChangeType(Data, OutputType).ToString();
        }
    }

    class StringModel : ModelContext
    {
        public string Data;

        public override string ToString()
        {
            return Data.ToString();
        }
    }

    class FloatModel : ModelContext
    {
        public float Data;

        public override string ToString()
        {
            return Data.ToString();
        }
    }

    class BoolModel : ModelContext
    {
        public bool Data;

        public override string ToString()
        {
            return Data.ToString();
        }
    }



    public class ModelDataManager : Singleton<ModelDataManager>
    {
        Dictionary<string, ModelContext> _DataMap = new Dictionary<string, ModelContext>();

        public void Register<ModelType>( string key, Type outType = null)
        {
            var data = Activator.CreateInstance<ModelType>() as ModelContext;
            data.OutputType = outType;
            _DataMap.Add(key, data);
        }

        public void Listen( string key, Action<gamedef.ModelSyncBehavior> callback )
        {
            var data = GetModel(key);
            if ( data != null )
            {
                data.Changed += callback;
            }
        }

        ModelType GetValue<ModelType>(string key) where ModelType : ModelContext
        {
            var data = GetModel(key);
            if (data == null)
                return default(ModelType);

            return data as ModelType;           
        }

        ModelContext GetModel(string key)
        {
            ModelContext data;
            if (_DataMap.TryGetValue(key, out data))
            {
                return data;
            }

            return null;
        }



        public string GetValueAsString( string key )
        {
            var data = GetModel(key);
            if (data == null)
                return default(string);

            return data.ToString();
        }


        public void SetInteger( string key, int v, gamedef.ModelSyncBehavior behavior )
        {
            var m = GetValue<IntegerModel>(key);
            if (m == null)
                return;

            m.Data = v;
            if (m.Changed != null)
            {
                m.Changed(behavior);
            }
        }

        public void SetString(string key, string v, gamedef.ModelSyncBehavior behavior)
        {
            var m = GetValue<StringModel>(key);
            if (m == null)
                return;

            m.Data = v;
            if (m.Changed != null)
            {
                m.Changed(behavior);
            }
        }

        public void SetNumber(string key, float v, gamedef.ModelSyncBehavior behavior)
        {
            var m = GetValue<FloatModel>(key);
            if (m == null)
                return;

            m.Data = v;
            if (m.Changed != null)
            {
                m.Changed(behavior);
            }
        }

        public void SetBool(string key, bool v, gamedef.ModelSyncBehavior behavior)
        {
            var m = GetValue<BoolModel>(key);
            if (m == null)
                return;

            m.Data = v;
            if (m.Changed != null )
            {
                m.Changed(behavior);
            }
            
        }


    }

}