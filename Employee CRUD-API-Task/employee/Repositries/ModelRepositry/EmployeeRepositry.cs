using employee.Models;
using employee.Repositries.IModelRepositry;
using Microsoft.EntityFrameworkCore;

namespace employee.Repositries.ModelRepositry
{
    public class EmployeeRepositry : IEmployeeRepositry
    {
        private RentSaaSContext context;
        public EmployeeRepositry (RentSaaSContext context)
        {
            this.context = context;
        }

        #region GetAll
        public async Task<IEnumerable<Employees>> GetAll()
        {
            return await context.Employees.ToListAsync();
        }
        #endregion
        
        #region GetById
        public async Task<Employees> GetById(int id)
        {
            return await context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        }
        #endregion

        #region Create
        public async Task Create(Employees employee)
        {
            await context.Employees.AddAsync(employee);
        }
        #endregion

        #region Delete
        public async Task Delete(int id)
        {
            await context.Employees
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync();
        }
        #endregion

        #region Update
        public async Task Update(Employees employee)
        {
            context.Employees.Update(employee);
        }
        #endregion

        #region SaveDB
        public async Task SaveDB()
        {
            await context.SaveChangesAsync();
        }
        #endregion
    }
}
