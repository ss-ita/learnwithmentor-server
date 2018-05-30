using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories;

namespace LearnWithMentorDAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private LearnWithMentor_DBEntities context;
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

        public UnitOfWork(LearnWithMentor_DBEntities _context)
        {
            context = _context;
            disposed = false;
        }

        public ICommentRepository Comments
        {
            get
            {
                if (comments == null)
                {
                    comments = new CommentRepository(context);
                }
                return comments;
            }
        }
        public IGroupPlanTaskViewRepository GroupPlanTaskView
        {
            get
            {
                if (groupPlanTaskView == null)
                {
                    groupPlanTaskView = new GroupPlanTaskViewRepository(context);
                }
                return groupPlanTaskView;
            }
        }
        public IGroupRepository Groups
        {
            get
            {
                if (groups == null)
                {
                    groups = new GroupRepository(context);
                }
                return groups;
            }
        }
        public IMessageRepository Messages
        {
            get
            {
                if (messages == null)
                {
                    messages = new MessageRepository(context);
                }
                return messages;
            }
        }
        public IPlanRepository Plans
        {
            get
            {
                if (plans == null)
                {
                    plans = new PlanRepository(context);
                }
                return plans;
            }
        }
        public IPlanSuggestionRepository PlanSuggestions
        {
            get
            {
                if (planSuggestions == null)
                {
                    planSuggestions = new PlanSuggestionRepository(context);
                }
                return planSuggestions;
            }
        }
        public IPlanTaskRepository PlanTasks
        {
            get
            {
                if (planTasks == null)
                {
                    planTasks = new PlanTaskRepository(context);
                }
                return planTasks;
            }
        }
        public IRoleRepository Roles
        {
            get
            {
                if (roles == null)
                {
                    roles = new RoleRepository(context);
                }
                return roles;
            }
        }
        public ISectionRepository Sections
        {
            get
            {
                if (sections == null)
                {
                    sections = new SectionRepository(context);
                }
                return sections;
            }
        }
        public ITaskRepository Tasks
        {
            get
            {
                if (tasks == null)
                {
                    tasks = new TaskRepository(context);
                }
                return tasks;
            }
        }
        public IUserRepository Users
        {
            get
            {
                if (users == null)
                {
                    users = new UserRepository(context);
                }
                return users;
            }
        }
        public IUserRoleViewRepository UserRoleView
        {
            get
            {
                if (userRoleView == null)
                {
                    userRoleView = new UserRoleViewRepository(context);
                }
                return userRoleView;
            }
        }
        public IUserTaskRepository UserTasks
        {
            get
            {
                if (userTasks == null)
                {
                    userTasks = new UserTaskRepository(context);
                }
                return userTasks;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
