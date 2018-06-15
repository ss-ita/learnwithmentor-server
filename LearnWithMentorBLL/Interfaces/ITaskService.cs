using System;
using System.Collections.Generic;
using LearnWithMentorDAL;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    interface ITaskService
    {
        TaskDTO GetTasks(int id);
        void RemoveTaskById(int id);
        void DeleteTaskById(int id);
        IEnumerable<TaskDTO> Search(string[] str, int planId);
        IEnumerable<TaskDTO> Search(string[] str);
        void Dispose();
    }
}
