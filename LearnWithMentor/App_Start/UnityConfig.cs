using LearnWithMentor.Log;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Services;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.UnitOfWork;
using System.Web.Http;
using System.Web.Http.Tracing;
using Unity;
using Unity.Injection;
using Unity.WebApi;

namespace LearnWithMentor
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<ICommentService, CommentService>();
            container.RegisterType<ITraceWriter, LwmLogger>();
            container.RegisterType<IPlanService, PlanService>();
            container.RegisterType<IGroupService, GroupService>();
            container.RegisterType<ITaskService, TaskService>();
            container.RegisterType<IMessageService, MessageService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IUserIdentityService, UserIdentityService>();

            container.RegisterType<IUnitOfWork, UnitOfWork>(new InjectionConstructor(typeof (LearnWithMentor_DBEntities)));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}