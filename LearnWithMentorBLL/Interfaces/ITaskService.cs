using System.Collections.Generic;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ITaskService : IDisposableService
    {
        IEnumerable<TaskDTO> GetAllTasks();
        TaskDTO GetTaskById(int id);
        TaskDTO GetTaskForPlan(int taskId, int planId);
        TaskDTO GetTaskForPlan(int planTaskId);
        List<UserTaskDTO> GetUserTasksList(int userId, int[] planTaskId);
        UserTaskDTO GetUserTaskByUserPlanTaskId(int userId, int planTaskId);
        bool CreateTask(TaskDTO dto);
        bool CreateUserTask(UserTaskDTO utDTO);
        bool UpdateUserTaskStatus(int userTaskId, string newStatus);
        bool UpdateUserTaskResult(int userTaskId, string newResult);
        bool UpdateTaskById(int taskId,TaskDTO dto);
        bool RemoveTaskById(int id);
        IEnumerable<TaskDTO> Search(string[] str, int planId);
        IEnumerable<TaskDTO> Search(string[] str);
        StatisticsDTO GetUserStatistics(int userId);
    }
}
