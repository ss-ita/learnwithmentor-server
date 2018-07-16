using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IDataBaseService : IDisposableService
    {
        void DbInitialize();
    }
}
