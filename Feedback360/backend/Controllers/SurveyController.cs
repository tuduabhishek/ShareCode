using System;
using System.Web.Http;
using feedback360.Api.DTOs;
using feedback360.Api.Repositories;

namespace feedback360.Api.Controllers
{
    [RoutePrefix("api/survey")]
    public class SurveyController : ApiController
    {
        private readonly SurveyRepository _repository = new SurveyRepository();

        [HttpGet]
        [Route("pending")]
        public IHttpActionResult GetPending(string respondentId, string fy, string cycle)
        {
            return Ok(_repository.GetPendingSurveys(respondentId, fy, cycle));
        }

        [HttpGet]
        [Route("details")]
        public IHttpActionResult GetDetails(string respondentId, string assesPno, string fy, string cycle)
        {
            var details = _repository.GetSurveyDetails(respondentId, assesPno, fy, cycle);
            if (details == null) return NotFound();
            return Ok(details);
        }

        [HttpPost]
        [Route("submit")]
        public IHttpActionResult Submit(SurveyResponseDTO response)
        {
            if (_repository.SubmitFeedback(response))
                return Ok(new { message = "Feedback submitted successfully" });
            return BadRequest("Could not submit feedback");
        }

        [HttpPost]
        [Route("reject")]
        public IHttpActionResult Reject(string respondentId, string assesPno, string fy, string cycle)
        {
            if (_repository.RejectFeedback(respondentId, assesPno, fy, cycle))
                return Ok(new { message = "Feedback marked as insufficient exposure" });
            return BadRequest("Could not process request");
        }
        [HttpPost]
        [Route("verify-otp")]
        public IHttpActionResult VerifyOTP(string id, string otp)
        {
            if (_repository.VerifyOTP(id, otp, out string assesPno, out string fy, out string cycle))
            {
                return Ok(new { assesPno, fy, cycle });
            }
            return BadRequest("Invalid ID or OTP");
        }
    }
}
