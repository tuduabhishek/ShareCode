using System.Web.Http;
using feedback360.Backend.DTOs;
using feedback360.Backend.Repositories;

namespace feedback360.Backend.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly IUserRepository _repository;

        public UserController()
        {
            _repository = new UserRepository();
        }

        [HttpGet]
        [Route("session")]
        public IHttpActionResult GetSession()
        {
            // In a real app, this would get the user ID from the OAuth token
            // For now, using a placeholder or a header
            var userId = Request.Headers.Contains("X-User-ID") 
                         ? ((System.Collections.Generic.List<string>)Request.Headers.GetValues("X-User-ID"))[0] 
                         : "SYSTEM";
            
            var session = _repository.GetUserSession(userId);
            return Ok(session);
        }
    }
}
