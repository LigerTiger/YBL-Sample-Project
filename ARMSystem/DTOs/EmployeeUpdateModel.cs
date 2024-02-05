namespace ARMSystem.DTOs
{
    public class EmployeeUpdateModel
    {
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "Existing User";
        public bool IsActive { get; set; }
        public string EmpAdId { get; set; }
        public string Type { get; set; }
    }
}
