using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Form;

namespace WarehouseInventoryManagementAPI.Repository.Form_Related_Repository
{
    public class ReturnReplenishFormRepository : IRepository<ReturnReplenishForm>
    {
        private readonly ApplicationDbContext _context;
        public ReturnReplenishFormRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(ReturnReplenishForm item)
        {
            await _context.ReturnReplenishForms.AddAsync(item);
        }

        public async Task Delete(string id)
        {
            var form = await GetById(id);
            if (form != null)
            {
                _context.ReturnReplenishForms.Remove(form);
            }
        }

        public async Task<ReturnReplenishForm> GetById(string id)
        {
            return await _context.ReturnReplenishForms
                                 .Include(s => s.Receipt)
                                 .ThenInclude(r => r.Details)
                                 .Include(s => s.ServiceRegisteredFrom)
                                 .FirstOrDefaultAsync(s => s.Id == id);
        }

        public IQueryable<ReturnReplenishForm> getFromDatabase()
        {
            return _context.ReturnReplenishForms;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task Update(ReturnReplenishForm item)
        {
            var form = await GetById(item.Id);
            if (form != null)
            {
                form.OrderDate = item.OrderDate;
            }
        }
    }
}
