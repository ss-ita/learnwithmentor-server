using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    class PlanRepository: BaseRepository<Plan>, IPlanRepository
    {
        public PlanRepository(LearnWithMentor_DBEntities _context) : base(_context)
        {
        }
        public Plan Get(int id)
        {
            return context.Plans.Where(p => p.Id == id).FirstOrDefault();
        }
    }
}
