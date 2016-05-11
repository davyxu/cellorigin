
using System;

namespace ProtobufText
{
    public class Message
    {
        object _ins;
        Type _type;
        public Message(object ins)
        {
            _ins = ins;
            _type = ins.GetType();
        }


        public void SetValue( string field, string value )
        {
            var prop = _type.GetProperty(field);
            if (prop == null)
                return;

            if ( prop.PropertyType.IsGenericType )
            {
                // 取泛型第一个类型
                // 取泛型第一个类型
                var elementType = prop.PropertyType.GetGenericArguments()[0];

                object v;

                // 枚举List
                if ( elementType.IsEnum )
                {
                    v = Enum.Parse(elementType, value);
                }
                else
                {
                    v = Convert.ChangeType(value, elementType);
                }

                // 找到这个字段List的实例
                var listIns = prop.GetValue(_ins, null);

                // List没有分配实例
                if ( listIns != null )
                {
                    // 取出List的Add函数
                    var listAdd = listIns.GetType().GetMethod("Add");

                    // 调用Add函数传入创建好的对象
                    listAdd.Invoke(listIns, new object[] { v });
                }
            }
            else if ( prop.PropertyType.IsEnum )
            {
                object v = Enum.Parse(prop.PropertyType, value);

                prop.SetValue(_ins, v, null);
                
            }
            else
            {
                object v = Convert.ChangeType(value, prop.PropertyType);

                prop.SetValue(_ins, v, null);
            }

            
        }

        public Message AddMessage(string field)
        {
            var prop = _type.GetProperty(field);
            if (prop == null)
                return null;

            // 泛型肯定是List, 且肯定是Repeated
            if ( prop.PropertyType.IsGenericType )
            {
                // 取泛型第一个类型
                var elementType = prop.PropertyType.GetGenericArguments()[0];

                // 创建List<T>中的T类型实例
                var newIns = Activator.CreateInstance(elementType);

                // 找到这个字段List的实例
                var listIns = prop.GetValue(_ins, null);

                // 取出List的Add函数
                var listAdd = listIns.GetType().GetMethod("Add");

                // 调用Add函数传入创建好的对象
                listAdd.Invoke(listIns, new object[]{newIns});


                var msg = new Message(newIns);


                return msg;
            }
            else
            {
                var newIns = Activator.CreateInstance(prop.PropertyType);

                var msg = new Message(newIns);


                prop.SetValue(_ins, newIns, null);

                return msg;
            }

           
        }

        



    }
}
