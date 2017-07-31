using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;

namespace DBMS.Repository.Core
{
    /// <summary>
    /// 数据库操作基类(此类中只维护公共方法，需要特定方法，在派生类中添加)
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public class DapperRepositoryBase<TDbContext, TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TDbContext : DbContext where TEntity : class, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        public IDbConnection DbConnection
        {
            get { return DbContext.Database.Connection; }
        }

        /// <summary>
        /// 数据库上下文
        /// </summary>
        public DbContext DbContext { get; }

        /// <summary>
        /// 当前数据库上下文中的事物
        /// </summary>
        public virtual IDbTransaction ActiveTransaction
        {
            get
            {
                if (DbContext.Database.CurrentTransaction == null)
                {
                    return null;
                }
                return DbContext.Database.CurrentTransaction.UnderlyingTransaction;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public DapperRepositoryBase(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// 公用方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected virtual T Invoke<T>(Func<IDbConnection, T> func)
        {
            var conn = DbConnection;
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                return func(conn);
            }
            finally
            {
                if (ActiveTransaction == null && conn != null)
                    conn.Close();
            }
        }
        /// <summary>
        /// 获取第一个对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity change = default(TEntity);
            Invoke((conn) => change = conn.GetList<TEntity>(predicate, transaction: ActiveTransaction).FirstOrDefault());
            return change;
        }

        /// <summary>
        /// 根据主键查找
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public TEntity FindById(TPrimaryKey primaryKey)
        {
            var predicate = CreateEqualityExpressionForId(primaryKey);
            TEntity change = default(TEntity);
            Invoke((conn) => change = conn.GetList<TEntity>(predicate, transaction: ActiveTransaction).FirstOrDefault());
            return change;
        }

        /// <summary>
        /// linq查找对应所有数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> change = default(IEnumerable<TEntity>);
            Invoke((conn) => change = conn.GetList<TEntity>(predicate, transaction: ActiveTransaction));
            return change;
        }

        /// <summary>
        /// 不带条件的分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindByPage(int pageIndex, int pageSize, IDictionary<string, bool> orders = null)
        {
            List<ISort> sortList = null;
            foreach (var item in orders)
            {
                sortList.Add(new Sort { Ascending = item.Value, PropertyName = item.Key });
            }
            IEnumerable<TEntity> change = default(IEnumerable<TEntity>);
            Invoke((conn) => change = conn.GetPage<TEntity>(null, sort: sortList, transaction: ActiveTransaction, page: pageIndex, resultsPerPage: pageSize));
            return change;
        }
        /// <summary>
        /// lamda表达式分页
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orders">Ascending:true,Desc:false</param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindByPage(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, IDictionary<string, bool> orders = null)
        {
            List<ISort> sortList = null;
            foreach (var item in orders)
            {
                sortList.Add(new Sort { Ascending = item.Value, PropertyName = item.Key });
            }
            IEnumerable<TEntity> change = default(IEnumerable<TEntity>);
            Invoke((conn) => change = conn.GetPage<TEntity>(predicate, sort: sortList, transaction: ActiveTransaction, page: pageIndex, resultsPerPage: pageSize));
            return change;
        }

        /// <summary>
        /// 查找数量
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            int primaryKey = default(int);
            Invoke((conn) => primaryKey = conn.GetList<TEntity>(predicate, transaction: ActiveTransaction).Count());
            return primaryKey;
        }

        /// <summary>
        /// 插入TEntity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TPrimaryKey Insert(TEntity entity)
        {
            TPrimaryKey primaryKey = default(TPrimaryKey);
            Invoke((conn) => primaryKey = conn.Insert(entity, transaction: ActiveTransaction));
            return primaryKey;
        }

        /// <summary>
        /// 更新TEntity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(TEntity entity)
        {
            bool change = default(bool);
            Invoke((conn) => change = conn.Update(entity, transaction: ActiveTransaction));
            return change;
        }
        /// <summary>
        /// 执行查询sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> ExcuteQuery(string sql)
        {
            //todo 过滤更新语句
            IEnumerable<TEntity> change = default(IEnumerable<TEntity>);
            Invoke((conn) => change = conn.Query<TEntity>(sql, transaction: ActiveTransaction));
            return change;
        }

        /// <summary>
        /// 执行非查询sql（更新，存储过程）
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> ExcuteNonQuery(string sql)
        {
            //todo 过滤Query语句
            IEnumerable<TEntity> change = default(IEnumerable<TEntity>);
            Invoke((conn) => change = conn.Query<TEntity>(sql, transaction: ActiveTransaction));
            return change;
        }

        /// <summary>
        /// 返回动态类型
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object DynamicExcuteQuery(string sql)
        {
            object change = default(object);
            Invoke((conn) => change = conn.Query(sql, transaction: ActiveTransaction));
            return change;
        }
        /// <summary>
        /// 生产表达式
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            ParameterExpression lambdaParam = Expression.Parameter(typeof(TEntity));

            BinaryExpression lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
            );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        /// <summary>
        /// 查询语句过滤
        /// </summary>
        /// <returns></returns>
        private static string QuerySqlFilter()
        {
            return string.Empty;
        }

        /// <summary>
        /// 修改语句过滤
        /// </summary>
        /// <returns></returns>
        private static string ChangeSqlFilter()
        {
            return string.Empty;
        }

    }
}
