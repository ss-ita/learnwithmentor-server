using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using LearnWithMentor.Filters;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Infrastructure;
using LearnWithMentorBLL.Services;

namespace LearnWithMentor.Controllers
{
    [Authorize]
    [JwtAuthentication]
    public class CommentController : ApiController
    {

        private readonly ICommentService commentService;
        
        public CommentController()
        {
            commentService = new CommentService();
        }

        #region Get
        /// <summary>Returns comments for task in plan.</summary>
        /// <param name="id">ID of the comment.</param>
        [HttpGet]
        [Route("api/comment")]
        public HttpResponseMessage GetComment(int id)
        {
            try
            {
                CommentDTO comment = commentService.GetComment(id);
                if (comment == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Comment with this ID does not exist in database.");
                return Request.CreateResponse(HttpStatusCode.OK, comment);
            }
            catch (ValidationException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Internal server error");
            }
        }

        /// <summary>Returns comments for task in plan.</summary>
        /// <param name="taskId">ID of the tast.</param>
        /// <param name="planId">ID of the plan.</param>
        [HttpGet]
        [Route("api/comment")]
        public HttpResponseMessage GetCommentsForPlanAndTaskId(int taskId, int planId)
        {
            try
            {
                var t = commentService.GetCommentsForPlanTask(taskId, planId);
                return Request.CreateResponse(HttpStatusCode.OK, t);
            }
            catch (ValidationException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>Returns comments for plantask.</summary>
        /// <param name="planTaskId">ID of the plantask.</param>
        [HttpGet]
        [Route("api/comment")]
        public HttpResponseMessage GetCommentsForPlanTask(int planTaskId)
        {
            try
            {
                var t = commentService.GetCommentsForPlanTask(planTaskId);
                return Request.CreateResponse(HttpStatusCode.OK, t);
            }
            catch (ValidationException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        #endregion

        /// <summary>Adds comment for task in plan.</summary>
        /// <param name="taskId">ID of the tast.</param>
        /// <param name="planId">ID of the plan.</param>
        /// <param name="comment">New comment.</param>
        [HttpPost]
        [Route("api/comment")]
        public HttpResponseMessage Post(int taskId, int planId,CommentDTO comment)
        {
            try
            {
                if (commentService.AddCommentToPlanTask(planId, taskId, comment))
                    return Request.CreateResponse(HttpStatusCode.OK, "Comment succesfully created");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Creation error");
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Creation error");
            }
        }

        /// <summary>Adds comment for task in plan.</summary>
        /// <param name="planTaskId">ID of the plantask.</param>
        /// <param name="comment">New comment.</param>
        [HttpPost]
        [Route("api/comment")]
        public HttpResponseMessage Post(int planTaskId, CommentDTO comment)
        {
            try
            {
                if (commentService.AddCommentToPlanTask(planTaskId, comment))
                    return Request.CreateResponse(HttpStatusCode.OK, "Comment succesfully created");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Creation error");
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Creation error");
            }
        }

        /// <summary>Updates comment for task in plan.</summary>
        /// <param name="commentId">ID of the comment.</param>
        /// <param name="comment">New comment.</param>
        [HttpPut]
        [Route("api/comment")]
        public HttpResponseMessage PutComment(int commentId, [FromBody]CommentDTO comment)
        {
            try
            {
                if (commentService.UpdateComment(commentId, comment))
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully updated comment id: {commentId}.");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"No task with id: {commentId} or cannot be updated.");
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Updation error");
            }
        }

        /// <summary>Deletes comment for task in plan.</summary>
        /// <param name="commentId">ID of the comment.</param>
        [HttpDelete]
        [Route("api/comment/{id}")]
        public HttpResponseMessage Delete(int commentId)
        {
            try
            {
                if (commentService.RemoveById(commentId))
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully deleted comment id: {commentId}.");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"No task with id: {commentId} or cannot be deleted.");
            }
            catch(Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Deletion error.");
            }
        }
    }
}
