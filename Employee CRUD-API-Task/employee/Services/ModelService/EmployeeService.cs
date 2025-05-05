using employee.DTOs;
using employee.Models;
using employee.Repositries.IModelRepositry;
using employee.Services.IModelService;
namespace employee.Services.ModelService
{
    public class EmployeeService : IEmployeeService
    {
        IEmployeeRepositry employeeRepositry; 
        public EmployeeService(IEmployeeRepositry employeeRepository)  
        {
            this.employeeRepositry = employeeRepository;
        }

        #region Get All By Pagination
        public async Task<PaginationDTO<EmployeeGetUpdateDTO>> PaginationAndSearch(string? SearchTxt, int PageSize = 10, int PageNumber = 1)
        {
            var employees = await employeeRepositry.GetAll();
            if (employees == null || !employees.Any()) throw new Exception("Not Found!");
            else
            {
                if (!string.IsNullOrEmpty(SearchTxt))
                {
                    // Searching by name or email or position
                    employees = employees
                        .Where(item =>
                            (item.FirstName?.Contains(SearchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                            (item.LastName?.Contains(SearchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                            (item.Email?.Contains(SearchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                            (item.Position?.Contains(SearchTxt, StringComparison.OrdinalIgnoreCase) ?? false)
                        )
                        .ToList();

                    if (!employees.Any()) throw new Exception("Not Found!");
                }

                var totalEmployees = employees.Count();

                // Pagination
                var paginatedEmployees = employees
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .Select(e => new EmployeeGetUpdateDTO
                    {
                        Id = e.Id,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Email = e.Email,
                        Position = e.Position
                    })
                    .ToList();


                PaginationDTO<EmployeeGetUpdateDTO> result = new PaginationDTO<EmployeeGetUpdateDTO>
                {
                    entitiesNumber = totalEmployees,
                    pageNumber = PageNumber,
                    pageSize = PageSize,
                    entites = paginatedEmployees
                };

                return result;
            }
        }
        #endregion

        #region GetByID
        public async Task<EmployeeGetUpdateDTO> GetById(int id)
        {
            var employee = await employeeRepositry.GetById(id);
            if (employee == null) throw new Exception("Not Found!");

            return new EmployeeGetUpdateDTO
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Position = employee.Position
            };
        }
        #endregion

        #region Create
        public async Task Create(EmployeeCreateDTO dto)
        {
            var existingEmployees = await employeeRepositry.GetAll();

            bool isEmailExists = existingEmployees.Any(e =>
                e.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase));

            bool isNameExists = existingEmployees.Any(e =>
                e.FirstName.Equals(dto.FirstName, StringComparison.OrdinalIgnoreCase) &&
                e.LastName.Equals(dto.LastName, StringComparison.OrdinalIgnoreCase));

            if (isEmailExists)
                throw new Exception("Email already exists!");

            if (isNameExists)
                throw new Exception("Employee with the same name already exists!");

            var employee = new Employees
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Position = dto.Position
            };

            await employeeRepositry.Create(employee);
            await employeeRepositry.SaveDB();
        }
        #endregion

        #region Update
        public async Task Update(EmployeeGetUpdateDTO dto)
        {
            var employee = await employeeRepositry.GetById(dto.Id);
            if (employee == null)
                throw new Exception("Not Found!");

            var allEmps = await employeeRepositry.GetAll();
            var existingEmployeeWithName = allEmps.FirstOrDefault(e => e.FirstName == dto.FirstName && e.LastName == dto.LastName && e.Id != dto.Id);
            var existingEmployeeWithEmail = allEmps.FirstOrDefault(e => e.Email == dto.Email && e.Id != dto.Id);

            if (existingEmployeeWithName != null)
                throw new Exception("An employee with the same name already exists.");

            if (existingEmployeeWithEmail != null)
                throw new Exception("Email already exists.");

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Email = dto.Email;
            employee.Position = dto.Position;

            employeeRepositry.Update(employee);
            await employeeRepositry.SaveDB();
        }
        #endregion

        #region Delete
        public async Task Delete(int id)
        {
            var employee = await employeeRepositry.GetById(id);
            if (employee == null)
                throw new Exception("Not Found!");

            await employeeRepositry.Delete(id);
            await employeeRepositry.SaveDB();
        }
        #endregion
    }
}
