using System.Web.Http;
using feedback360.Backend.DTOs;
using feedback360.Backend.Repositories;

namespace feedback360.Backend.Controllers
{
    [RoutePrefix("api/feedback")]
    public class FeedbackController : ApiController
    {
        private readonly IFeedbackRepository _repository;

        public FeedbackController()
        {
            _repository = new FeedbackRepository();
        }

        [HttpGet]
        [Route("pending")]
        public IHttpActionResult GetPending(string userId, string fy)
        {
            var records = _repository.GetPendingRecords(userId, fy);
            return Ok(records);
        }

        [HttpPost]
        [Route("submit")]
        public IHttpActionResult Submit(FeedbackSubmitRequestDTO request)
        {
            if (_repository.SubmitFeedback(request))
                return Ok(new { message = "Feedback submitted successfully" });
            return BadRequest("Failed to submit feedback");
        }

        [HttpPost]
        [Route("reject")]
        public IHttpActionResult Reject(FeedbackRejectRequestDTO request)
        {
            if (_repository.RejectFeedback(request))
                return Ok(new { message = "Feedback rejected successfully" });
            return BadRequest("Failed to reject feedback");
        }
    }
}
