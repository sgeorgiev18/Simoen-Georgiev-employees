namespace Simeon_Georgiev_employees.Models
{
    public class RowDto
    {
        public int EmpId { get; set; }
        public int ProjectId { get; set; }
        public DateOnly DateFrom { get; set; }
        public String DateTo { get; set; }
    }
}
