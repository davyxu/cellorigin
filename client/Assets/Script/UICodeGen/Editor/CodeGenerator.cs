using System.Text;

/// <summary>
/// 代码生成类
/// </summary>
public class CodeGenerator
{
    StringBuilder _builder = new StringBuilder();
    string _indend;

    public override string ToString()
    {
        return _builder.ToString();
    }

    public void Print(params object[] values)
    {
        foreach( object obj in values )
        {
            _builder.Append(obj.ToString());
        }
    }

    public void BeginLine( )
    {
        _builder.Append(_indend);
    }

    public void EndLine()
    {
        _builder.Append("\n");
    }

    /// <summary>
    /// 把多个参数打到一行里
    /// </summary>
    /// <param name="values"></param>
    public void PrintLine(params object[] values)
    {
        BeginLine();
        Print(values);
        EndLine();
    }

    /// <summary>
    /// 进入一个层次, 例如括号
    /// </summary>
    public void In( )
    {
        _indend += "\t";
    }

    /// <summary>
    /// 退出一个层次
    /// </summary>
    public void Out( )
    {
        if ( _indend.Length > 0 )
        {
            _indend = _indend.Substring(1);
        }
    }
}
