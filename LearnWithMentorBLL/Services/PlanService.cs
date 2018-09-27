using System;
using System.Collections.Generic;
using System.Linq;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.UnitOfWork;
using ThreadTask = System.Threading.Tasks;
using System.Threading.Tasks;


namespace LearnWithMentorBLL.Services
{
    public class PlanService : BaseService, IPlanService
    {
        public PlanService(IUnitOfWork db) : base(db)
        {
        }
        public async Task<PlanDto> GetAsync(int id)
        {
            var plan = await db.Plans.Get(id);
            if (plan == null)
            {
                return null;
            }
            return new PlanDto(plan.Id,
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
        public List<PlanDto> GetAll()
        {
            var allPlans = db.Plans.GetAll();
            if (allPlans == null)
            {
                return null;
            }
            var dtosList = new List<PlanDto>();
            foreach (var plan in allPlans)
            {
                dtosList.Add(new PlanDto(plan.Id,
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
        public List<PlanDto> GetSomeAmount(int prevAmount, int amount)
        {
            var somePlans = db.Plans.GetSomePlans(prevAmount, amount);
            if (somePlans == null)
            {
                return null;
            }
            var dtosList = new List<PlanDto>();
            foreach (var plan in somePlans)
            {
                dtosList.Add(new PlanDto(plan.Id,
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

        public async ThreadTask.Task<List<TaskDto>> GetAllTasksAsync(int planId)
        {
            var plan = await db.Plans.Get(planId);
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
            var dtosList = new List<TaskDto>();
            foreach (var task in tasksForConcretePlan)
            {
                var toAdd = new TaskDto(task.Id,
                                         task.Name,
                                         task.Description,
                                         task.Private,
                                         task.Create_Id,
                                         await db.Users.ExtractFullNameAsync(task.Create_Id),
                                         task.Mod_Id,
                                         await db.Users.ExtractFullNameAsync(task.Mod_Id),
                                         task.Create_Date,
                                         task.Mod_Date,
                                         await db.PlanTasks.GetTaskPriorityInPlanAsync(task.Id, planId),
                                         await db.PlanTasks.GetTaskSectionIdInPlanAsync(task.Id, planId),
                                         await db.PlanTasks.GetIdByTaskAndPlanAsync(task.Id, planId));
                dtosList.Add(toAdd);
            }
            return dtosList;
        }

        public async ThreadTask.Task<List<int>> GetAllPlanTaskidsAsync(int planId)
        {
            var plan = await db.Plans.Get(planId);
            if (plan == null)
            {
                return null;
            }
            var planTaskIds = db.PlanTasks.GetAll()
                .Where(pt => pt.Plan_Id == planId)
                .Select(pt => pt.Id).ToList();
            if (!planTaskIds.Any())
            {
                return null;
            }
            return planTaskIds;
        }

        public async ThreadTask.Task<List<SectionDto>> GetTasksForPlanAsync(int planId)
        {
            var plan = await db.Plans.Get(planId);
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

            List<SectionDto> sectionDTOs = new List<SectionDto>();

            foreach (var sec in section)
            {
                List<TaskDto> taskDTOs = new List<TaskDto>();
                ContentDto contentDTO = new ContentDto();
                foreach (var task in sec.Tasks)
                {
                    var toAdd = new TaskDto(task.Id,
                        task.Name,
                        task.Description,
                        task.Private,
                        task.Create_Id,
                        await db.Users.ExtractFullNameAsync(task.Create_Id),
                        task.Mod_Id,
                        await db.Users.ExtractFullNameAsync(task.Mod_Id),
                        task.Create_Date,
                        task.Mod_Date,
                        await db.PlanTasks.GetTaskPriorityInPlanAsync(task.Id, planId),
                        await db.PlanTasks.GetTaskSectionIdInPlanAsync(task.Id, planId),
                        await db.PlanTasks.GetIdByTaskAndPlanAsync(task.Id, planId));
                    taskDTOs.Add(toAdd);
                }
                contentDTO.Tasks = taskDTOs;
                SectionDto sectionDTO = new SectionDto()
                {
                    Id = sec.Id,
                    Name = sec.Name,
                    Content = contentDTO
                };
                sectionDTOs.Add(sectionDTO);
            }
            return sectionDTOs;
        }

        public async ThreadTask.Task<bool> UpdateByIdAsync(PlanDto plan, int id)
        {
            var toUpdate = await db.Plans.Get(id);
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

        private async ThreadTask.Task CreateUserTasksForAllLearningByPlanAsync(int planId, int taskId)
        {
            var planTaskId = await db.PlanTasks.GetIdByTaskAndPlanAsync(taskId, planId);
            var plan = await db.Plans.Get(planId);
            var groups = await db.Groups.GetGroupsByPlanAsync(planId);
            if (plan == null ||groups.Any() || planTaskId == null)
            {
                return;
            }
            foreach (var group in groups)
            {
                foreach (var user in group.Users)
                {
                    if (db.UserTasks.  GetByPlanTaskForUserAsync(planTaskId.Value, user.Id) == null)
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

        public async ThreadTask.Task<bool> AddTaskToPlanAsync(int planId, int taskId, int? sectionId, int? priority)
        {
            var plan = await db.Plans.Get(planId);
            if (plan == null)
            {
                return false;
            }
            LearnWithMentorDAL.Entities.Task task = await db.Tasks.GetAsync(taskId);
            if (task == null)
            {
                return false;
            }
            await db.Plans.AddTaskToPlanAsync(planId, taskId, sectionId, priority);
            await CreateUserTasksForAllLearningByPlanAsync(planId, taskId);
            db.Save();
            return true;
        }

        public async ThreadTask.Task<bool> SetImageAsync(int id, byte[] image, string imageName)
        {
            var toUpdate = await db.Plans.Get(id);
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

        public async ThreadTask.Task<ImageDto> GetImageAsync(int id)
        {
            var toGetImage = await db.Plans.Get(id);
            if (toGetImage?.Image == null || toGetImage.Image_Name == null)
            {
                return null;
            }
            return new ImageDto()
            {
                Name = toGetImage.Image_Name,
                Base64Data = toGetImage.Image
            };
        }

        public async ThreadTask.Task<bool> AddAsync(PlanDto dto)
        {
            if (! await ContainsId(dto.CreatorId))
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
        public async ThreadTask.Task<int?> AddAndGetIdAsync(PlanDto dto)
        {
            if (!(await db.Users.ContainsIdAsync(dto.CreatorId)))
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

        public List<PlanDto> Search(string[] searchString)
        {
            var result = db.Plans.Search(searchString);
            if (result == null)
            {
                return null;
            }
            var dtosList = new List<PlanDto>();
            foreach (var plan in result)
            {
                dtosList.Add(new PlanDto(plan.Id,
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

        public async ThreadTask.Task<string> GetInfoAsync(int groupid, int planid)
        {
            Group group = await db.Groups.GetAsync(groupid);
            if (group == null)
            {
                return null;
            }
            var plan = await db.Plans.Get(planid);
            if (plan == null)
            {
                return null;
            }

            return group.Name + ": " + plan.Name;
        }

        public ThreadTask.Task<bool> ContainsId(int id)
        {
            return db.Plans.ContainsId(id);
        }
    }
}
