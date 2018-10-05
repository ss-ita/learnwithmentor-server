using System.Collections.Generic;
using System;
using LearnWithMentorDTO;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ITaskService : IDisposableService
    {
        bool CreateTask(TaskDto taskDTO);
        Task<IEnumerable<TaskDto>> GetAllTasksAsync();
        Task<TaskDto> GetTaskByIdAsync(int taskId);
        Task<int?> AddAndGetIdAsync(TaskDto taskDTO);
        Task<TaskDto> GetTaskForPlanAsync(int taskId, int planId);
        Task<TaskDto> GetTaskForPlanAsync(int planTaskId);
        Task<IEnumerable<TaskDto>> GetTasksNotInPlanAsync(int planId);
        Task<UserTaskDto> GetUserTaskByUserPlanTaskIdAsync(int userId, int planTaskId);
        Task<bool> CreateUserTaskAsync(UserTaskDto userTaskDTO);
        Task<bool> UpdateUserTaskStatusAsync(int userTaskId, string newStatus);
        Task<bool> UpdateUserTaskResultAsync(int userTaskId, string newResult);
        Task<bool> UpdateTaskByIdAsync(int taskId,TaskDto taskDTO);
        Task<bool> RemoveTaskByIdAsync(int taskId);
        Task<bool> UpdateProposeEndDateAsync(int userTaskId, DateTime proposeEndDate);
        Task<List<UserTaskDto>> GetTaskStatesForUserAsync(int[] planTaskIds, int userId);
        Task<IEnumerable<TaskDto>> SearchAsync(string[] str, int planId);
        Task<List<TaskDto>> SearchAsync(string[] keys);
        Task<StatisticsDto> GetUserStatisticsAsync(int userId);
        Task<PagedListDto<TaskDto>> GetTasks(int pageSize, int pageNumber = 1);
        Task<bool> DeleteProposeEndDateAsync(int userTaskId);
        Task<bool> SetNewEndDateAsync(int userTaskId);
        Task<bool> CheckUserTaskOwnerAsync(int userTaskId, int userId);
    }
}
