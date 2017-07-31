using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.Repository.Core
{
    /// <summary>
    /// 数据库操作接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IRepository<TEntity, TPrimaryKey> where TEntity :
        class, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 查找元素
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression);
        /// <summary>
        /// 主键查询
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        TEntity FindById(TPrimaryKey primaryKey);
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 不带条件的分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindByPage(int pageIndex, int pageSize, IDictionary<string, bool> orders = null);
        /// <summary>
        /// lamda表达式分页
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindByPage(Expression<Func<TEntity, bool>> expression, int pageIndex, int pageSize, IDictionary<string, bool> orders = null);
        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> expression);
        /// <summary>
        /// 插入操作
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TPrimaryKey Insert(TEntity entity);
        /// <summary>
        /// 更新操作
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(TEntity entity);
        /// <summary>
        /// 执行查询sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IEnumerable<TEntity> ExcuteQuery(string sql);
        /// <summary>
        /// 执行非查询sql（更新，存储过程）
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IEnumerable<TEntity> ExcuteNonQuery(string sql);
    }
}
