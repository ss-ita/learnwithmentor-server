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

        [HttpGet]
        [Route("api/comment/{id}")]
        public HttpResponseMessage Get(int id)
        {
            CommentDTO comment = commentService.GetComment(id);
            if(comment==null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Comment with this ID does not exist in database.");
            return Request.CreateResponse(HttpStatusCode.OK, comment);
        }

        [HttpPost]
        [Route("api/comment")]
        public HttpResponseMessage Post(int taskId, int planId,CommentDTO c)
        {
            if (commentService.AddCommentToPlanTask(planId, taskId, c))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Creation error");
            return Request.CreateResponse(HttpStatusCode.OK, "Comment succesfully created");
        }

        [HttpDelete]
        [Route("api/comment/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            if(commentService.RemoveById(id))
                return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully deleted comment id: {id}.");
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"No task with id: {id} or cannot be deleted.");
        }

        [HttpPut]
        [Route("api/comment")]
        public HttpResponseMessage PutComment(int id, [FromBody]CommentDTO value)
        {
            if (commentService.UpdateComment(id, value))
                return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully updated comment id: {id}.");
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"No task with id: {id} or cannot be updated.");
        }
    }
}
