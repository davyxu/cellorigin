using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

public class Printer
{
    StringBuilder _builder = new StringBuilder();
    string _indend;
    /// <summary>
    /// 单行模式
    /// </summary>
    public bool SingleLineMode { get; set; }

    public Printer( )
    {
        SingleLineMode = true;
    }

    public string Print( object msg )
    {
        if (msg == null)
            return "";
        _builder.Length = 0;

        Iterate(msg);

        return _builder.ToString();
    }

    void Iterate( object ins )
    {
        var propList = ins.GetType().GetProperties();

        foreach (var prop in propList)
        {
            var type = prop.PropertyType;


            if (prop.IsDefined(typeof(Sproto.SprotoHasFieldAttribute), false))
                continue;

            if (IsRepeated(type))
            {
                var elementType = type.GetGenericArguments()[0];


                var collection = prop.GetValue(ins, null) as ICollection;
                if (collection != null)
                {
                    if (collection.Count > 0)
                    {
                        if (IsMessage(elementType))
                        {

                            foreach (var c in collection)
                            {
                                BeginLine();

                                PrintFieldName(prop);

                                EndLine();

                                PrintLine("{ ");
                                In();

                                Iterate(c);

                                Out();
                                PrintLine("} ");

                                EndLine();
                            }
                        }
                        else
                        {
                            foreach (var c in collection)
                            {
                                BeginLine();
                                PrintFieldName(prop);

                                _builder.Append(": ");

                                PrintFieldValue(c);
                                EndLine();
                            }
                        }
                    }
                }
                    

            }
            else
            {

                var value = prop.GetValue(ins, null);
                if ( value != null )
                {
                    BeginLine();

                    PrintFieldName(prop);


                    if (IsMessage(type))
                    {
                        PrintLine("{");
                        In();

                        Iterate(value);

                        Out();
                        PrintLine("}");
                    }
                    else
                    {
                        _builder.Append(": ");


                        PrintFieldValue(value);
                    }

                    EndLine();

                }
            }
        }
    }



    void PrintFieldName(PropertyInfo prop)
    {
        PrintValues(prop.Name);
    }

    void PrintFieldValue( object value )
    {
        if (value == null)
        {
            PrintValues(" ");
            return;
        }       
        if (value.GetType() == typeof(string)) 

        {
            PrintValues("\"", EscapeString(value.ToString()), "\"");
        }
        else
        {
            PrintValues(value.ToString());                
        }

        PrintValues(" ");
    }

    string EscapeString( string str )
    {
        var sb = new StringBuilder();

        foreach( var c in str )
        {
            switch (c)
            {
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\"':
                    sb.Append("\\\"");
                    break;
                default:
                    sb.Append( c );
                    break;
            }
        }

        return sb.ToString();
    }
        


    static bool IsMessage( Type type )
    {
        return type.IsClass && type != typeof(string);
    }

    static bool IsRepeated(Type type)
    {            
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    }


    #region Printer Control

    void In()
    {
        _indend += "\t";
    }


    void Out()
    {
        if (_indend.Length > 0)
        {
            _indend = _indend.Substring(1);
        }
    }

    void BeginLine()
    {
        if (!SingleLineMode)
        {
            _builder.Append(_indend);
        }
    }

    void EndLine()
    {
        if (!SingleLineMode)
        {
            _builder.Append("\n");
        }
    }

    public void PrintLine(params object[] values)
    {
        BeginLine();
        PrintValues(values);
        EndLine();
    }

    public void PrintValues(params object[] values)
    {
        foreach (object obj in values)
        {
            _builder.Append(obj.ToString());
        }
    }

    #endregion

}
