namespace Simeon_Georgiev_employees.Models
{
    public class Row
    {
        public int EmpId { get; set; }
        public int ProjectId { get; set; }
        public DateOnly DateFrom { get; set; }
        public DateOnly DateTo { get; set; }
    }
}
