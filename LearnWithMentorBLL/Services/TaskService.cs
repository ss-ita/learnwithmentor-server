using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDAL.UnitOfWork;

namespace LearnWithMentorBLL.Services
{
    public class TaskService : BaseService, ITaskService
    {
        public TaskService(IUnitOfWork db) : base(db)
        {
        }
          
        public IEnumerable<TaskDTO> GetAllTasks()
        {
            List<TaskDTO> taskDTO = new List<TaskDTO>();
            var tasks = db.Tasks.GetAll();
            if (tasks == null)
            {
                return null;
            }
            foreach (var t in tasks)
            {
                taskDTO.Add(new TaskDTO(t.Id,
                                    t.Name,
                                    t.Description,
                                    t.Private,
                                    t.Create_Id,
                                    db.Users.ExtractFullName(t.Create_Id),
                                    t.Mod_Id,
                                    db.Users.ExtractFullName(t.Mod_Id),
                                    t.Create_Date,
                                    t.Mod_Date,
                                    null,
                                    null,
                                    null));
            }
            return taskDTO;
        }

        public TaskDTO GetTaskById(int taskId)
        {
            Task taks = db.Tasks.Get(taskId);
            if (taks == null)
            {
                return null;
            }
            return new TaskDTO(taks.Id,
                                taks.Name,
                                taks.Description,
                                taks.Private,
                                taks.Create_Id,
                                db.Users.ExtractFullName(taks.Create_Id),
                                taks.Mod_Id,
                                db.Users.ExtractFullName(taks.Mod_Id),
                                taks.Create_Date,
                                taks.Mod_Date,
                                null,
                                null,
                                null);   
        }

        public TaskDTO GetTaskForPlan(int taskId, int planId)
        {
            Task task = db.Tasks.Get(taskId);
            if (task == null)
            {
                return null;
            }
            var planTask = db.PlanTasks.Get(taskId, planId);
            if (planTask == null)
            {
                return null;
            }
            return GetTaskForPlan(planTask.Id);
        }

        public TaskDTO GetTaskForPlan(int planTaskId)
        {
            PlanTask planTask = db.PlanTasks.Get(planTaskId);
            if (planTask == null)
            {
                return null;
            }
            Task task = planTask.Tasks;
            var taskDTO = new TaskDTO(task.Id,
                                    task.Name,
                                    task.Description,
                                    task.Private,
                                    task.Create_Id,
                                    db.Users.ExtractFullName(task.Create_Id),
                                    task.Mod_Id,
                                    db.Users.ExtractFullName(task.Mod_Id),
                                    task.Create_Date,
                                    task.Mod_Date,
                                    planTask?.Priority,
                                    planTask?.Section_Id,
                                    planTask.Id);
            return taskDTO;
        }

        public StatisticsDTO GetUserStatistics(int userId)
        {
            if(!db.Users.ContainsId(userId))
            {
                return null;
            }
            return new StatisticsDTO()
            {
                InProgressNumber = db.UserTasks.GetNumberOfTasksByState(userId, "P"),
                DoneNumber = db.UserTasks.GetNumberOfTasksByState(userId, "D"),
                ApprovedNumber = db.UserTasks.GetNumberOfTasksByState(userId, "A"),
                RejectedNumber = db.UserTasks.GetNumberOfTasksByState(userId, "R")
            };
        }

        public IEnumerable<TaskDTO> Search(string[] keys, int planId)
        {
            if (!db.Plans.ContainsId(planId))
            {
                return null;
            }
            List<TaskDTO> taskList = new List<TaskDTO>();
            foreach (var task in db.Tasks.Search(keys, planId))
            {
                taskList.Add(new TaskDTO(task.Id,
                                    task.Name,
                                    task.Description,
                                    task.Private,
                                    task.Create_Id,
                                    db.Users.ExtractFullName(task.Create_Id),
                                    task.Mod_Id,
                                    db.Users.ExtractFullName(task.Mod_Id),
                                    task.Create_Date,
                                    task.Mod_Date,
                                    task.PlanTasks.Where(pt => pt.Task_Id == task.Id && pt.Plan_Id == planId).FirstOrDefault()?.Priority,
                                    task.PlanTasks.Where(pt => pt.Task_Id == task.Id && pt.Plan_Id == planId).FirstOrDefault()?.Section_Id,
                                    task.PlanTasks.Where(pt => pt.Task_Id == task.Id && pt.Plan_Id == planId).FirstOrDefault()?.Id));
            }
            return taskList;
        }

        public IEnumerable<TaskDTO> Search(string[] keys)
        {
            List<TaskDTO> taskList = new List<TaskDTO>();
            foreach ( var t in db.Tasks.Search(keys))
            {
                taskList.Add(new TaskDTO(t.Id,
                                    t.Name,
                                    t.Description,
                                    t.Private,
                                    t.Create_Id,
                                    db.Users.ExtractFullName(t.Create_Id),
                                    t.Mod_Id,
                                    db.Users.ExtractFullName(t.Mod_Id),
                                    t.Create_Date,
                                    t.Mod_Date,
                                    null,
                                    null,
                                    null));
            }
            return taskList;
        }

