using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Study.ORM.Core;

namespace DBMS.Repository
{
    /// <summary>
    /// 数据库访问上下文
    /// </summary>
    public class SystemBaseDbContext:DefaultDbContext
    {
        public SystemBaseDbContext(string dbName) : base(dbName)
        {
            
        }
    }
}
