using System.Collections.Generic;
using System;
using LearnWithMentorDTO;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ITaskService : IDisposableService
    {
        Task<IEnumerable<TaskDto>> GetAllTasks();
        Task<TaskDto> GetTaskById(int taskId);
        Task<int?> AddAndGetId(TaskDto taskDTO);
        Task<TaskDto> GetTaskForPlan(int taskId, int planId);
        Task<TaskDto> GetTaskForPlan(int planTaskId);
        Task<IEnumerable<TaskDto>> GetTasksNotInPlan(int planId);
        Task<UserTaskDto> GetUserTaskByUserPlanTaskId(int userId, int planTaskId);
        bool CreateTask(TaskDto taskDTO);
        Task<bool> CreateUserTask(UserTaskDto userTaskDTO);
        Task<bool> UpdateUserTaskStatus(int userTaskId, string newStatus);
        Task<bool> UpdateUserTaskResult(int userTaskId, string newResult);
        Task<bool> UpdateTaskById(int taskId,TaskDto taskDTO);
        Task<bool> RemoveTaskById(int taskId);
        Task<bool> UpdateProposeEndDate(int userTaskId, DateTime proposeEndDate);
        Task<List<UserTaskDto>> GetTaskStatesForUser(int[] planTaskIds, int userId);
        Task<IEnumerable<TaskDto>> Search(string[] str, int planId);
        Task<List<TaskDto>> Search(string[] keys);
        Task<StatisticsDto> GetUserStatistics(int userId);
        Task<PagedListDto<TaskDto>> GetTasks(int pageSize, int pageNumber = 1);
        Task<bool> DeleteProposeEndDate(int userTaskId);
        Task<bool> SetNewEndDate(int userTaskId);
        Task<bool> CheckUserTaskOwner(int userTaskId, int userId);
    }
}
