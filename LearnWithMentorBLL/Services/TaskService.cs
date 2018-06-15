using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Services
{
    class TaskService : BaseService,ITaskService
    {
        public TaskService() : base()
        {
        }
        public void DeleteTaskById(int id)
        {
            
        }

        public TaskDTO GetTasks(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveTaskById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskDTO> Search(string[] str, int planId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskDTO> Search(string[] str)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
