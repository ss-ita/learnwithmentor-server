using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorBLL.Interfaces;
using System.Web;
using LearnWithMentorDAL.UnitOfWork;

namespace LearnWithMentorBLL.Services
{
    public class HttpContextUserProvider: BaseService,IUserProviderService
    {
        public HttpContextUserProvider(IUnitOfWork db) : base(db)
        {
        }

        IPrincipal IUserProviderService.User => HttpContext.Current.User;
    }
}
