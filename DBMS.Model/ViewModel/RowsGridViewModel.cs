using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBMS.Model.ViewModel
{
    public class RowsGridViewModel
    {
        public int Total { get; set; }
        public IEnumerable<RowsViewModel> Rows { get; set; }

    }
}