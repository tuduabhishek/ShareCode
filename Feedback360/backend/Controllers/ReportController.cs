using System.Web.Http;
using feedback360.Backend.Repositories;

namespace feedback360.Backend.Controllers
{
    [RoutePrefix("api/reports")]
    public class ReportController : ApiController
    {
        private readonly IReportRepository _repository;

        public ReportController()
        {
            _repository = new ReportRepository();
        }

        [HttpGet]
        [Route("detailed-status")]
        public IHttpActionResult GetDetailedStatus(string year, string cycle, string pno = null)
        {
            return Ok(_repository.GetDetailedStatusReport(year, cycle, pno));
        }

        [HttpGet]
        [Route("summary-completion")]
        public IHttpActionResult GetSummaryCompletion(string year, string cycle)
        {
            return Ok(_repository.GetSummaryCompletionReport(year, cycle));
        }

        [HttpGet]
        [Route("raw-data")]
        public IHttpActionResult GetRawData(string perno, string year)
        {
            return Ok(_repository.GetRawSurveyData(perno, year));
        }
        [HttpGet]
        [Route("individual-scores")]
        public IHttpActionResult GetIndividualScores(string perno, string year)
        {
            return Ok(_repository.GetIndividualScores(perno, year));
        }

        [HttpGet]
        [Route("individual-comments")]
        public IHttpActionResult GetIndividualComments(string perno, string year)
        {
            return Ok(_repository.GetIndividualComments(perno, year));
        }
    }
}
