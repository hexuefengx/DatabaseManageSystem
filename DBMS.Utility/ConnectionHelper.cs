using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace DBMS.Utility
{
    public class ConnectionHelper
    {
        static string configFilePath = HttpContext.Current.Server.MapPath("/App_Data/connectionStrings.config");

        /// <summary>
        /// 获取数据库配置结点
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDataBaseList()
        {
            List<string> list_connStr = new List<string>();
            string loginType = ConfigurationManager.AppSettings["loginType"];
            if (loginType == "1")
            {
                XElement root = XElement.Load(configFilePath);
                list_connStr = root.Elements().Select(n => n.Attribute("name").Value).ToList();
            }
            return list_connStr;
        }
    }
}
