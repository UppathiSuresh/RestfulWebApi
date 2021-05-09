using RestWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebApi.EmployeeData
{
   public interface IEmployeeData
    {
        List<Employee> GetEmployees();

        Employee GetEmployee(int Id);

        Employee AddEmployee(Employee employee);

        void DeleteEmployee(Employee employee);

        Employee EditEmployee(Employee employee);
    }
}
