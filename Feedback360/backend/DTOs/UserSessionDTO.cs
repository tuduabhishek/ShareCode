using System;

namespace feedback360.Backend.DTOs
{
    public class UserSessionDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FiscalYear { get; set; }
        public string Cycle { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string Role { get; set; }
    }

    public class LoginRequestDTO
    {
        public string UserId { get; set; }
    }
}
