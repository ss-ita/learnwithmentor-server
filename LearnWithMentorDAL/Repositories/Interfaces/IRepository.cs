using System.Collections.Generic;
using System.Linq;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
   public interface IRepository<T> where T: class
    {
        IQueryable<T> GetAll();
        void Add(T item);
        void Update(T item);
        void Remove(T item);
    }
}
