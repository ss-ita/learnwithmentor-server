using System.Collections.Generic;
using System;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ITaskService : IDisposableService
    {
        IEnumerable<TaskDTO> GetAllTasks();
        TaskDTO GetTaskById(int id);
        int? AddAndGetId(TaskDTO taskDTO);
        TaskDTO GetTaskForPlan(int taskId, int planId);
        TaskDTO GetTaskForPlan(int planTaskId);
        IEnumerable<TaskDTO> GetTasksNotInPlan(int planId);
        UserTaskDTO GetUserTaskByUserPlanTaskId(int userId, int planTaskId);
        bool CreateTask(TaskDTO dto);
        bool CreateUserTask(UserTaskDTO utDTO);
        bool UpdateUserTaskStatus(int userTaskId, string newStatus);
        bool UpdateUserTaskResult(int userTaskId, string newResult);
        bool UpdateTaskById(int taskId,TaskDTO dto);
        bool RemoveTaskById(int id);
        bool UpdateProposeEndDate(int userTaskId, DateTime proposeEndDate);
        List<UserTaskDTO> GetTaskStatesForUser(int[] planTaskIds, int userId);
        IEnumerable<TaskDTO> Search(string[] str, int planId);
        List<TaskDTO> Search(string[] keys);
        StatisticsDTO GetUserStatistics(int userId);
        PagedListDTO<TaskDTO> GetTasks(int pageSize, int pageNumber = 1);
        bool DeleteProposeEndDate(int userTaskId);
        bool SetNewEndDate(int userTaskId);
        bool CheckUserTaskOwner(int userTaskId, int userId);
    }
}
