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
    /// <summary>
    /// Controller, that provides API for work with comments
    /// </summary>
    [Authorize]
    [JwtAuthentication]
    public class CommentController : ApiController
    {
        /// <summary>
        /// Services for work with different DB parts
        /// </summary>
        private readonly ICommentService commentService;

        /// <summary>
        /// Services initiation
        /// </summary>
        public CommentController()
        {
            commentService = new CommentService();
        }

        /// <summary>Returns comment by id.</summary>
        /// <param name="id">Id of the comment.</param>
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
            catch (ValidationException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Internal server error");
            }
        }

        /// <summary>Returns comments by plantask id.</summary>
        /// <param name="planTaskId">Id of the plantask.</param>
        [HttpGet]
        [Route("api/comment/plantask/{planTaskId}")]
        public HttpResponseMessage GetCommentsForPlanTask(int planTaskId)
        {
            try
            {
                var comments = commentService.GetCommentsForPlanTask(planTaskId);
                return Request.CreateResponse(HttpStatusCode.OK, comments);
            }
            catch (ValidationException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Internal server error");
            }
        }

        /// <summary>Adds comment for planTask.</summary>
        /// <param name="planTaskId">Id of the plantask.</param>
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
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Internal creation error");
            }
        }

        /// <summary>Updates comment by id.</summary>
        /// <param name="commentId">Id of the comment.</param>
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
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Internal updation error");
            }
        }

        /// <summary>Deletes comment by id.</summary>
        /// <param name="commentId">Id of the comment.</param>
        [HttpDelete]
        [Route("api/comment/{commentId}")]
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
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Internal deletion error.");
            }
        }

        /// <summary>
        /// Releases memory
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            commentService.Dispose();
            base.Dispose(disposing);
        }
    }
}
