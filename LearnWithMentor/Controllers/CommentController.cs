using System.Web.Http;
using LearnWithMentorDAL;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentor.Controllers
{
    public class CommentController : ApiController
    {
        private IUnitOfWork UoW;

        public CommentController()
        {
            UoW = new UnitOfWork(new LearnWithMentor_DBEntities());
        }

        [HttpGet]
        [Route("api/comment/{id}")]
        public CommentDTO Get(int id)
        {
            Comment comment = UoW.Comments.Get(id);
            if (comment == null) return null;
            return new CommentDTO(comment.id, comment.Text, comment.Create_Id, comment.Users.FirstName, comment.Users.LastName, comment.Create_Date, comment.Mod_Date);
        }

        [HttpDelete]
        [Route("api/comment/{id}")]
        public IHttpActionResult Delete(int id)
        {
            UoW.Comments.RemoveById(id);
            UoW.Save();
            return Ok();
        }

        [HttpPut]
        [Route("api/comment")]
        public IHttpActionResult PutComment([FromBody]CommentDTO value)
        {
            UoW.Comments.Update(value);
            UoW.Save();
            return Ok();
        }
    }
}
