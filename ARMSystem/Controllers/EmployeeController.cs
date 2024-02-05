using ARMSystem.Data;
using ARMSystem.DTOs;
using ARMSystem.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ARMSystem.Properties
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly DataContext _context;

        public EmployeeController(DataContext context)
        {
            _context = context;
        }


        // GET: api/Employee/Type/EmpAdId

        [HttpGet("TypeAndEmpID")]
        public async Task<ActionResult<Employee>> GetEmployee(string type, string empAdId)
        {
            var employee = await _context.Employees
                .Where(e => e.Type == type && e.EmpAdId == empAdId)
                .Select(e => new
                {
                    e.EmployeeName,
                    e.BusinessUnit,
                    e.CorporateDesignation,
                    e.FunctionalDesignation,
                    e.IsActive
                })
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost("SrNo")]
        public async Task<ActionResult<Employee>> PostEmployee([FromBody] EmployeeCreateDTO employeeDTO)
        {
            // Check if the employee already exists in the database
            var existingEmployee = _context.Employees
                .FirstOrDefault(x => x.EmpAdId == employeeDTO.EmpAdId && x.Type == employeeDTO.Type);

            if (existingEmployee != null)
            {
                // Employee with the same EmpAdId and Type already exists
                return Conflict($"Employee with ID {employeeDTO.EmpAdId} and type {employeeDTO.Type} already exists.");
            }

            // Map the DTO to the actual Employee entity
            var employee = new Employee
            {
                EmpAdId = employeeDTO.EmpAdId,
                EmployeeName = employeeDTO.EmployeeName,
                BusinessUnit = employeeDTO.BusinessUnit,
                CorporateDesignation = employeeDTO.CorporateDesignation,
                FunctionalDesignation = employeeDTO.FunctionalDesignation,
                IsActive = employeeDTO.IsActive,
                Type = employeeDTO.Type
                // Other properties will be handled by the backend (createdOn, createdBy, modifiedOn, modifiedBy)
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { type = employee.Type, empAdId = employee.EmpAdId }, employee);
        }

        // 1

        // Update Method in same records not going to update in next srno.

        [HttpPut("TypeAndEmpAdIdUpdate")]
        public async Task<IActionResult> UpdateEmployee(string type, string empAdId, [FromBody] EmployeeUpdateModel updateModel)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Type == type && e.EmpAdId == empAdId);

            if (employee == null)
            {
                return NotFound();
            }

            // Update the fields from the update model

            employee.ModifiedOn = updateModel.ModifiedOn;
            employee.ModifiedBy = updateModel.ModifiedBy;
            employee.IsActive = updateModel.IsActive;
            employee.Type = updateModel.Type;

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(type, empAdId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }


        [HttpGet("empAdId")]
        public async Task<ActionResult<Employee>> GetEmployee(string empAdId)
        {
            var employee = await _context.Employees
                .Where(e => e.EmpAdId == empAdId)
                .Select(e => new
                {
                    e.EmployeeName,
                    e.BusinessUnit,
                    e.CorporateDesignation,
                    e.FunctionalDesignation,
                    e.IsActive,
                    e.Type
                })
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpGet("Verify")]
        // [HttpGet]
        public IActionResult VerifyMethod([FromQuery] string empAdid, [FromQuery] string type)
        {
            var employee = _context.Employees
               .FirstOrDefault(x => x.EmpAdId == empAdid && x.Type == type);

            // .Where(x => x.EmpAdId == empAdid && x.Type == type).ToList(); 

            if (employee == null)
            {
                return NotFound($"Employee with ID {empAdid} and type {type} not found.");
            }

            return Ok(employee);

        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployee()
        {
            var data = await _context.Employees.ToListAsync();
            return Ok(data);

        }



        [HttpDelete("TypeAndEmpAdId")]
        public async Task<IActionResult> DeleteEmployee(string type, string empAdId)
        {
            var employeesToDelete = await _context.Employees
                .Where(e => e.Type == type && e.EmpAdId == empAdId)
                .ToListAsync();

            if (!employeesToDelete.Any())
            {
                return NotFound();
            }

            _context.Employees.RemoveRange(employeesToDelete);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool EmployeeExists(string type, string empAdId)
        {
            return _context.Employees.Any(e => e.Type == type && e.EmpAdId == empAdId);
        }



    }
}
