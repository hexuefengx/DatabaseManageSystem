using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.Repository.Core
{
    /// <summary>
    /// Entity接口，所有的Entity都要继承本接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 主键值
        /// </summary>
        TPrimaryKey Id { get; set; }

        /// <summary>
        /// 是否需要持久化 Checks if this entity is transient (not persisted to database and it has not an <see cref="Id"/>).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        bool IsTransient();
    }
}
