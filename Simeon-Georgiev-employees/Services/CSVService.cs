using Simeon_Georgiev_employees.Contracts;
using Simeon_Georgiev_employees.Models;

namespace Simeon_Georgiev_employees.Services
{
    public class CSVService : ICSVService
    {
        public OutputEmployees getResult(List<Row> rows)
        {
            List<int> ids = new List<int>();

            foreach (var record in rows)
            {
                if (!ids.Contains(record.EmpId))
                {
                    ids.Add(record.EmpId);
                }
            }
            List<Employee> employees = new List<Employee>();
            foreach (int id in ids)
            {
                Employee e = new Employee();
                foreach (int coleagueId in ids)
                {
                    if (coleagueId != id)
                    {
                        e.EmployeesWorkedWithRecords.Add(coleagueId, 0);
                    }
                }
                e.EmpId = id;
                employees.Add(e);
            }
            List<int> projectIds = new List<int>();
            foreach (Row row in rows)
            {
                if (!projectIds.Contains(row.ProjectId))
                {
                    projectIds.Add(row.ProjectId);
                }
            }
            List<Row> currentProject = new List<Row>();
            List<Employee> employeesInProject = new List<Employee>();
            for (int i = 0; i < projectIds.Count; i++)
            {
                currentProject = rows.Where(x => x.ProjectId == projectIds[i]).ToList();
                List<int> employeesInProjectIds = new List<int>();
                foreach (var proj in currentProject)
                {
                    if (!employeesInProjectIds.Contains(proj.EmpId))
                    {
                        employeesInProjectIds.Add(proj.EmpId);
                    }
                }

                for (int j = 0; j < employeesInProjectIds.Count; j++)
                {
                    bool contains = false;
                    foreach (var e in employeesInProject)
                    {
                        if (e.EmpId == employeesInProjectIds[j])
                        {
                            contains = true;
                        }
                    }
                    if (!contains)
                    {
                        employeesInProject.Add(employees.First(x => x.EmpId == employeesInProjectIds[j]));
                    }
                }

                List<Employee> result = new List<Employee>();
                result = calculateDays(employeesInProject, currentProject);
                foreach (var e in result)
                {
                    employees.First(x => x.EmpId == e.EmpId).EmployeesWorkedWithRecords = e.EmployeesWorkedWithRecords;
                }
            }
            List<int> employeesIds = new List<int>();
            foreach (var employee in employees)
            {
                if (!employeesIds.Contains(employee.EmpId))
                {
                    employeesIds.Add(employee.EmpId);
                }
            }
            Employee employeeMaxDays = maxDays(employees, employeesIds);
            int value = colleagueMaxDays(employeeMaxDays, employeesIds);
            int key = employeeMaxDays.EmployeesWorkedWithRecords.FirstOrDefault(x => x.Value == value).Key;
            List<Row> employee1Projects = rows.Where(x => x.EmpId == employeeMaxDays.EmpId).ToList();
            List<Row> employee2Projects = rows.Where(x => x.EmpId == key).ToList();
            List<int> commonProjectsIds = new List<int>();
            foreach (var project in employee1Projects)
            {
                foreach (var project2 in employee2Projects)
                {
                    if (project.ProjectId == project2.ProjectId)
                    {
                        if (!commonProjectsIds.Contains(project2.ProjectId))
                        {
                            commonProjectsIds.Add(project2.ProjectId);
                        }
                    }
                }
            }
            OutputEmployees output = new OutputEmployees();
            output.EmployeeId1 = employeeMaxDays.EmpId;
            output.EmployeeId2 = key;
            foreach (int id in commonProjectsIds)
            {
                Row p1 = new Row();
                Row p2 = new Row();
                List<Row> currProject = rows.Where(x => x.ProjectId == id).ToList();

                foreach (var project in currProject)
                {
                    if (project.EmpId == output.EmployeeId1)
                    {
                        p1.DateFrom = project.DateFrom;
                        p1.DateTo = project.DateTo;
                    }
                    if (project.EmpId == output.EmployeeId2)
                    {
                        p2.DateFrom = project.DateFrom;
                        p2.DateTo = project.DateTo;
                    }
                }


                int days = DifferenceBetweenRangeOfDays(p1.DateFrom, p2.DateFrom, p1.DateTo, p2.DateTo);
                output.Projects.Add(id, days);
            }
            return output;
        }



        public int colleagueMaxDays(Employee e, List<int> employeesIds)
        {
            int maxDays = 0;
            foreach (int id in employeesIds)
            {
                if (e.EmpId != id)
                {
                    if (e.EmployeesWorkedWithRecords[id] > maxDays)
                    {
                        maxDays = e.EmployeesWorkedWithRecords[id];
                    }
                }
            }
            return maxDays;
        }
        public Employee maxDays(List<Employee> employees, List<int> employeesIds)
        {
            Employee max = new Employee();
            int maxDays = employees[0].EmployeesWorkedWithRecords[employeesIds[1]];
            foreach (Employee e in employees)
            {
                foreach (int id in employeesIds)
                {
                    if (id != e.EmpId)
                    {
                        if (e.EmployeesWorkedWithRecords[id] > maxDays)
                        {
                            maxDays = e.EmployeesWorkedWithRecords[id];
                            max = e;
                        }
                    }
                }
            }
            return max;
        }
        public int DifferenceBetweenRangeOfDays(DateOnly from1, DateOnly from2, DateOnly to1, DateOnly to2)
        {
            if (to1 == null)
            {
                to1 = DateOnly.FromDateTime(DateTime.Now);
            }
            if (to2 == null)
            {
                to2 = DateOnly.FromDateTime(DateTime.Now);
            }
            DateOnly soonerFrom;
            DateOnly soonerTo;
            if (from1 > from2)
            {
                soonerFrom = from1;
            }
            else
            {
                soonerFrom = from2;
            }

            if (to1 > to2)
            {
                soonerTo = to1;
            }
            else
            {
                soonerTo = to2;
            }
            return (soonerTo.DayNumber - soonerFrom.DayNumber);

        }

        public List<Employee> calculateDays(List<Employee> employees, List<Row> rows)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = i + 1; j < rows.Count; j++)
                {
                    if (rows[i].EmpId != rows[j].EmpId)
                    {
                        int res = DifferenceBetweenRangeOfDays(rows[i].DateFrom, rows[j].DateFrom, rows[i].DateTo, rows[j].DateTo);
                        employees.First(x => x.EmpId == rows[i].EmpId).EmployeesWorkedWithRecords[rows[j].EmpId] += res;
                        employees.First(x => x.EmpId == rows[j].EmpId).EmployeesWorkedWithRecords[rows[i].EmpId] += res;
                    }
                }
            }
            return employees;
        }
    }
}
