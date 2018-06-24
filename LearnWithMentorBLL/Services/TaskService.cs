using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Infrastructure;

namespace LearnWithMentorBLL.Services
{
    public class TaskService : BaseService, ITaskService
    {
        public TaskService() : base()
        {
        }
          
        public IEnumerable<TaskDTO> GetAllTasks()
        {
            List<TaskDTO> dto = new List<TaskDTO>();
            var tasks = db.Tasks.GetAll();
            if (tasks == null)
                return dto;
            foreach (var t in tasks)
            {
                dto.Add(new TaskDTO(t.Id,
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
            return dto;
        }

        public TaskDTO GetTaskById(int id)
        {
            Task t = db.Tasks.Get(id);
            if (t == null)
                return null;
            return new TaskDTO(t.Id,
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
                                null);   
        }

        public TaskDTO GetTaskForPlan(int taskId, int planId)
        {
            Task task = db.Tasks.Get(taskId);
            if (task == null)
                throw new InternalServiceException($"Task with ID:{taskId} does not exist.", "");
            if(!db.PlanTasks.ContainsTaskInPlan(taskId, planId))
                throw new InternalServiceException($"Task(ID:{taskId}) does not exist in plan(ID:{planId}).", "");
            var dto = new TaskDTO(task.Id,
                                    task.Name,
                                    task.Description,
                                    task.Private,
                                    task.Create_Id,
                                    db.Users.ExtractFullName(task.Create_Id),
                                    task.Mod_Id,
                                    db.Users.ExtractFullName(task.Mod_Id),
                                    task.Create_Date,
                                    task.Mod_Date,
                                    db.PlanTasks.GetTaskPriorityInPlan(taskId,planId),
                                    db.PlanTasks.GetTaskSectionIdInPlan(taskId, planId),
                                    db.PlanTasks.GetIdByTaskAndPlan(taskId,planId));
            return dto;
        }

        public TaskDTO GetTaskForPlan(int planTaskId)
        {
            PlanTask planTask = db.PlanTasks.Get(planTaskId);
            if (planTask == null)
                throw new InternalServiceException($"PlanTask with ID:{planTask.Id} does not exist.", "");
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
                                    planTask.Priority,
                                    planTask.Section_Id,
                                    planTask.Id);
            return taskDTO;
        }

        public IEnumerable<TaskDTO> Search(string[] str, int planId)
        {
            List<TaskDTO> dto = new List<TaskDTO>();
            foreach (var t in db.Tasks.Search(str,planId))
            {
                dto.Add(new TaskDTO(t.Id,
                                    t.Name,
                                    t.Description,
                                    t.Private,
                                    t.Create_Id,
                                    db.Users.ExtractFullName(t.Create_Id),
                                    t.Mod_Id,
                                    db.Users.ExtractFullName(t.Mod_Id),
                                    t.Create_Date,
                                    t.Mod_Date,
                                    t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == planId).FirstOrDefault()?.Priority,
                                    t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == planId).FirstOrDefault()?.Section_Id,
                                    t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == planId).FirstOrDefault()?.Id));
            }
            return dto;
        }

        public IEnumerable<TaskDTO> Search(string[] str)
        {
            List<TaskDTO> dto = new List<TaskDTO>();
            foreach ( var t in db.Tasks.Search(str))
            {
                dto.Add(new TaskDTO(t.Id,
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
            return dto;
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
            if (planTask==null)
                throw new InternalServiceException($"No task [ID:{planTask.Task_Id}] in plan [ID:{planTask.Plan_Id}]", "");
            if(db.Users.Get(userTaskDTO.UserId) == null)
                throw new InternalServiceException($"No user [ID:{userTaskDTO.UserId}] in db", "");
            UserTask userTask = new UserTask()
            {
                User_Id = userTaskDTO.UserId,
                PlanTask_Id = userTaskDTO.PlanTaskId,
                State = userTaskDTO.State,
                End_Date = userTaskDTO.EndDate,
                Result = userTaskDTO.Result,
                Propose_End_Date = userTaskDTO.ProposeEndDate,
                //todo mentor auto-setting logic by planId
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
                item.Id = taskDTO.Id;
                item.Name = taskDTO.Name;
                item.Description = taskDTO.Description;
                item.Private = taskDTO.Private;
                item.Create_Id = taskDTO.CreatorId;
                item.Mod_Id = taskDTO.ModifierId;
                db.Tasks.Update(item);
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
                return true;
            }
            return false;
        }
        public List<UserTaskStateDTO> GetTaskStatesForUser(int[] planTaskIds, int userId)
        {
            List<UserTaskStateDTO> dtosList = new List<UserTaskStateDTO>();
            foreach (int planTaskId in planTaskIds)
            {
                UserTask userTask = db.UserTasks.GetByPlanTaskForUser(planTaskId, userId);
                if (userTask != null)
                    dtosList.Add(new UserTaskStateDTO(planTaskId, userTask.State));
            }
            return dtosList;
        }

        public UserTaskDTO GetUserTaskByUserTaskPlanIds(int userId, int taskId, int planId)
        {
            int? planTaskId = db.PlanTasks.GetIdByTaskAndPlan(taskId, planId);
            if (planTaskId==null)
                throw new InternalServiceException($"Task(ID:{taskId}) does not exist in plan(ID:{planId}).", "");
            UserTask ut= db.UserTasks.GetByPlanTaskForUser(planTaskId.Value, userId);
            if (ut == null)
                throw new InternalServiceException($"Users task for this plan does not exist.", "");
            var dto = new UserTaskDTO(ut.Id,
                                      userId,
                                      ut.PlanTask_Id,
                                      ut.End_Date,
                                      ut.Propose_End_Date,
                                      ut.Mentor_Id,
                                      ut.State,
                                      ut.Result);
            return dto;
        }

        public UserTaskDTO GetUserTaskByUserTaskPlanId(int userId, int planTaskId)
        {
            UserTask userTask = db.UserTasks.GetByPlanTaskForUser(planTaskId, userId);
            if (userTask == null)
                throw new InternalServiceException($"Users task for this plan does not exist.", "");
            var userTaskdto = new UserTaskDTO(userTask.Id,
                                      userTask.User_Id,
                                      userTask.PlanTask_Id,
                                      userTask.End_Date,
                                      userTask.Propose_End_Date,
                                      userTask.Mentor_Id,
                                      userTask.State,
                                      userTask.Result);
            return userTaskdto;
        }

        public bool UpdateUserTaskStatus(int userTaskId, string newStatus)
        {
            if (!Regex.IsMatch(newStatus, ValidationRules.USERTASK_STATE))
                throw new InternalServiceException("New Status not valid","");
            var userTask= db.UserTasks.Get(userTaskId);
            if(userTask == null)
                throw new InternalServiceException("No task in plan for this user", "");
            userTask.State = newStatus;
            db.UserTasks.Update(userTask);
            db.Save();
            return true;
        }
        public bool UpdateUserTaskResult(int userTaskId, string newResult)
        {
            var userTask = db.UserTasks.Get(userTaskId);
            if (userTask == null)
                throw new InternalServiceException("No task in plan for this user", "");
            userTask.Result = newResult;
            db.UserTasks.Update(userTask);
            db.Save();
            return true;
        }
    }
}
