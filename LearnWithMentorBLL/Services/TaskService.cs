using System;
using System.Linq;
using System.Data.Entity;
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
                                null);   
        }

        public TaskDTO GetTaskForPlan(int taskId, int planId)
        {
            Task t = db.Tasks.Get(taskId);
            if (t == null)
                throw new ValidationException($"Task with ID:{taskId} does not exist.", "");
            if(!db.PlanTasks.ContainsTaskInPlan(taskId, planId))
                throw new ValidationException($"Task(ID:{taskId}) does not exist in plan(ID:{planId}).", "");
            var dto = new TaskDTO(t.Id,
                                    t.Name,
                                    t.Description,
                                    t.Private,
                                    t.Create_Id,
                                    db.Users.ExtractFullName(t.Create_Id),
                                    t.Mod_Id,
                                    db.Users.ExtractFullName(t.Mod_Id),
                                    t.Create_Date,
                                    t.Mod_Date,
                                    db.PlanTasks.GetTaskPriorityInPlan(taskId,planId),
                                    db.PlanTasks.GetTaskSectionIdInPlan(taskId, planId));
            return dto;
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
                                    t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == planId).FirstOrDefault()?.Section_Id));
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
                                    null));
            }
            return dto;
        }

        public bool CreateTask(TaskDTO taskDTO)
        {
            Task t = new Task()
            {
                Id = taskDTO.Id,
                Name = taskDTO.Name,
                Description = taskDTO.Description,
                Private = taskDTO.Private,
                Create_Id = taskDTO.CreatorId,
                Mod_Id = taskDTO.ModifierId
            };
            db.Tasks.Add(t);
            db.Save();
            return true;
        }

        public bool CreateUserTask(UserTaskDTO utDTO)
        {
            if (!db.PlanTasks.ContainsTaskInPlan(utDTO.TaskId, utDTO.PlanId))
                throw new ValidationException($"No task [ID:{utDTO.TaskId}] in plan [ID:{utDTO.PlanId}]", "");
            if(db.Users.Get(utDTO.UserId) == null)
                throw new ValidationException($"No user [ID:{utDTO.UserId}] in db", "");
            UserTask t = new UserTask()
            {
                User_Id = utDTO.UserId,
                PlanTask_Id = db.PlanTasks.GetIdByTaskAndPlan(utDTO.TaskId, utDTO.PlanId).Value,
                State = utDTO.State,
                End_Date = utDTO.EndDate,
                Result = utDTO.Result,
                Propose_End_Date = utDTO.ProposeEndDate,
                //todo mentor auto-setting logic by planId
                Mentor_Id = utDTO.MentorId
            };
            db.UserTasks.Add(t);
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
                UserTask userTask = db.UserTasks.Get(planTaskId, userId);
                if (userTask != null)
                    dtosList.Add(new UserTaskStateDTO(planTaskId, userTask.State));
            }
            return dtosList;
        }

        public UserTaskDTO GetUserTaskByUserTaskPlanIds(int userId, int taskId, int planId)
        {
            int? planTaskId = db.PlanTasks.GetIdByTaskAndPlan(taskId, planId);
            if (planTaskId==null)
                throw new ValidationException($"Task(ID:{taskId}) does not exist in plan(ID:{planId}).", "");
            UserTask ut= db.UserTasks.Get(planTaskId.Value, userId);
            if (ut == null)
                throw new ValidationException($"Users task for this plan does not exist.", "");
            var dto = new UserTaskDTO(ut.Id,
                                      userId,
                                      planId,
                                      taskId,
                                      ut.End_Date,
                                      ut.Propose_End_Date,
                                      ut.Mentor_Id,
                                      ut.State,
                                      ut.Result);
            return dto;
        }

        public bool UpdateUserTaskStatus(int userId, int taskId, int planId, string newStatus)
        {
            var ptId= db.PlanTasks.GetIdByTaskAndPlan(taskId, planId);
            if (ptId == null)
                throw new ValidationException("No task in plan", "");
            var ut= db.UserTasks.Get(ptId.Value, userId);
            if(ut==null)
                throw new ValidationException("No task in plan for this user", "");
            ut.State = newStatus;
            db.UserTasks.Update(ut);
            db.Save();
            return true;
        }

        public IEnumerable<TaskDTO> GetAllTasksForPlan(int taskId, int planId)
        {
            //todo if needed
            throw new NotImplementedException();
        }
    }
}
