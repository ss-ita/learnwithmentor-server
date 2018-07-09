﻿using System;
using System.Collections.Generic;
using System.Linq;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;
using System.Drawing;
using System.IO;
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
        public List<PlanDTO> GetSomeAmount(int prevAmount, int amount)
        {
            var somePlans = db.Plans.GetSomePlans(prevAmount, amount);
            if (!somePlans.Any())
                return null;
            List<PlanDTO> dtosList = new List<PlanDTO>();
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
                                         db.PlanTasks.GetTaskSectionIdInPlan(task.Id, planId),
                                         db.PlanTasks.GetIdByTaskAndPlan(task.Id, planId));
                dtosList.Add(toAdd);
            }
            return dtosList;
        }

        public List<SectionDTO> GetTasksForPlan(int planId)
        {
            var plan = db.Plans.Get(planId);
            if (plan == null)
                return null;
            var section = db.PlanTasks.GetAll().Where(pt => pt.Plan_Id == plan.Id).GroupBy(s => s.Sections).Select(p => new { Name = p.Key.Name, Tasks = p.Key.PlanTasks.Select(pt => pt.Tasks) }).ToList();

            List<SectionDTO> sectionDTOs = new List<SectionDTO>();

            foreach (var sec in section)
            {
                List<TaskDTO> taskDTOs = new List<TaskDTO>();
                foreach (var task in sec.Tasks)
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
                                         db.PlanTasks.GetTaskSectionIdInPlan(task.Id, planId),
                                         db.PlanTasks.GetIdByTaskAndPlan(task.Id, planId));
                    taskDTOs.Add(toAdd);
                }
                SectionDTO sectionDTO = new SectionDTO()
                {
                    Name = sec.Name,
                    Tasks = taskDTOs
                };
            sectionDTOs.Add(sectionDTO);
            }
            return sectionDTOs;
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

        public bool SetImage(int id, byte[] image, string imageName)
        {
            Plan toUpdate = db.Plans.Get(id);
            if (toUpdate == null)
                return false;
            string converted = Convert.ToBase64String(image);
            toUpdate.Image = converted;
            toUpdate.Image_Name = imageName;
            db.Save();
            return true;
        }

        public ImageDTO GetImage(int id)
        {
            Plan toGetImage = db.Plans.Get(id);
            if (toGetImage == null || toGetImage.Image == null || toGetImage.Image_Name == null)
                return null;
            return new ImageDTO()
            {
                Name = toGetImage.Image_Name,
                Base64Data = toGetImage.Image
            };            
        }

        public bool Add(PlanDTO dto)
        {
            if (!ContainsId(dto.CreatorId))
                return false;
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
        public List<PlanDTO> Search(string[] searchString)
        {
            var result = db.Plans.Search(searchString);
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
