using System;
using System.Collections.Generic;
using LearnWithMentorDAL;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ITaskService
    {
        IEnumerable<TaskDTO> GetAllTasks();
        TaskDTO GetTaskById(int id);
        TaskDTO GetTaskForPlan(int taskId, int planId);
        UserTaskDTO GetUserTaskByUserTaskPlanIds(int userId,int taskId, int planId);
        bool CreateTask(TaskDTO dto);
        bool CreateUserTask(UserTaskDTO utDTO);
        bool UpdateUserTaskStatus(int userId, int taskId, int planId, string newStatus);
        bool UpdateTaskById(int taskId,TaskDTO dto);
        bool RemoveTaskById(int id);
        List<UserTaskStateDTO> GetTaskStatesForUser(int[] planTaskIds, int userId);
        IEnumerable<TaskDTO> GetAllTasksForPlan(int taskId, int planId);
        IEnumerable<TaskDTO> Search(string[] str, int planId);
        IEnumerable<TaskDTO> Search(string[] str);
    }
}
