using System.Reflection;

namespace Framework
{
    /// <summary>
    /// 一个窗口上的一系列控件
    /// </summary>
    class UIGenControl
    {
        UIGenWindow _window;

        DataContext _binder;

        public UIGenControl(UIGenWindow win, DataContext binder)
        {
            _binder = binder;
            _window = win;
        }

        public WidgetType ObjectType
        {
            get { return _binder.Type; }
        }

        public void PrintDeclareCode(CodeGenerator gen)
        {
            gen.PrintLine(GetVarType(), " _", Name, ";");
        }

        public void PrintAttachCode(CodeGenerator gen)
        {
            var path = ObjectUtility.GetGameObjectPath(_binder.gameObject, _window.Obj);
            gen.PrintLine("_", Name, " = trans.Find(\"", path, "\").GetComponent<", GetVarType(), ">();");
        }


        public bool ButtonCallbackExists
        {
            get
            {
                // 加载游戏的二进制
                var ass = Assembly.LoadFile(@"Library\ScriptAssemblies\Assembly-CSharp.dll");

                // 取到类信息
                var classInfo = ass.GetType(_window.Name);

                if (classInfo == null)
                    return false;

                // 取方法, 查方法是否存在
                var methodInfo = classInfo.GetMethod("", BindingFlags.NonPublic | BindingFlags.Instance);

                return methodInfo != null;
            }
        }

        public void PrintButtonClickImplementCode(CodeGenerator gen)
        {

            var path = ObjectUtility.GetGameObjectPath(_binder.gameObject, _window.Obj);

            gen.PrintLine("// Button @ ", path);
            gen.PrintLine("void ",  "( )");
            gen.PrintLine("{");
            gen.PrintLine();
            gen.PrintLine("}");
            gen.PrintLine();
        }

        // TODO 处理名字不符合函数命名规定的问题
        public string Name
        {
            get { return _binder.gameObject.name; }
        }

        string GetVarType()
        {
            switch (_binder.Type)
            {
                case WidgetType.Button:
                    return "Button";
                case WidgetType.Text:
                    return "Text";
                case WidgetType.InputField:
                    return "InputField";
                case WidgetType.Image:
                    return "Image";
            }

            return "Unknown";
        }
    }

}