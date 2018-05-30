using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Repositories;

namespace LearnWithMentorDAL
{
    public interface IUnitOfWork : IDisposable
    {
        ICommentRepository Comments { get; }
        IGroupPlanTaskViewRepository GroupPlanTaskView { get; }
        IGroupRepository Groups { get; }
        IMessageRepository Messages { get; }
        IPlanRepository Plans { get; }
        IPlanSuggestionRepository PlanSuggestions { get; }
        IPlanTaskRepository PlanTasks { get; }
        IRoleRepository Roles { get; }
        ISectionRepository Sections { get; }
        ITaskRepository Tasks { get; }
        IUserRepository Users { get; }
        IUserRoleViewRepository UserRoleView { get; }
        IUserTaskRepository UserTasks { get;  }

        void Save();
    }
}
