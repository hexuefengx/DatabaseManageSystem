using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Study.ORM.Core;

namespace DBMS.Model.TableModel
{
    public class DataBaseModel:IEntity<string>
    {
        public string Type { get; set; }
        public string Conn { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string CreateDate { get; set; }

        public bool IsTransient()
        {
            throw new NotImplementedException();
        }
    }
}
