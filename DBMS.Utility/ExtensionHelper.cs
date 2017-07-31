using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBMS.Utility
{
    public static class ExtensionHelper
    {
        public static bool IsNotNull<T>(this List<T> t)
        {
            return t != null && t.Any();
        } 
    }
}
