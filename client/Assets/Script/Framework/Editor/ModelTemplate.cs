using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    class ModelTemplate
    {
        public static string ClassName(DataContext ctx)
        {
            return ctx.Name + "Model";
        }

    }
}
