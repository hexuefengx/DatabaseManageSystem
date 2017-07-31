using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBMS.Model.TableModel;
using DBMS.Model.ViewModel;
using DBMS.Repository;
using DBMS.Utility;

namespace DBMS.Service
{
    /// <summary>
    /// 数据库文档服务类
    /// </summary>
    public class DBDocumentService
    {
        DbDocumentQueryRepository repository = new DbDocumentQueryRepository();
        public List<DataBaseModel> GetDataBaseList()
        {
            var list = repository.GetDatabaseList();
            return list;
;
        }

        public List<TablesViewModel> GetTableList(string dbName,string tableName="")
        {
            var list = repository.GetTableList(dbName,tableName);
            return list;
        }

        public List<RowsViewModel> GetTableRowList(string dbName,string tableName)
        {
            var list = repository.GetTableRowsList(dbName, tableName);
            return list;
        }

        public List<RowsViewModel> GetRowListPaging(string dbName, string tableName,int pageIndex,int pageSize)
        {
            var list = repository.GetRowListPaging(dbName, tableName,pageSize,pageIndex);
            return list;
        }

        public int GetRowsCount(string dbName, string tableName)
        {
            var count = repository.GetRowsCount(dbName, tableName);
            return count;
        }

        public string GetTableDescription(string dbName, string tableName)
        {
            var desc = repository.GetTableDescription(dbName, tableName);
            return desc;
        }

        public int EditTableDescription(string dbName, string tableName, string description)
        {
            var change = repository.EditTableDescription(dbName, tableName, description);
            return change;
        }

        public int EditRowDescription(string dbName, string tableName, string rowName, string description)
        {
            var change = repository.EditRowDescription(dbName, tableName,rowName,description);
            return change;
        }
    }

}
