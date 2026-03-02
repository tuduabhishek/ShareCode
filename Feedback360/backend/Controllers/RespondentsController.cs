using System.Web.Http;
using feedback360.Backend.DTOs;
using feedback360.Backend.Repositories;

namespace feedback360.Backend.Controllers
{
    [RoutePrefix("api/respondent")]
    public class RespondentsController : ApiController
    {
        private readonly IRespondentRepository _repository;

        public RespondentsController()
        {
            _repository = new RespondentRepository();
        }

        [HttpGet]
        [Route("assessees")]
        public IHttpActionResult GetAssessees(string adminId, bool isSuperAdmin, string fy, string cycle, string execHead = null, string dept = null, string perno = null)
        {
            return Ok(_repository.GetAssesseesForAdmin(adminId, isSuperAdmin, fy, cycle, execHead, dept, perno));
        }

        [HttpGet]
        [Route("list")]
        public IHttpActionResult GetList(string assesPno, string fy, string cycle)
        {
            return Ok(_repository.GetRespondents(assesPno, fy, cycle));
        }

        [HttpGet]
        [Route("rules")]
        public IHttpActionResult GetRules(string level)
        {
            return Ok(_repository.GetValidationRules(level));
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult Add(RespondentSaveRequestDTO request)
        {
            var id = _repository.AddRespondent(request);
            return Ok(new { message = "Respondent added successfully", id = id });
        }

        [HttpDelete]
        [Route("remove")]
        public IHttpActionResult Remove(string assesPno, string respPno, string category, string fy, string cycle)
        {
            if (_repository.RemoveRespondent(assesPno, respPno, category, fy, cycle))
                return Ok(new { message = "Respondent removed" });
            return BadRequest("Could not remove respondent");
        }

        [HttpPost]
        [Route("submit")]
        public IHttpActionResult Submit(string assesPno, string fy, string cycle, string me)
        {
            if (_repository.SubmitRespondentList(assesPno, fy, cycle, me))
                return Ok(new { message = "List submitted" });
            return BadRequest("Submission failed");
        }

        [HttpPost]
        [Route("approve")]
        public IHttpActionResult Approve(string assesPno, string fy, string cycle, string me)
        {
            if (_repository.ApproveRespondentList(assesPno, fy, cycle, me))
                return Ok(new { message = "List approved" });
            return BadRequest("Approval failed");
        }

        [HttpPost]
        [Route("reject")]
        public IHttpActionResult Reject(string assesPno, string fy, string cycle, string me)
        {
            if (_repository.RejectRespondentList(assesPno, fy, cycle, me))
                return Ok(new { message = "List rejected" });
            return BadRequest("Rejection failed");
        }
        [HttpGet]
        [Route("auto-populated")]
        public IHttpActionResult GetAutoPopulated(string assesPno, string fy, string cycle, string level)
        {
            return Ok(_repository.GetAutoPopulatedRespondents(assesPno, fy, cycle, level));
        }
    }
}
