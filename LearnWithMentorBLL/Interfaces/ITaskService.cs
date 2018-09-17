using System.Collections.Generic;
using System;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ITaskService : IDisposableService
    {
        IEnumerable<TaskDto> GetAllTasks();
        TaskDto GetTaskById(int taskId);
        int? AddAndGetId(TaskDto taskDTO);
        TaskDto GetTaskForPlan(int taskId, int planId);
        TaskDto GetTaskForPlan(int planTaskId);
        IEnumerable<TaskDto> GetTasksNotInPlan(int planId);
        UserTaskDto GetUserTaskByUserPlanTaskId(int userId, int planTaskId);
        bool CreateTask(TaskDto taskDTO);
        bool CreateUserTask(UserTaskDto userTaskDTO);
        bool UpdateUserTaskStatus(int userTaskId, string newStatus);
        bool UpdateUserTaskResult(int userTaskId, string newResult);
        bool UpdateTaskById(int taskId,TaskDto taskDTO);
        bool RemoveTaskById(int taskId);
        bool UpdateProposeEndDate(int userTaskId, DateTime proposeEndDate);
        List<UserTaskDto> GetTaskStatesForUser(int[] planTaskIds, int userId);
        IEnumerable<TaskDto> Search(string[] str, int planId);
        List<TaskDto> Search(string[] keys);
        StatisticsDto GetUserStatistics(int userId);
        PagedListDto<TaskDto> GetTasks(int pageSize, int pageNumber = 1);
        bool DeleteProposeEndDate(int userTaskId);
        bool SetNewEndDate(int userTaskId);
        bool CheckUserTaskOwner(int userTaskId, int userId);
    }
}
