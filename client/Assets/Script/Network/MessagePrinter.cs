using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;


public class MessagePrinter
{
    bool _singleLineMode = true;
    bool _useShortRepeatedPrimitives = true;
    StringBuilder _builder = new StringBuilder();

    delegate void FieldIterCallBack(Type root, MemberInfo field, PropertyInfo prop, object parent);

    /// <summary>
    /// 单行模式
    /// </summary>
    public bool SingleLineMode
    {
        get { return _singleLineMode; }
        set { _singleLineMode = value; }
    }

    /// <summary>
    /// 用缩略的repeated模式
    /// </summary>
    public bool UseShortRepeatedPrimitives
    {
        get { return _useShortRepeatedPrimitives; }
        set { _useShortRepeatedPrimitives = value; }
    }

    public string Print(object msg)
    {
        _builder.Length = 0;

        RawPrint(msg.GetType(), msg);

        return _builder.ToString();
    }

    void RawPrint(Type msgType, object parent)
    {
        InteratorField(msgType, parent, (root, type, prop, msg) =>
        {
            PrintField(root, type, prop, parent);
        });
    }

    void PrintMsg(Type msgType, object parent)
    {
        _builder.Append(parent.GetType().FullName);
        _builder.Append(SingleLineMode ? "{" : "{\n");

        InteratorField(msgType, parent, (root, type, prop, msg) =>
        {
            PrintField(root, type, prop, parent);
        });

        _builder.Append(SingleLineMode ? "}" : "}\n");
    }

    void PrintField(Type root, MemberInfo field, PropertyInfo prop, object parent)
    {
        if (IsRepeated(root.GetProperty(field.Name)))
        {
            var collection = (System.Collections.IEnumerable)prop.GetValue(parent, null);

            Type colleType = prop.PropertyType;
            var arg = colleType.GetGenericArguments();
            var t = arg[0];

            if (UseShortRepeatedPrimitives
                && (IsValue(t) || t.IsEnum))
            {
                PrintShortRepeatedField(root, field, prop, parent);
                _builder.Append(SingleLineMode ? "" : "\n");
                return;
            }
            else
            {
                if (IsValue(t) || t.IsEnum)
                {
                    foreach (var c in collection)
                    {
                        PrintFieldName(field.Name);
                        PrintFieldValue(c);
                    }
                }
                else
                {
                    PrintFieldName(field.Name);
                    if (SingleLineMode)
                    {
                        _builder.Append("{");
                    }
                    else
                    {
                        _builder.Append("{\n");
                    }

                    foreach (var c in collection)
                    {
                        PrintMsg(c.GetType(), c);
                        _builder.Append(" ");
                    }

                    if (SingleLineMode)
                    {
                        _builder.Append("}");
                    }
                    else
                    {
                        _builder.Append("}\n");
                    }
                }
            }
        }
        else
        {
            PrintFieldName(field.Name);

            if (IsValue(prop.PropertyType) || IsEnum(prop))
            {
                PrintFieldValue(prop.GetValue(parent, null));
            }
            else
            {
                if (SingleLineMode)
                {
                    _builder.Append("{");
                }
                else
                {
                    _builder.Append("{\n");
                }

                object msg = prop.GetValue(parent, null);
                if (msg != null)
                {
                    PrintMsg(msg.GetType(), msg);
                    _builder.Append(" ");
                }
                if (SingleLineMode)
                {
                    _builder.Append("}");
                }
                else
                {
                    _builder.Append("}\n");
                }
            }
        }
    }

    void PrintShortRepeatedField(Type root, MemberInfo field, PropertyInfo prop, object parent)
    {
        PrintFieldName(field.Name);
        _builder.Append(SingleLineMode ? "[" : "[\n");
        var collection = (System.Collections.IEnumerable)prop.GetValue(parent, null);

        int idx = 0;
        foreach (var c in collection)
        {
            if (idx > 0)
            {
                _builder.Append(",");
                _builder.Append(" ");
            }

            if (IsString(c.GetType().FullName))
            {
                if (string.IsNullOrEmpty(c as string))
                {
                    _builder.Append("\"\"");
                }
                else
                {
                    _builder.Append(c as string);
                }
            }
            else
            {
                _builder.Append(c.ToString());
            }
            idx++;
        }
        _builder.Append(SingleLineMode ? "] " : "]\n");
    }
    void PrintFieldName(string Name)
    {
        _builder.Append(Name);
    }

    void PrintFieldValue(object value)
    {
        if (value == null)
            return;

        _builder.Append(":");
        if (IsString(value.GetType().FullName))
        {
            if (String.IsNullOrEmpty(value as string))
            {
                _builder.Append("\"\"");
            }
            else
            {
                _builder.Append("\"");
                _builder.Append(value as string);
                _builder.Append("\"");
            }
        }
        else
        {
            _builder.Append(value);
        }

        if (SingleLineMode)
        {
            _builder.Append(" ");
        }
        else
        {
            _builder.Append("\n");
        }
    }


    void InteratorField(Type type, object msg, FieldIterCallBack callback)
    {
        var member = type.GetMembers();
        foreach (var mem in member)
        {               
            var attributes = mem.GetCustomAttributes(false);
            foreach (var att in attributes)
            {
                if (att is global::ProtoBuf.ProtoMemberAttribute)
                {
                    if (callback != null)
                    {
                        callback.Invoke(type, mem, type.GetProperty(mem.Name), msg);
                    }
                }
            }
        }
    }

    static bool IsValue(Type type)
    {
        string FullName = type.FullName;
        return FullName == "System.String"
                            || FullName == "System.Int32"
                            || FullName == "System.UInt32"
                            || FullName == "System.Int64"
                            || FullName == "System.Single"
                            || FullName == "System.Boolean"
                            || FullName == "System.Double";
    }

    static bool IsString(string FullName)
    {
        return FullName == "System.String";
    }

    static bool IsRepeated(System.Reflection.PropertyInfo info)
    {
        System.Type type = info.PropertyType;
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    }

    static bool IsEnum(System.Reflection.PropertyInfo info)
    {
        return info.PropertyType.IsEnum;
    }
}
