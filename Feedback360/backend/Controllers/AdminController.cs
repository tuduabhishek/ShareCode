using System.Collections.Generic;
using System.Web.Http;
using feedback360.Backend.DTOs;
using feedback360.Backend.Repositories;

namespace feedback360.Backend.Controllers
{
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        private readonly IAdminRepository _repository;

        public AdminController()
        {
            _repository = new AdminRepository();
        }

        [HttpGet]
        [Route("potential-exclusions")]
        public IHttpActionResult GetPotentialExclusions(string year, string cycle, string execHead = null, string sgrade = null, string buhr = null, string perno = null)
        {
            return Ok(_repository.GetPotentialExclusions(year, cycle, execHead, sgrade, buhr, perno));
        }

        [HttpGet]
        [Route("excluded")]
        public IHttpActionResult GetExcluded(string year, string cycle, string execHead = null, string sgrade = null, string buhr = null, string perno = null)
        {
            return Ok(_repository.GetExcludedEmployees(year, cycle, execHead, sgrade, buhr, perno));
        }

        [HttpPost]
        [Route("add-exclusions")]
        public IHttpActionResult AddExclusions(List<ExcludedEmployeeDTO> exclusions, string me)
        {
            if (_repository.AddExclusions(exclusions, me))
                return Ok(new { message = "Exclusions added successfully" });
            return BadRequest("Failed to add exclusions");
        }

        [HttpPost]
        [Route("remove-exclusions")]
        public IHttpActionResult RemoveExclusions(List<ExcludedEmployeeDTO> exclusions)
        {
            if (_repository.RemoveExclusions(exclusions))
                return Ok(new { message = "Exclusions removed successfully" });
            return BadRequest("Failed to remove exclusions");
        }

        [HttpGet]
        [Route("super-admins")]
        public IHttpActionResult GetSuperAdmins(string userId = null)
        {
            return Ok(_repository.GetSuperAdmins(userId));
        }

        [HttpPost]
        [Route("add-super-admin")]
        public IHttpActionResult AddSuperAdmin(string userId, string me)
        {
            if (_repository.AddSuperAdmin(userId, me))
                return Ok(new { message = "Super Admin added successfully" });
            return BadRequest("Failed to add Super Admin");
        }

        [HttpDelete]
        [Route("remove-super-admin")]
        public IHttpActionResult RemoveSuperAdmin(string userId)
        {
            if (_repository.RemoveSuperAdmin(userId))
                return Ok(new { message = "Super Admin removed successfully" });
            return BadRequest("Failed to remove Super Admin");
        }

        [HttpPost]
        [Route("update-record")]
        public IHttpActionResult UpdateRecord(RespondentRecordDTO record)
        {
            if (_repository.UpdateSurveyRecord(record))
                return Ok(new { message = "Record updated successfully" });
            return BadRequest("Failed to update record");
        }

        [HttpGet]
        [Route("communication-metrics")]
        public IHttpActionResult GetCommMetrics(string year, string cycle, string type)
        {
            return Ok(_repository.GetCommunicationMetrics(year, cycle, type));
        }

        [HttpPost]
        [Route("trigger-reminders")]
        public IHttpActionResult TriggerReminders(string year, string cycle, string type, string endDate, string me)
        {
            if (_repository.TriggerReminders(year, cycle, type, endDate, me))
                return Ok(new { message = "Reminders triggered successfully" });
            return BadRequest("Failed to trigger reminders");
        }

        [HttpPost]
        [Route("update-buhr")]
        public IHttpActionResult UpdateBUHR(string pno, string buhrPno, string buhrName, string year, string cycle)
        {
            if (_repository.UpdateBUHR(pno, buhrPno, buhrName, year, cycle))
                return Ok(new { message = "BUHR updated successfully" });
            return BadRequest("Failed to update BUHR");
        }

        [HttpPost]
        [Route("update-approver")]
        public IHttpActionResult UpdateApprover(string pno, string appPno, string year, string cycle)
        {
            if (_repository.UpdateApprover(pno, appPno, year, cycle))
                return Ok(new { message = "Approver updated successfully" });
            return BadRequest("Failed to update approver");
        }

        [HttpPost]
        [Route("bulk-upload-master")]
        public IHttpActionResult BulkUploadMaster(List<EmployeeDTO> employees)
        {
            if (_repository.BulkUploadEmployeeMaster(employees))
                return Ok(new { message = "Master data uploaded successfully" });
            return BadRequest("Failed to upload master data");
        }
    }
}
