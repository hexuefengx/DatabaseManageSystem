using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBMS.Model.ViewModel
{
    public class ColumnsViewModel
    {
        public string name { get; set; }

        public string rowType { get; set; }

        public int isnullable { get; set; }

        public int length { get; set; }
    }
}