using System;
using System.Collections.Generic;
using System.Linq;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.UnitOfWork;

namespace LearnWithMentorBLL.Services
{
    public class PlanService : BaseService, IPlanService
    {
        public PlanService(IUnitOfWork db) : base(db)
        {
        }
        public PlanDTO Get(int id)
        {
            var plan = db.Plans.Get(id);
            if (plan == null)
            {
                return null;
            }
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
            if (allPlans == null)
            {
                return null;
            }
            var dtosList = new List<PlanDTO>();
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
        public List<PlanDTO> GetSomeAmount(int prevAmount, int amount)
        {
            var somePlans = db.Plans.GetSomePlans(prevAmount, amount);
            if (somePlans == null)
            {
                return null;
            }
            var dtosList = new List<PlanDTO>();
            foreach(var plan in somePlans)
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
            {
                return null;
            }
            var planTaskIds = db.PlanTasks.GetAll()
                .Where(pt => pt.Plan_Id == plan.Id)
                .Select(pt => pt.Task_Id).ToList();
            var tasksForConcretePlan = db.Tasks.GetAll()
                .Where(t => planTaskIds.Contains(t.Id))
                .ToList();
            if (!tasksForConcretePlan.Any())
            {
                return null;
            }
            var dtosList = new List<TaskDTO>();
            foreach (var task in tasksForConcretePlan)
            {
                var toAdd = new TaskDTO(task.Id,
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
                                         db.PlanTasks.GetTaskSectionIdInPlan(task.Id, planId),
                                         db.PlanTasks.GetIdByTaskAndPlan(task.Id, planId));
                dtosList.Add(toAdd);
            }
            return dtosList;
        }

        public List<int> GetAllPlanTaskids(int planId)
        {
            var plan = db.Plans.Get(planId);
            if (plan == null)
            {
                return null;
            }
            var planTaskIds =   db.PlanTasks.GetAll()
                .Where(pt => pt.Plan_Id == planId)
                .Select(pt => pt.Id).ToList();
            if (!planTaskIds.Any())
            {
                return null;
            }
            return planTaskIds;
        }

        public List<SectionDTO> GetTasksForPlan(int planId)
        {
            var plan = db.Plans.Get(planId);
            if (plan == null)
            {
                return null;
            }

            var section = db.PlanTasks.GetAll()
                .Where(pt => pt.Plan_Id == planId)
                .GroupBy(s => s.Sections)
                .Select(p => new
                {
                    Id = p.Key.Id,
                    Name = p.Key.Name,
                    Tasks = p.Key.PlanTasks
                        .Where(pt => pt.Plan_Id == planId)
                        .Select(pt => pt.Tasks)
                }).ToList();

            List<SectionDTO> sectionDTOs = new List<SectionDTO>();

            foreach (var sec in section)
            {
                List<TaskDTO> taskDTOs = new List<TaskDTO>();
                ContentDTO contentDTO = new ContentDTO();
                foreach (var task in sec.Tasks)
                {
                    var toAdd = new TaskDTO(task.Id,
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
                        db.PlanTasks.GetTaskSectionIdInPlan(task.Id, planId),
                        db.PlanTasks.GetIdByTaskAndPlan(task.Id, planId));
                    taskDTOs.Add(toAdd);
                }
                contentDTO.Tasks = taskDTOs;
                SectionDTO sectionDTO = new SectionDTO()
                {
                    Id = sec.Id,
                    Name = sec.Name,
                    Content = contentDTO
                };
                sectionDTOs.Add(sectionDTO);
            }
            return sectionDTOs;
        }

        public bool UpdateById(PlanDTO plan, int id)
        {
            var toUpdate = db.Plans.Get(id);
            if (toUpdate == null)
            {
                return false;
            }
            var modified = false;
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
            toUpdate.Published = plan.Published;
            db.Plans.Update(toUpdate);
            db.Save();
            return modified;
        }

        private void CreateUserTasksForAllLearningByPlan(int planId, int taskId)
        {
            var planTaskId = db.PlanTasks.GetIdByTaskAndPlan(taskId, planId);
            var plan = db.Plans.Get(planId);
            var groups = db.Groups.GetGroupsByPlan(planId).ToList();
            if (plan == null || groups.Any() || planTaskId == null)
            {
                return;
            }
            foreach(var group in groups)
            {
                foreach (var user in group.Users)
                {
                    if(db.UserTasks.GetByPlanTaskForUser(planTaskId.Value, user.Id) == null)
                    {
                        if (group.Mentor_Id == null)
                        {
                            continue;
                        }
                        var toInsert = new UserTask()
                        {
                            User_Id = user.Id,
                            PlanTask_Id = planTaskId.Value,
                            State = "P",
                            Mentor_Id = group.Mentor_Id.Value,
                            Result = ""
                        };
                        db.UserTasks.Add(toInsert);
                    }
                }
            }
            
        }

        public bool AddTaskToPlan(int planId, int taskId, int? sectionId, int? priority)
        {
            var plan = db.Plans.Get(planId);
            if (plan == null)
            {
                return false;
            }
            var task = db.Tasks.Get(taskId);
            if (task == null)
            {
                return false;
            }
            db.Plans.AddTaskToPlan(planId, taskId, sectionId, priority);
            CreateUserTasksForAllLearningByPlan(planId, taskId);
            db.Save();             
            return true;
        }

        public bool SetImage(int id, byte[] image, string imageName)
        {
            var toUpdate = db.Plans.Get(id);
            if (toUpdate == null)
            {
                return false;
            }
            var converted = Convert.ToBase64String(image);
            toUpdate.Image = converted;
            toUpdate.Image_Name = imageName;
            db.Save();
            return true;
        }

        public ImageDTO GetImage(int id)
        {
            var toGetImage = db.Plans.Get(id);
            if (toGetImage?.Image == null || toGetImage.Image_Name == null)
            {
                return null;
            }
            return new ImageDTO()
            {
                Name = toGetImage.Image_Name,
                Base64Data = toGetImage.Image
            };            
        }

        public bool Add(PlanDTO dto)
        {
            if (!ContainsId(dto.CreatorId))
            {
                return false;
            }
            var plan = new Plan
            {
                Name = dto.Name,
                Description = dto.Description,
                Create_Id = dto.CreatorId,
                Published = dto.Published
            };
            db.Plans.Add(plan);
            db.Save();
            return true;
        }
        public int? AddAndGetId(PlanDTO dto)
        {
            if (!db.Users.ContainsId(dto.CreatorId))
            {
                return null;
            }
            var plan = new Plan
            {
                Name = dto.Name,
                Description = dto.Description,
                Create_Id = dto.CreatorId,
                Published = dto.Published
            };
            var createdPlan = db.Plans.AddAndReturnElement(plan);
            db.Save();
            return createdPlan?.Id;
        }
        public List<PlanDTO> Search(string[] searchString)
        {
            var result = db.Plans.Search(searchString);
            if (result == null)
            {
                return null;
            }
            var dtosList = new List<PlanDTO>();
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

        public string GetInfo(int groupid, int planid)
        {
            var group = db.Groups.Get(groupid);
            if (group == null)
            {
                return null;
            }
            var plan = db.Plans.Get(planid);
            if (plan == null)
            {
                return null;
            }

            return group.Name + ": " + plan.Name;
        }
       public bool ContainsId(int id)
        {
            return db.Plans.ContainsId(id);
        }
    }
}
