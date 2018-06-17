using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Infrastructure;
using LearnWithMentorBLL.Services;

namespace LearnWithMentor.Controllers
{
    public class CommentController : ApiController
    {

        private readonly ICommentService commentService;
        
        public CommentController()
        {
            commentService = new CommentService();
        }

        /// <summary>
        /// Returns comments for task in plan.
        /// </summary>
        /// <param name="taskId">ID of the tast.</param>
        /// <param name="planId">ID of the plan.</param>
        [HttpGet]
        [Route("api/comment/plantaskcomments")]
        public HttpResponseMessage GetCommentsForPlanTask(int taskId, int planId)
        {
            try
            {
                var t = commentService.GetTaskCommentsForPlan(taskId, planId);
                return Request.CreateResponse(HttpStatusCode.OK, t);
            }
            catch (ValidationException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        //[HttpGet]
        //[Route("api/comment/{id}")]
        //public CommentDTO Get(int id)
        //{
        //    Comment comment = UoW.Comments.Get(id);
        //    if (comment == null) return null;
        //    return new CommentDTO(comment.Id, comment.Text, comment.Create_Id, comment.Creator.FirstName, comment.Creator.LastName, comment.Create_Date, comment.Mod_Date);
        //}

        //[HttpDelete]
        //[Route("api/comment/{id}")]
        //public IHttpActionResult Delete(int id)
        //{
        //    UoW.Comments.RemoveById(id);
        //    UoW.Save();
        //    return Ok();
        //}

        //[HttpPut]
        //[Route("api/comment")]
        //public IHttpActionResult PutComment([FromBody]CommentDTO value)
        //{
        //    UoW.Comments.Update(value);
        //    UoW.Save();
        //    return Ok();
        //}
    }
}
