using RestWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebApi.EmployeeData
{
    public class MockEmployeeData : IEmployeeData
    {
        private List<Employee> employees = new List<Employee>()
        {
            new Employee()
            {
                Id=1,
                Name="Employee one"
            },

            new Employee()
            {
                Id=2,
                Name="Employee two"
            }
        };

        public Employee AddEmployee(Employee employee)
        {
         employees.Add(employee);
            return employee;
        }

        public void DeleteEmployee(Employee employee)
        {
            employees.Remove(employee);
        }

        public Employee EditEmployee(Employee employee)
        {
            var existingEmploy = GetEmployee(employee.Id);
            existingEmploy.Name = employee.Name;
            return existingEmploy;
        }

        public Employee GetEmployee(int Id)
        {
            //throw new NotImplementedException();
            return employees.SingleOrDefault(x => x.Id == Id);
        }

        public List<Employee> GetEmployees()
        {
            return employees;
        }
    }
}
