using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDAL.UnitOfWork;
using LearnWithMentorDTO.Infrastructure;
using System;

namespace LearnWithMentorBLL.Services
{
    public class TaskService : BaseService, ITaskService
    {
        public TaskService(IUnitOfWork db) : base(db)
        {
        }
          
        public IEnumerable<TaskDTO> GetAllTasks()
        {
            var taskDTO = new List<TaskDTO>();
            var tasks = db.Tasks.GetAll();
            if (tasks == null)
            {
                return null;
            }
            foreach (var t in tasks)
            {
                taskDTO.Add(TaskToTaskDTO(t));
            }
            return taskDTO;
        }

        public TaskDTO GetTaskById(int taskId)
        {
            var taks = db.Tasks.Get(taskId);
            if (taks == null)
            {
                return null;
            }
            return TaskToTaskDTO(taks);
        }

        public int? AddAndGetId(TaskDTO taskDTO)
        {
            if (!db.Users.ContainsId(taskDTO.CreatorId))
            {
                return null;
            }
            var task = new Task
            {
                Name = taskDTO.Name,
                Description = taskDTO.Description,
                Private = taskDTO.Private,
                Create_Id = taskDTO.CreatorId,
                Mod_Id = taskDTO.ModifierId
            };
            var createdTask = db.Tasks.AddAndReturnElement(task);
            db.Save();
            return createdTask?.Id;
        }

        public TaskDTO GetTaskForPlan(int taskId, int planId)
        {
            var task = db.Tasks.Get(taskId);
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
            var planTask = db.PlanTasks.Get(planTaskId);
            if (planTask == null)
            {
                return null;
            }
            var task = planTask.Tasks;
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
                                    planTask.Priority,
                                    planTask.Section_Id,
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
            var taskList = new List<TaskDTO>();
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
                                    task.PlanTasks.FirstOrDefault(pt => pt.Task_Id == task.Id && pt.Plan_Id == planId)?.Priority,
                                    task.PlanTasks.FirstOrDefault(pt => pt.Task_Id == task.Id && pt.Plan_Id == planId)?.Section_Id,
                                    task.PlanTasks.FirstOrDefault(pt => pt.Task_Id == task.Id && pt.Plan_Id == planId)?.Id));
            }
            return taskList;
        }

        public List<TaskDTO> Search(string[] keys)
        {
            var taskList = new List<TaskDTO>();
            foreach ( var t in db.Tasks.Search(keys))
            {
                taskList.Add(TaskToTaskDTO(t));
            }
            return taskList;
        }

        public bool CreateTask(TaskDTO taskDTO)
        {
            var task = new Task()
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
            var userTask = new UserTask()
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

        public bool UpdateProposeEndDate(int userTaskId, DateTime proposeEndDate)
        {
            var userTask = db.UserTasks.Get(userTaskId);
            if (userTask == null) return false;
            userTask.Propose_End_Date = proposeEndDate;
            db.UserTasks.Update(userTask);
            db.Save();
            return true;
        }

        public bool SetNewEndDate(int userTaskId)
        {
            var userTask = db.UserTasks.Get(userTaskId);
            if (userTask == null) return false;
            userTask.End_Date = userTask.Propose_End_Date;
            userTask.Propose_End_Date = null;
            db.UserTasks.Update(userTask);
            db.Save();
            return true;
        }

        public bool DeleteProposeEndDate(int userTaskId)
        {
            var userTask = db.UserTasks.Get(userTaskId);
            if (userTask == null) return false;
            userTask.Propose_End_Date = null;
            db.UserTasks.Update(userTask);
            db.Save();
            return true;
        }

        public bool UpdateTaskById(int taskId, TaskDTO taskDTO)
        {
            var item = db.Tasks.Get(taskId);
            if (item == null)
            {
                return false;
            }
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

        public List<UserTaskDTO> GetTaskStatesForUser(int[] planTaskIds, int userId)
        {
            var dtoList = new List<UserTaskDTO>();
            foreach (int planTaskId in planTaskIds)
            {
                var userTask = db.UserTasks.GetByPlanTaskForUser(planTaskId, userId);
                if (userTask != null)
                {
                    dtoList.Add(new UserTaskDTO(userTask.Id, userTask.User_Id, userTask.PlanTask_Id, userTask.End_Date,
                        userTask.Propose_End_Date, userTask.Mentor_Id, userTask.State, userTask.Result));
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
            var tasksNotUsedInPlanList = new List<TaskDTO>();
            foreach (var task in tasksNotUsedInPlan)
            {
                var taskDto = new TaskDTO
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
            var userTask = db.UserTasks.GetByPlanTaskForUser(planTaskId, userId);
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
        public PagedListDTO<TaskDTO> GetTasks(int pageSize, int pageNumber = 1)
        {
            var query = db.Tasks.GetAll().AsQueryable();
            query = query.OrderBy(x => x.Id);
            return PagedList<Task, TaskDTO>.GetDTO(query, pageNumber, pageSize, TaskToTaskDTO);
        }
        private TaskDTO TaskToTaskDTO(Task task)
        {
            return new TaskDTO(task.Id,
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
        }
        public bool CheckUserTaskOwner(int userTaskId, int userId)
        {
            var userTask = db.UserTasks.Get(userTaskId);
            return userTask.User_Id == userId;
        }
    }
}
