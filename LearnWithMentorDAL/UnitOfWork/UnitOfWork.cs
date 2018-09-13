using System;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LearnWithMentor_DBEntities context;
        private bool disposed;
        private ICommentRepository comments;
        private IGroupPlanTaskViewRepository groupPlanTaskView;
        private IGroupRepository groups;
        private IMessageRepository messages;
        private IPlanRepository plans;
        private IPlanSuggestionRepository planSuggestions;
        private IPlanTaskRepository planTasks;
        private IRoleRepository roles;
        private ISectionRepository sections;
        private ITaskRepository tasks;
        private IUserRepository users;
        private IUserRoleViewRepository userRoleView;
        private IUserTaskRepository userTasks;

        public UnitOfWork(LearnWithMentor_DBEntities context)
        {
            this.context = context;
            disposed = false;
        }

        public ICommentRepository Comments => comments ?? (comments = new CommentRepository(context));

        public IGroupPlanTaskViewRepository GroupPlanTaskView => groupPlanTaskView ?? (groupPlanTaskView = new GroupPlanTaskViewRepository(context));

        public IGroupRepository Groups => groups ?? (groups = new GroupRepository(context));

        public IMessageRepository Messages => messages ?? (messages = new MessageRepository(context));

        public IPlanRepository Plans => plans ?? (plans = new PlanRepository(context));

        public IPlanSuggestionRepository PlanSuggestions => planSuggestions ?? (planSuggestions = new PlanSuggestionRepository(context));

        public IPlanTaskRepository PlanTasks => planTasks ?? (planTasks = new PlanTaskRepository(context));

        public IRoleRepository Roles => roles ?? (roles = new RoleRepository(context));

        public ISectionRepository Sections => sections ?? (sections = new SectionRepository(context));

        public ITaskRepository Tasks => tasks ?? (tasks = new TaskRepository(context));

        public virtual IUserRepository Users => users ?? (users = new UserRepository(context));

        public IUserRoleViewRepository UserRoleView => userRoleView ?? (userRoleView = new UserRoleViewRepository(context));

        public IUserTaskRepository UserTasks => userTasks ?? (userTasks = new UserTaskRepository(context));

        public void Save()
        {
            context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
