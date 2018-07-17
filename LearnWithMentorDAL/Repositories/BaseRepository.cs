using System.Collections.Generic;
using LearnWithMentorDAL.Entities;
using System.Data.Entity;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class BaseRepository<T>: IRepository<T> where T : class
    {
        protected readonly LearnWithMentor_DBEntities Context;
        public BaseRepository(LearnWithMentor_DBEntities context)
        {
            Context = context;
        }
        public IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }
        public void Add(T item)
        {
            Context.Set<T>().Add(item);
        }
        public void Update(T item)
        {
            Context.Entry(item).State = EntityState.Modified;
        }
        public void Remove(T item)
        {
            Context.Set<T>().Remove(item);
        }
    }
}
