using System.Web.Http;
using System.Net;
using System.Net.Http;
using LearnWithMentor.Filters;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using System.Web.Http.Tracing;
using System.Data.Entity.Core;

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
        private readonly ITraceWriter tracer;

        /// <summary>
        /// Services initiation
        /// </summary>
        public CommentController(ICommentService commentService, ITraceWriter tracer)
        {
            this.commentService = commentService;
            this.tracer = tracer;
        }

        /// <summary>Returns comment by id.</summary>
        /// <param name="id">Id of the comment.</param>
        [HttpGet]
        [Route("api/comment")]
        public HttpResponseMessage GetComment(int id)
        {
            try
            {
                var comment = commentService.GetComment(id);
                if (comment == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Comment with this Id does not exist in database.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, comment);
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal server error");
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
                if (comments == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "There are no comments for this task in that plan");
                }
                return Request.CreateResponse(HttpStatusCode.OK, comments);
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal server error");
            }
        }

        /// <summary>Adds comment for planTask.</summary>
        /// <param name="planTaskId">Id of the plantask.</param>
        /// <param name="comment">New comment.</param>
        [HttpPost]
        [Route("api/comment")]
        public HttpResponseMessage Post(int planTaskId, CommentDTO comment)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                if (commentService.AddCommentToPlanTask(planTaskId, comment))
                {
                    var log = $"Succesfully created comment with id = {comment.Id} by user id = {comment.CreatorId}";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, "Comment succesfully created");
                }
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on creating comment");
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Not possibly to add comment: task in this plan does not exist");
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal creation error");
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
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully updated comment id: {commentId}.");
                }
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on updating comment");
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, ("Not possibly to update comment: comment does not exist."));
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal updation error");
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
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully deleted comment id: {commentId}.");
                }
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on deleting comment");
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, ("Not possibly to delete comment: comment does not exist."));
            }
            catch(EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal deletion error.");
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
