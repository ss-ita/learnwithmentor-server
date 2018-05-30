using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
