using System.Reflection.Metadata.Ecma335;

namespace Simeon_Georgiev_employees.Models
{
    public class OutputEmployees
    {
        public int EmployeeId1 { get; set; }
        public int EmployeeId2 { get; set; }
        public Dictionary<int, int> Projects { get; set; }

        public OutputEmployees() 
        {
            Projects = new Dictionary<int, int>();
        }
    }

}
