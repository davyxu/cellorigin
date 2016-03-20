using System.Text;

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

    public void PrintLine(params object[] values)
    {
        BeginLine();
        Print(values);
        EndLine();
    }

    public void In( )
    {
        _indend += "\t";
    }

    public void Out( )
    {
        if ( _indend.Length > 0 )
        {
            _indend = _indend.Substring(1);
        }
    }
}
