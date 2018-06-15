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
        IEnumerable<TaskDTO> GetAllTasksForPlan(int taskId, int planId);
        IEnumerable<CommentDTO> GetTaskCommentsForPlan(int taskId, int planId);
        void RemoveTaskById(int id);
        void DeleteTaskById(int id);
        IEnumerable<TaskDTO> Search(string[] str, int planId);
        IEnumerable<TaskDTO> Search(string[] str);
    }
}
