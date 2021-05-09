using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestWebApi.EmployeeData;
using RestWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private IEmployeeData _employeeData;

        public EmployeesController(IEmployeeData employeeData)
        {
            _employeeData = employeeData;
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            return Ok(_employeeData.GetEmployees());
        }

        [HttpGet("{Id}")]
        public IActionResult GetEmployee(int Id)
        {
            var emp = _employeeData.GetEmployee(Id);
            if(emp!=null)
            {
                return Ok(emp);
            }
            return NotFound($"Employee with id: {Id} not found");

        }

        [HttpPost("{Created}")]
        public IActionResult GetEmployee(Employee employee)
        {
            _employeeData.AddEmployee(employee);

            return Created(HttpContext.Request.Scheme+"://"+HttpContext.Request.Host+HttpContext.Request.Path+"/"+ employee.Id, employee);

        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteEmployee(int id)
        {

            var emp = _employeeData.GetEmployee(id);
            if (emp != null)
            {
                _employeeData.DeleteEmployee(emp);
                return Ok();
            }
            return NotFound($"Employee with id: {id} not found");
        }


        [HttpPatch("{Id}")]
        public IActionResult EditEmployee(int id,Employee employee)
        {

            var existingEmp = _employeeData.GetEmployee(id);
            if (existingEmp != null)
            {
                employee.Id = existingEmp.Id;
                _employeeData.EditEmployee(employee);
             
            }
            return Ok(employee);
        }

    }
}
