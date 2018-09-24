using System.Collections.Generic;
using System;
using LearnWithMentorDTO;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ITaskService : IDisposableService
    {
        IEnumerable<TaskDto> GetAllTasks();
        TaskDto GetTaskById(int taskId);
        int? AddAndGetId(TaskDto taskDTO);
        Task<TaskDto> GetTaskForPlan(int taskId, int planId);
        Task<TaskDto> GetTaskForPlan(int planTaskId);
        Task<IEnumerable<TaskDto>> GetTasksNotInPlan(int planId);
        Task<UserTaskDto> GetUserTaskByUserPlanTaskId(int userId, int planTaskId);
        bool CreateTask(TaskDto taskDTO);
        Task<bool> CreateUserTask(UserTaskDto userTaskDTO);
        Task<bool> UpdateUserTaskStatus(int userTaskId, string newStatus);
        Task<bool> UpdateUserTaskResult(int userTaskId, string newResult);
        bool UpdateTaskById(int taskId,TaskDto taskDTO);
        bool RemoveTaskById(int taskId);
        Task<bool> UpdateProposeEndDate(int userTaskId, DateTime proposeEndDate);
        Task<List<UserTaskDto>> GetTaskStatesForUser(int[] planTaskIds, int userId);
        Task<IEnumerable<TaskDto>> Search(string[] str, int planId);
        List<TaskDto> Search(string[] keys);
        Task<StatisticsDto> GetUserStatistics(int userId);
        PagedListDto<TaskDto> GetTasks(int pageSize, int pageNumber = 1);
        Task<bool> DeleteProposeEndDate(int userTaskId);
        Task<bool> SetNewEndDate(int userTaskId);
        Task<bool> CheckUserTaskOwner(int userTaskId, int userId);
    }
}
