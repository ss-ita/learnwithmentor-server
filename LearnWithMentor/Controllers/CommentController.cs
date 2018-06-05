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
        [Route("api/comment/{commentId}")]
        public CommentDTO Get(int commentId)
        {
            Comment comment = UoW.Comments.Get(commentId);
            if (comment == null) return null;
            return new CommentDTO(comment.Id, comment.Text, comment.Create_Id, comment.Users.FirstName, comment.Users.LastName, comment.Create_Date, comment.Mod_Date);
        }

        [HttpDelete]
        [Route("api/comment/{commentId}")]
        public IHttpActionResult Delete(int id)
        {
            UoW.Comments.RemoveById(id);
            UoW.Save();
            return Ok();
        }
    }
}
