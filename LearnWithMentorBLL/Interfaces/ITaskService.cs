using System;
using System.Collections.Generic;
using LearnWithMentorDAL;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    interface ITaskService
    {
        IEnumerable<TaskDTO> GetAllTasks();
        TaskDTO GetTasks(int id);
        TaskDTO GetTasksForPlan(int taskId, int planId);
        TaskDTO GetTaskCommentsForPlan(int taskId, int planId);
        void RemoveTaskById(int id);
        void DeleteTaskById(int id);
        IEnumerable<TaskDTO> Search(string[] str, int planId);
        IEnumerable<TaskDTO> Search(string[] str);
        void Dispose();
    }
}
