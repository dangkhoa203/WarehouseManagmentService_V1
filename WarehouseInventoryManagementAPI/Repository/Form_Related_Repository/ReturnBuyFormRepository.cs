using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Form;

namespace WarehouseInventoryManagementAPI.Repository.Form_Related_Repository
{
    public class ReturnBuyFormRepository:IRepository<ReturnBuyForm>
    {
        private readonly ApplicationDbContext _context;
        public ReturnBuyFormRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(ReturnBuyForm item)
        {
            await _context.ReturnBuyForm.AddAsync(item);
        }

        public async Task Delete(string id)
        {
            var form = await GetById(id);
            if (form != null)
            {
                _context.ReturnBuyForm.Remove(form);
            }
        }

        public async Task<ReturnBuyForm> GetById(string id)
        {
            return await _context.ReturnBuyForm
                                 .Include(s => s.Receipt)
                                 .ThenInclude(r => r.Details)
                                 .Include(s => s.ServiceRegisteredFrom)
                                 .FirstOrDefaultAsync(s => s.Id == id);
        }

        public IQueryable<ReturnBuyForm> getFromDatabase()
        {
            return _context.ReturnBuyForm;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task Update(ReturnBuyForm item)
        {
            var form = await GetById(item.Id);
            if (form != null)
            {
                form.OrderDate = item.OrderDate;
            }
        }
    }
}
