using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorBLL.Services
{
    public class PlanService : BaseService, IPlanService
    {
        public PlanService() : base()
        {
        }
        public PlanDTO Get(int id)
        {
            Plan plan = db.Plans.Get(id);
            if (plan == null)
                return null;
            return new PlanDTO(plan.Id,
                               plan.Name,
                               plan.Description,
                               plan.Published,
                               plan.Create_Id,
                               plan.Creator.FirstName,
                               plan.Creator.LastName,
                               plan.Mod_Id,
                               plan.Modifier?.FirstName,
                               plan.Modifier?.LastName,
                               plan.Create_Date,
                               plan.Mod_Date);
        }
        public List<PlanDTO> GetAll()
        {
            var allPlans = db.Plans.GetAll();
            if (!allPlans.Any())
                return null;
            List<PlanDTO> dtosList = new List<PlanDTO>();
            foreach (var plan in allPlans)
            {
                dtosList.Add(new PlanDTO(plan.Id,
                               plan.Name,
                               plan.Description,
                               plan.Published,
                               plan.Create_Id,
                               plan.Creator.FirstName,
                               plan.Creator.LastName,
                               plan.Mod_Id,
                               plan.Modifier?.FirstName,
                               plan.Modifier?.LastName,
                               plan.Create_Date,
                               plan.Mod_Date));
            }
            return dtosList;
        }
        public List<TaskDTO> GetAllTasks(int planId)
        {
            var plan = db.Plans.Get(planId);
            if (plan == null)
                return null;
            var planTaskIds = db.PlanTasks.GetAll().Where(pt => pt.Plan_Id == plan.Id).Select(pt => pt.Task_Id).ToList();
            var tasksForConcretePlan = db.Tasks.GetAll().Where(t => planTaskIds.Contains(t.Id));
            if (!tasksForConcretePlan.Any())
                return null;
            List<TaskDTO> dtosList = new List<TaskDTO>();
            foreach (var task in tasksForConcretePlan)
            {
                TaskDTO toAdd = new TaskDTO(task.Id,
                                         task.Name,
                                         task.Description,
                                         task.Private,
                                         task.Create_Id,
                                         db.Users.ExtractFullName(task.Create_Id),
                                         task.Mod_Id,
                                         db.Users.ExtractFullName(task.Mod_Id),
                                         task.Create_Date,
                                         task.Mod_Date,
                                         db.PlanTasks.GetTaskPriorityInPlan(task.Id, planId),
                                         db.PlanTasks.GetTaskSectionIdInPlan(task.Id, planId))
                {
                    PlanTaskId = db.PlanTasks.GetIdByTaskAndPlan(task.Id, plan.Id)
                };
                dtosList.Add(toAdd);
            }
            return dtosList;
        }

        public bool UpdateById(PlanDTO plan, int id)
        {
            var toUpdate = db.Plans.Get(id);
            if (toUpdate == null)
                return false;
            bool modified = false;
            if (!string.IsNullOrEmpty(plan.Name))
            {
                toUpdate.Name = plan.Name;
                modified = true;
            }
            if (plan.Description != null)
            {
                toUpdate.Description = plan.Description;
                modified = true;
            }
            if (plan.Modid != null)
            {
                toUpdate.Mod_Id = plan.Modid;
                modified = true;
            }
            modified = !modified && toUpdate.Published != plan.Published;
            toUpdate.Published = plan.Published;
            db.Plans.Update(toUpdate);
            db.Save();
            return modified;
        }
        public bool Add(PlanDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Description) || !ContainsId(dto.Creatorid))
                return false;
            var plan = new Plan
            {
                Name = dto.Name,
                Description = dto.Description,
                Create_Id = dto.Creatorid,
                Published = dto.Published
            };
            db.Plans.Add(plan);
            db.Save();
            return true;
        }
        public List<PlanDTO> Search(string[] str)
        {
            var result = db.Plans.Search(str);
            if (result == null)
                return null;
            List<PlanDTO> dtosList = new List<PlanDTO>();
            foreach (var plan in result)
            {
                dtosList.Add(new PlanDTO(plan.Id,
                                         plan.Name,
                                         plan.Description,
                                         plan.Published,
                                         plan.Create_Id,
                                         plan.Creator.FirstName,
                                         plan.Creator.LastName,
                                         plan.Mod_Id,
                                         plan.Modifier?.FirstName,
                                         plan.Modifier?.LastName,
                                         plan.Create_Date,
                                         plan.Mod_Date));
            }
            return dtosList;
        }
        public bool ContainsId(int id)
        {
            return db.Plans.ContainsId(id);
        }
    }
}
