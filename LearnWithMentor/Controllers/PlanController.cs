using System.Collections.Generic;
using System.Web.Http;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.UnitOfWork;
using LearnWithMentorDTO;

namespace LearnWithMentor.Controllers
{
    public class PlanController : ApiController
    {
        private IUnitOfWork UoW;
        public PlanController()
        {
            UoW = new UnitOfWork(new LearnWithMentor_DBEntities());
        }
        // GET: api/Plan
        public IEnumerable<PlanDTO> Get()
        {
            var dto = new List<PlanDTO>();

            foreach (var p in UoW.Plans.GetAll())
            {
                var firstName = p.Users1?.FirstName;
                var lastName = p.Users1?.LastName;
                dto.Add(
                    new PlanDTO(
                        p.Id, 
                        p.Name, 
                        p.Description, 
                        p.Published, 
                        p.Create_Id, 
                        p.Users.FirstName, 
                        p.Users.LastName, 
                        p.Mod_Id,
                        firstName,
                        lastName, 
                        p.Create_Date, 
                        p.Mod_Date
                        )
                    );
                }
            return dto;
        }


    }
}
