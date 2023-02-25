namespace Simeon_Georgiev_employees.Models
{
    public class Employee
    {
        public int EmpId { get; set; }
        public Dictionary<int, int> EmployeesWorkedWithRecords { get; set; }

        public Employee() 
        { 
            EmployeesWorkedWithRecords= new Dictionary<int, int>();
        }
    }
}
