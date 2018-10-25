using System.Collections.Generic;
//using System.Linq;
using System.Threading.Tasks;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
   public interface IRepository<T> where T: class
   {
        Task<IEnumerable<T>> GetAll();
        Task AddAsync(T item);
        Task UpdateAsync(T item);
        Task RemoveAsync(T item);
    }
}
