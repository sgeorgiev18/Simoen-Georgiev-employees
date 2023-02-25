using Simeon_Georgiev_employees.Models;

namespace Simeon_Georgiev_employees.Contracts
{
    public interface ICSVService
    {
        public List<Employee> calculateDays(List<Employee> employees, List<Row> rows);
        public int DifferenceBetweenRangeOfDays(DateOnly from1, DateOnly from2, DateOnly to1, DateOnly to2);
        public Employee maxDays(List<Employee> employees, List<int> employeesIds);
        public int colleagueMaxDays(Employee e, List<int> employeesIds);
        public OutputEmployees getResult(List<Row> rows);
    }
}
