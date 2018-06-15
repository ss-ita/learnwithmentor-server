using System;
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

        public IEnumerable<TaskDTO> GetAllTasksForPlan(int taskId, int planId)
        {
            throw new NotImplementedException();
        }
        public TaskDTO GetTaskCommentsForPlan(int taskId, int planId)
        {
            throw new NotImplementedException();
        }
        public void RemoveTaskById(int id)
        {
            throw new NotImplementedException();
        }
        public void DeleteTaskById(int id)
        {

        }


        public IEnumerable<TaskDTO> Search(string[] str, int planId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskDTO> Search(string[] str)
        {
            throw new NotImplementedException();
        }

        IEnumerable<CommentDTO> ITaskService.GetTaskCommentsForPlan(int taskId, int planId)
        {
            throw new NotImplementedException();
        }
    }
}