        public bool CreateTask(TaskDTO taskDTO)
        {
            Task task = new Task()
            {
                Name = taskDTO.Name,
                Description = taskDTO.Description,
                Private = taskDTO.Private,
                Create_Id = taskDTO.CreatorId,
                Mod_Id = taskDTO.ModifierId
            };
            db.Tasks.Add(task);
            db.Save();
            return true;
        }

        public bool CreateUserTask(UserTaskDTO userTaskDTO)
        {
            var planTask = db.PlanTasks.Get(userTaskDTO.PlanTaskId);
            if (planTask == null)
            {
                return false;
            }
            if (db.Users.Get(userTaskDTO.UserId) == null)
            {
                return false;
            }
            UserTask userTask = new UserTask()
            {
                User_Id = userTaskDTO.UserId,
                PlanTask_Id = userTaskDTO.PlanTaskId,
                State = userTaskDTO.State,
                End_Date = userTaskDTO.EndDate,
                Result = userTaskDTO.Result,
                Propose_End_Date = userTaskDTO.ProposeEndDate,
                Mentor_Id = userTaskDTO.MentorId
            };
            db.UserTasks.Add(userTask);
            db.Save();
            return true;
        }

        public bool UpdateTaskById(int taskId, TaskDTO taskDTO)
        {
            var item = db.Tasks.Get(taskId);
            if (item != null)
            {
                if (!string.IsNullOrEmpty(taskDTO.Name))
                {
                    item.Name = taskDTO.Name;
                }
                if (!string.IsNullOrEmpty(taskDTO.Description))
                {
                    item.Description = taskDTO.Description;
                }
                item.Private = taskDTO.Private;
                if (taskDTO.ModifierId != null)
                {
                    item.Mod_Id = taskDTO.ModifierId;
                }
                db.Tasks.Update(item);
                db.Save();
                return true;
            }
            return false;
        }

        public bool RemoveTaskById(int taskId)
        {
            var item = db.Tasks.Get(taskId);
            if (item != null || db.Tasks.IsRemovable(taskId))
            {
                db.Tasks.Remove(item);
                db.Save();
                return true;
            }
            return false;
        }

        public List<UserTaskStateDTO> GetTaskStatesForUser(int[] planTaskIds, int userId)
        {
            List<UserTaskStateDTO> dtoList = new List<UserTaskStateDTO>();
            foreach (int planTaskId in planTaskIds)
            {
                UserTask userTask = db.UserTasks.GetByPlanTaskForUser(planTaskId, userId);
                if (userTask != null)
                {
                    dtoList.Add(new UserTaskStateDTO(planTaskId, userTask.State));
                }
            }
            return dtoList;
        }

        public IEnumerable<TaskDTO> GetTasksNotInPlan(int planId)
        {
            var plan = db.Plans.Get(planId);
            if (plan == null)
            {
                return null;
            }
            var tasksNotUsedInPlan= db.Tasks.GetTasksNotInPlan(planId);
            if (tasksNotUsedInPlan == null)
            {
                return null;
            }
            List<TaskDTO> tasksNotUsedInPlanList = new List<TaskDTO>();
            foreach (var task in tasksNotUsedInPlan)
            {
                TaskDTO taskDto = new TaskDTO
                (
                    task.Id,
                                task.Name,
                                task.Description,
                                task.Private,
                                task.Create_Id,
                                db.Users.ExtractFullName(task.Create_Id),
                                task.Mod_Id,
                                db.Users.ExtractFullName(task.Mod_Id),
                                task.Create_Date,
                                task.Mod_Date,
                                null,
                                null,
                                null);

                if (!tasksNotUsedInPlanList.Contains(taskDto))
                {
                    tasksNotUsedInPlanList.Add(taskDto);
                }
            }
            return tasksNotUsedInPlanList;
        }
        
        public UserTaskDTO GetUserTaskByUserPlanTaskId(int userId, int planTaskId)
        {
            UserTask userTask = db.UserTasks.GetByPlanTaskForUser(planTaskId, userId);
            if (userTask == null)
            {
                return null;
            }
            var userTaskDto = new UserTaskDTO(userTask.Id,
                                      userTask.User_Id,
                                      userTask.PlanTask_Id,
                                      userTask.End_Date,
                                      userTask.Propose_End_Date,
                                      userTask.Mentor_Id,
                                      userTask.State,
                                      userTask.Result);
            return userTaskDto;
        }

        public bool UpdateUserTaskStatus(int userTaskId, string newStatus)
        {
            if (!Regex.IsMatch(newStatus, ValidationRules.USERTASK_STATE))
            {
                return false;
            }
            var userTask= db.UserTasks.Get(userTaskId);
            if (userTask == null)
            {
                return false;
            }
            userTask.State = newStatus;
            db.UserTasks.Update(userTask);
            db.Save();
            return true;
        }

        public bool UpdateUserTaskResult(int userTaskId, string newResult)
        {
            if (newResult == null)
            {
                return false;
            }
            var userTask = db.UserTasks.Get(userTaskId);
            if (userTask == null)
            {
                return false;
            }
            userTask.Result = newResult;
            db.UserTasks.Update(userTask);
            db.Save();
            return true;
        }

        public bool CheckUserTaskOwner(int userTaskId, int userId)
        {
            var userTask = db.UserTasks.Get(userTaskId);
            return userTask.User_Id == userId;
        }
    }
}
