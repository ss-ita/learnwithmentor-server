using System.Collections.Generic;

namespace LearnWithMentorDAL.Repositories
{
   public interface IRepository<T> where T: class

    {
        IEnumerable<T> GetAll();
        void Add(T item);
        void Update(T item);
        void Remove(T item);
    }
}
