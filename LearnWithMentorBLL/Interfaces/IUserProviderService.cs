using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LearnWithMentorBLL.Interfaces
{
    public interface IUserProviderService:IDisposableService
    {
        System.Security.Principal.IPrincipal User { get; }
    }
}
