using employee.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using employee.DTOs;
using employee.Services.IModelService;
using employee.Services.ModelService;
namespace employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeService employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        #region Get All [Pagination and Search]
        [HttpGet]
        public async Task<ActionResult> GetPaginatedEmployees([FromQuery] string? SearchTxt, int PageSize = 10, int PageNumber = 1)
        {
            try
            {
                var employeesDTO = await employeeService.PaginationAndSearch(SearchTxt, PageSize, PageNumber);
                return Ok(employeesDTO);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Not Found!")
                {
                    return NotFound(new { Success = false, Message = "Not Found!" });
                }

                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }
        #endregion

        #region Get By Id Action
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var employeeDTO = await employeeService.GetById(id);
                return Ok(employeeDTO);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Not Found!")
                {
                    return NotFound(new { Success = false, Message = "Not Found!" });
                }

                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }
        #endregion

        #region Create Action
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(new { Success = false, Message = "DTO Error!." });
            }
            try
            {
                await employeeService.Create(dto);
                return Ok(new { Success = true, Message = "Created Successfully!." });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Not Found!")
                    return NotFound(new { Success = false, Message = "Not Found!" });

                else if (ex.Message == "Email already exists!.")
                    return BadRequest(new { Success = false, Message = "Email already exists!." });
                
                else if (ex.Message == "Employee with the same name already exists!")
                    return BadRequest(new { Success = false, Message = "Employee with the same name already exists!." });
                
                else
                    return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }
        #endregion

        #region Update Action
        [HttpPut]
        public async Task<IActionResult> Update(EmployeeGetUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(new { Success = false, Message = "DTO Error!." });
            }
            try
            {
                await employeeService.Update(dto);
                return Ok(new { Success = true, Message = "Updated Successfully!" });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Not Found!")
                    return NotFound(new { Success = false, Message = "Not Found!" });

                else if (ex.Message.Contains("name"))
                    return BadRequest(new { Success = false, Message = "An employee with the same name already exists." });

                else if (ex.Message.Contains("Email"))
                    return BadRequest(new { Success = false, Message = "Email already exists." });

                else
                    return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }
        #endregion

        #region Delete Action
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await employeeService.Delete(id);
                return Ok(new { Success = true, Message = "Deleted Successfully!." });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Not Found!")
                    return NotFound(new { Success = false, Message = "Not Found!" });

                else
                    return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }
        #endregion
    }
}


