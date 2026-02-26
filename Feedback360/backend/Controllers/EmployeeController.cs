using System.Web.Http;
using feedback360.Backend.DTOs;
using feedback360.Backend.Repositories;

namespace feedback360.Backend.Controllers
{
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeController()
        {
            _repository = new EmployeeRepository();
        }

        [HttpGet]
        [Route("lookups")]
        public IHttpActionResult GetLookups(string adminId, bool isSuperAdmin)
        {
            return Ok(new {
                Departments = _repository.GetDepartments(adminId, isSuperAdmin),
                ExecHeads = _repository.GetExecHeads(adminId, isSuperAdmin),
                SubAreas = _repository.GetSubAreas(adminId, isSuperAdmin),
                Designations = _repository.GetDesignations(adminId, isSuperAdmin)
            });
        }

        [HttpGet]
        [Route("search")]
        public IHttpActionResult Search(string prefix)
        {
            return Ok(_repository.SearchEmployees(prefix));
        }

        [HttpGet]
        [Route("details")]
        public IHttpActionResult GetDetails(string perno, string year, string cycle)
        {
            var emp = _repository.GetEmployee(perno, year, cycle);
            if (emp != null) return Ok(emp);
            return NotFound();
        }

        [HttpPost]
        [Route("save")]
        public IHttpActionResult Save(EmployeeDTO employee)
        {
            if (_repository.SaveEmployee(employee))
                return Ok(new { message = "Employee saved successfully" });
            return BadRequest("Failed to save employee");
        }

        [HttpPost]
        [Route("update")]
        public IHttpActionResult Update(EmployeeDTO employee)
        {
            if (_repository.UpdateEmployee(employee))
                return Ok(new { message = "Employee updated successfully" });
            return BadRequest("Failed to update employee");
        }
    }
}
