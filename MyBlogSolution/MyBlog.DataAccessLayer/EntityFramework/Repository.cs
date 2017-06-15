using Blog.Common;
using MyBlog.Core.DataAccess;
using MyBlog.DataAccessLayer;
using MyBlog.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DataAccessLayer.EntityFramework
{
    public class Repository<T> :RepositoryBase,IDataAccess<T> where T:class
    {

        private DbSet<T> _objectSet;
        public Repository()
        {
            _objectSet = db.Set<T>();
        }
        public List<T> List()
        {
            return _objectSet.ToList();
        }

        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }

        public List<T> List(Expression<Func<T,bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);
            if(obj is MyEntitiyBase)
            {
                MyEntitiyBase o = obj as MyEntitiyBase;
                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifyDate = now;
                o.ModifiedUser = App.Common.GetUsername();
            }

            return Save();

        }

        public int Uptade(T obj)
        {
            if (obj is MyEntitiyBase)
            {
                MyEntitiyBase o = obj as MyEntitiyBase;

                o.ModifyDate = DateTime.Now; ;
                o.ModifiedUser = App.Common.GetUsername();
            }

            return Save();
        }

        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }
        public int Save()
        {
           return db.SaveChanges();
        }
        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }
    }
}
