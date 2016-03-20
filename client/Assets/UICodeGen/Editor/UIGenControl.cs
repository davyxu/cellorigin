using System.Collections.Generic;
using UnityEngine;

class UIGenControl
{
    UIBinder _binder;

    public UIGenControl( UIBinder binder )
    {
        _binder = binder;
    }

    public void PrintDeclareCode( CodeGenerator gen )
    {
        gen.PrintLine(GetVarType(), " _", Name, ";");
    }

    public void PrintAttachCode(CodeGenerator gen)
    {
        var path = ObjectUtility.GetGameObjectPath(_binder.gameObject);
        gen.PrintLine("_", Name, " = GameObject.Find(\"", path, "\").GetComponent<", GetVarType(), ">();");
    }

    public void PrintButtonClickRegisterCode( CodeGenerator gen )
    {
        if (_binder.Type != CodeGenObjectType.GenAsButton)
            return;

        gen.PrintLine("_", Name, ".onClick.AddListener( On", Name, " );");
    }

    public void PrintButtonClickImplementCode(CodeGenerator gen)
    {
        if (_binder.Type != CodeGenObjectType.GenAsButton)
            return;

        gen.PrintLine("void On", Name, "( )");
        gen.PrintLine("{");
        gen.PrintLine();
        gen.PrintLine("}");
    }

    public string Name
    {
        get { return _binder.gameObject.name; }
    }

    string GetVarType( )
    {
        switch( _binder.Type )
        {
            case CodeGenObjectType.GenAsButton:
                return "Button";
            case CodeGenObjectType.GenAsText:
                return "Text";
            case CodeGenObjectType.GenAsInputField:
                return "InputField";
            case CodeGenObjectType.GenAsImage:
                return "Image";
        }

        return "Unknown";
    }
}
