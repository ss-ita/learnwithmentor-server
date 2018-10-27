using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDAL.UnitOfWork;
using LearnWithMentorDTO.Infrastructure;
using System;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Services
{
    public class TaskService : BaseService, ITaskService
    {
        public TaskService(IUnitOfWork db) : base(db)
        {
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            var taskDTO = new List<TaskDto>();
            var tasks = await db.Tasks.GetAll();
            if (tasks == null)
            {
                return null;
            }
            foreach (var t in tasks)
            {
                taskDTO.Add(await TaskToTaskDTOAsync(t));
            }
            return taskDTO;
        }

        public async Task<TaskDto> GetTaskByIdAsync(int taskId)
        {
            StudentTask taks = await db.Tasks.GetAsync(taskId);
            if (taks == null)
            {
                return null;
            }
            return await TaskToTaskDTOAsync(taks);
        }

        public async Task<int?> AddAndGetIdAsync(TaskDto taskDTO)
        {
            if (!(await db.Users.ContainsIdAsync(taskDTO.CreatorId)))
            {
                return null;
            }
            var task = new StudentTask
            {
                Name = taskDTO.Name,
                Description = taskDTO.Description,
                Private = taskDTO.Private,
                Create_Id = taskDTO.CreatorId,
                Mod_Id = taskDTO.ModifierId
            };
            var createdTask =  db.Tasks.AddAndReturnElement(task);
            db.Save();
            return createdTask?.Id;
        }

        public async Task<TaskDto> GetTaskForPlanAsync(int taskId, int planId)
        {
            StudentTask task = await db.Tasks.GetAsync(taskId);
            if (task == null)
            {
                return null;
            }
            var planTask = await db.PlanTasks.Get(taskId, planId);
            if (planTask == null)
            {
                return null;
            }
            return await GetTaskForPlanAsync(planTask.Id);
        }

        public async Task<TaskDto> GetTaskForPlanAsync(int planTaskId)
        {
            var planTask = await db.PlanTasks.Get(planTaskId);
            if (planTask == null)
            {
                return null;
            }
            var task = planTask.Tasks;
            var taskDTO = new TaskDto(task.Id,
                                    task.Name,
                                    task.Description,
                                    task.Private,
                                    task.Create_Id,
                                    await db.Users.ExtractFullNameAsync(task.Create_Id),
                                    task.Mod_Id,
                                    await db.Users.ExtractFullNameAsync(task.Mod_Id),
                                    task.Create_Date,
                                    task.Mod_Date,
                                    planTask.Priority,
                                    planTask.Section_Id,
                                    planTask.Id);
            return taskDTO;
        }

        public async Task<StatisticsDto> GetUserStatisticsAsync(int userId)
        {
            if (!(await db.Users.ContainsIdAsync(userId)))
            {
                return null;
            }
            return new StatisticsDto()
            {
                InProgressNumber = await db.UserTasks.GetNumberOfTasksByStateAsync(userId, "P"),
                DoneNumber = await db.UserTasks.GetNumberOfTasksByStateAsync(userId, "D"),
                ApprovedNumber = await db.UserTasks.GetNumberOfTasksByStateAsync(userId, "A"),
                RejectedNumber = await db.UserTasks.GetNumberOfTasksByStateAsync(userId, "R")
            };
        }

        public async Task<IEnumerable<TaskDto>> SearchAsync(string[] str, int planId)
        {
            if (! await db.Plans.ContainsId(planId))
            {
                return null;
            }
            var taskList = new List<TaskDto>();
            foreach (var task in await db.Tasks.SearchAsync(str, planId))
            {
                taskList.Add(new TaskDto(task.Id,
                                    task.Name,
                                    task.Description,
                                    task.Private,
                                    task.Create_Id,
                                    await db.Users.ExtractFullNameAsync(task.Create_Id),
                                    task.Mod_Id,
                                    await db.Users.ExtractFullNameAsync(task.Mod_Id),
                                    task.Create_Date,
                                    task.Mod_Date,
                                    task.PlanTasks.FirstOrDefault(pt => pt.Task_Id == task.Id && pt.Plan_Id == planId)?.Priority,
                                    task.PlanTasks.FirstOrDefault(pt => pt.Task_Id == task.Id && pt.Plan_Id == planId)?.Section_Id,
                                    task.PlanTasks.FirstOrDefault(pt => pt.Task_Id == task.Id && pt.Plan_Id == planId)?.Id));
            }
            return taskList;
        }

        public async Task<List<TaskDto>> SearchAsync(string[] keys)
        {
            var taskList = new List<TaskDto>();
            foreach (var t in await db.Tasks.SearchAsync(keys))
            {
                taskList.Add(await TaskToTaskDTOAsync(t));
            }
            return taskList;
        }

        public bool CreateTask(TaskDto taskDTO)
        {
            var task = new StudentTask()
            {
                Name = taskDTO.Name,
                Description = taskDTO.Description,
                Private = taskDTO.Private,
                Create_Id = taskDTO.CreatorId,
                Mod_Id = taskDTO.ModifierId
            };
            db.Tasks.AddAsync(task);
            db.Save();
            return true;
        }

        public async Task<bool> CreateUserTaskAsync(UserTaskDto userTaskDTO)
        {
            var planTask = await db.PlanTasks.Get(userTaskDTO.PlanTaskId);
            if (planTask == null)
            {
                return false;
            }
            if (await db.Users.GetAsync(userTaskDTO.UserId) == null)
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
            db.UserTasks.AddAsync(userTask);
            db.Save();
            return true;
        }

        public async Task<bool> UpdateProposeEndDateAsync(int userTaskId, DateTime proposeEndDate)
        {
            UserTask userTask = await db.UserTasks.GetAsync(userTaskId);
            if (userTask == null) return false;
            userTask.Propose_End_Date = proposeEndDate;
            db.UserTasks.UpdateAsync(userTask);
            db.Save();
            return true;
        }

        public async Task<bool> SetNewEndDateAsync(int userTaskId)
        {
            UserTask userTask = await db.UserTasks.GetAsync(userTaskId);
            if (userTask == null) return false;
            userTask.End_Date = userTask.Propose_End_Date;
            userTask.Propose_End_Date = null;
            await db.UserTasks.UpdateAsync(userTask);
            db.Save();
            return true;
        }

        public async Task<bool> DeleteProposeEndDateAsync(int userTaskId)
        {
            UserTask userTask = await db.UserTasks.GetAsync(userTaskId);
            if (userTask == null) return false;
            userTask.Propose_End_Date = null;
            await db.UserTasks.UpdateAsync(userTask);
            db.Save();
            return true;
        }

        public async Task<bool> UpdateTaskByIdAsync(int taskId, TaskDto taskDTO)
        {
            StudentTask item = await db.Tasks.GetAsync(taskId);
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
            await db.Tasks.UpdateAsync(item);
            db.Save();
            return true;
        }

        public async Task<bool> RemoveTaskByIdAsync(int taskId)
        {
            StudentTask item = await db.Tasks.GetAsync(taskId);
            if (item != null || await db.Tasks.IsRemovableAsync(taskId))
            {
                await db.Tasks.RemoveAsync(item);
                db.Save();
                return true;
            }
            return false;
        }

        public async Task<List<UserTaskDto>> GetTaskStatesForUserAsync(int[] planTaskIds, int userId)
        {
            var dtoList = new List<UserTaskDto>();
            foreach (int planTaskId in planTaskIds)
            {
                UserTask userTask = await db.UserTasks.GetByPlanTaskForUserAsync(planTaskId, userId);
                if (userTask != null)
                {
                    dtoList.Add(new UserTaskDto(userTask.Id, userTask.User_Id, userTask.PlanTask_Id, userTask.End_Date,
                        userTask.Propose_End_Date, userTask.Mentor_Id, userTask.State, userTask.Result));
                }
            }
            return dtoList;
        }

        public async Task<IEnumerable<TaskDto>> GetTasksNotInPlanAsync(int planId)
        {
            var plan = await db.Plans.Get(planId);
            if (plan == null)
            {
                return null;
            }
            IEnumerable<StudentTask> tasksNotUsedInPlan = await db.Tasks.GetTasksNotInPlanAsync(planId);
            if (tasksNotUsedInPlan == null)
            {
                return null;
            }
            var tasksNotUsedInPlanList = new List<TaskDto>();
            foreach (var task in tasksNotUsedInPlan)
            {
                var taskDto = new TaskDto
                (
                    task.Id,
                                task.Name,
                                task.Description,
                                task.Private,
                                task.Create_Id,
                                await db.Users.ExtractFullNameAsync(task.Create_Id),
                                task.Mod_Id,
                                await db.Users.ExtractFullNameAsync(task.Mod_Id),
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

        public async Task<UserTaskDto> GetUserTaskByUserPlanTaskIdAsync(int userId, int planTaskId)
        {
            UserTask userTask = await db.UserTasks.GetByPlanTaskForUserAsync(planTaskId, userId);
            if (userTask == null)
            {
                return null;
            }
            var userTaskDto = new UserTaskDto(userTask.Id,
                                      userTask.User_Id,
                                      userTask.PlanTask_Id,
                                      userTask.End_Date,
                                      userTask.Propose_End_Date,
                                      userTask.Mentor_Id,
                                      userTask.State,
                                      userTask.Result);
            return userTaskDto;
        }

        public async Task<bool> UpdateUserTaskStatusAsync(int userTaskId, string newStatus)
        {
            if (!Regex.IsMatch(newStatus, ValidationRules.USERTASK_STATE))
            {
                return false;
            }
            UserTask userTask = await db.UserTasks.GetAsync(userTaskId);
            if (userTask == null)
            {
                return false;
            }
            userTask.State = newStatus;
            await db.UserTasks.UpdateAsync(userTask);
            db.Save();
            return true;
        }

        public async Task<bool> UpdateUserTaskResultAsync(int userTaskId, string newResult)
        {
            if (newResult == null)
            {
                return false;
            }
            UserTask userTask = await db.UserTasks.GetAsync(userTaskId);
            if (userTask == null)
            {
                return false;
            }
            userTask.Result = newResult;
            await db.UserTasks.UpdateAsync(userTask);
            db.Save();
            return true;
        }
        public async Task<PagedListDto<TaskDto>> GetTasks(int pageSize, int pageNumber = 1)
        {
            var queryLan = await db.Tasks.GetAll();
            var query = queryLan.AsQueryable();
            query = query.OrderBy(x => x.Id);
            return await PagedList<StudentTask, TaskDto>.GetDTO(query, pageNumber, pageSize, TaskToTaskDTOAsync);
        }

        private async Task<TaskDto> TaskToTaskDTOAsync(StudentTask task)
        {
            return new TaskDto(task.Id,
                                task.Name,
                                task.Description,
                                task.Private,
                                task.Create_Id,
                                await db.Users.ExtractFullNameAsync(task.Create_Id),
                                task.Mod_Id,
                                await db.Users.ExtractFullNameAsync(task.Mod_Id),
                                task.Create_Date,
                                task.Mod_Date,
                                null,
                                null,
                                null);
        }
        public async Task<bool> CheckUserTaskOwnerAsync(int userTaskId, int userId)
        {
            UserTask userTask = await db.UserTasks.GetAsync(userTaskId);
            return userTask.User_Id == userId;
        }
    }
}
