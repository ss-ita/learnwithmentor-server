using System;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.UnitOfWork
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
