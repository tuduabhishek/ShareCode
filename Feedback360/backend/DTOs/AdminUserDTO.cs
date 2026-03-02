namespace feedback360.Backend.DTOs
{
    public class AdminUserDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Mode { get; set; } // I or E
        public string UserType { get; set; } // I or E
        public string Designation { get; set; }
        public string Department { get; set; }
    }
}
