using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    class PresenterTemplate
    {
        public static string ClassName(DataContext ctx)
        {
            return ctx.Name + "Presenter";
        }



        // Presenter中的事件名
        public static string Event(DataContext ctx)
        {
            return "On" + ctx.Name + "Changed";
        }

        public static string Property( DataContext ctx )
        {
            return ctx.Name;
        }

        public static string Command(DataContext ctx)
        {
            return "Cmd_" + ctx.Name;
        }
    }
}
