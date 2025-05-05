using employee.DTOs;
using employee.Models;

namespace employee.Services.IModelService
{
    public interface IEmployeeService
    {
        Task<PaginationDTO<EmployeeGetUpdateDTO>> PaginationAndSearch(string? searchTxt, int pageSize = 10, int PageNumber = 1);
        Task<EmployeeGetUpdateDTO> GetById(int id);
        Task Create(EmployeeCreateDTO dto);
        Task Delete(int id);
        Task Update(EmployeeGetUpdateDTO dto);
    }
}
