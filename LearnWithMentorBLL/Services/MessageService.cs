using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Infrastructure;

namespace LearnWithMentorBLL.Services
{
    public class MessageService: BaseService, IMessageService
    {
        public MessageService() : base()
        {
        }
        public IEnumerable<MessageDTO> GetMessages(int userId, int taskId, int planId)
        {
            int? ptId = db.PlanTasks.GetIdByTaskAndPlan(taskId, planId);
            if (ptId == null)
                throw new ValidationException("No task in this plan","");
            var ut = db.UserTasks.Get(ptId.Value, userId);
            if(ut==null)
                throw new ValidationException("No task in this plan for this user", "");
            var mss = db.Messages.GetByUserTaskId(ut.Id);

            List<MessageDTO> dto = new List<MessageDTO>();
            foreach (var m in mss)
            {
                // todo mentor/user - sender/receiver logic to add
                dto.Add(new MessageDTO(m.Id,
                                       userId,
                                       taskId,
                                       planId,
                                       ut.Mentor_Id,
                                       m.Text,
                                       m.Send_Time));
            }
            return dto;
        }

    }
    
}
