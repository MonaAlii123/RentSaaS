using employee.Models;

namespace employee.Repositries.IModelRepositry
{
    public interface IEmployeeRepositry
    {
        Task<IEnumerable<Employees>> GetAll();
        Task<Employees> GetById(int id);
        Task Create(Employees employee);
        Task Update(Employees employee);
        Task Delete(int id);
        Task SaveDB();
    }
}
