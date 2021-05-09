using RestWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebApi.EmployeeData
{
    public class SqlEmployeeData : IEmployeeData
    {
        private EmployeeContext _employeeContext;
        public SqlEmployeeData(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }
        public Employee AddEmployee(Employee employee)
        {
            _employeeContext.Employees.Add(employee);
            _employeeContext.SaveChanges();
            return employee;
        }

        public void DeleteEmployee(Employee employee)
        {
            _employeeContext.Remove(employee);
            _employeeContext.SaveChanges();
        }

        public Employee EditEmployee(Employee employee)
        {
            var existingEmploy = _employeeContext.Employees.Find(employee.Id);
            if (existingEmploy != null)
            {
                existingEmploy.Name = employee.Name;
                _employeeContext.Employees.Update(existingEmploy);
                _employeeContext.SaveChanges();
            }
            return employee;
        }

        public Employee GetEmployee(int Id)
        {
            var employee = _employeeContext.Employees.Find(Id);
            return employee;
        }

        public List<Employee> GetEmployees()
        {
            return _employeeContext.Employees.ToList();

        }
    }
}
