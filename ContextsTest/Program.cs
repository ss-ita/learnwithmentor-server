using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.UnitOfWork;
using LearnWithMentorDTO;

namespace ContextsTest
{
    class Program
    {
        static void Main(string[] args)
        {


            using (LearnWithMentor_DBEntities db = new LearnWithMentor_DBEntities())
            {
                foreach (var g in db.Users)
                    Console.WriteLine("User: {0} {1}", g.FirstName, g.LastName);
            }
            UnitOfWork UoW = new UnitOfWork(new LearnWithMentor_DBEntities());
            List<TaskDTO> dto = new List<TaskDTO>();
            foreach (var t in UoW.Tasks.GetAll())
            {
                //added null params to replace creator and modifier names
                dto.Add(new TaskDTO(t.Id,
                                    t.Name,
                                    t.Description,
                                    t.Private,
                                    t.Create_Id,
                                    null,
                                    t.Mod_Id,
                                    null,
                                    t.Create_Date,
                                    t.Mod_Date,
                                    null,
                                    null));
            }
            Console.ReadKey();
        }
    }
}
