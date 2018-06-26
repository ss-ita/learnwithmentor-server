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
using System.Web.Http.Tracing;
using LearnWithMentor.Log;

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
        private readonly ITraceWriter _tracer;

        /// <summary>
        /// Services initiation
        /// </summary>
        public CommentController()
        {
            commentService = new CommentService();
            _tracer = new NLogger();
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
            catch (InternalServiceException exception)
            {
                _tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, exception);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception.Message);
            }
            catch (Exception exception)
            {
                _tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, exception);
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
            catch (InternalServiceException exception)
            {
                _tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, exception);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception.Message);
            }
            catch (Exception exception)
            {
                _tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, exception);
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
                {
                    var log = $"Succesfully created comment with id = {comment.Id} by user id = {comment.CreatorId}";
                    _tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, "Comment succesfully created");
                }
                _tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on creating comment");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Creation error");
            }
            catch (Exception exception)
            {
                _tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, exception);
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
                {
                    var log = $"Succesfully updated comment with id = {commentId}";
                    _tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully updated comment id: {commentId}.");
                }
                _tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on updating comment");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"No task with id: {commentId} or cannot be updated.");
            }
            catch (Exception exception)
            {
                _tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, exception);
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
                {
                    var log = $"Succesfully deleted comment with id = {commentId}";
                    _tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully deleted comment id: {commentId}.");
                }
                _tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on deleting comment");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"No task with id: {commentId} or cannot be deleted.");
            }
            catch(Exception exception)
            {
                _tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, exception);
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
