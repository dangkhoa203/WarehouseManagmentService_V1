using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Form;

namespace WarehouseInventoryManagementAPI.Repository.Form_Related_Repository
{
    public class StockImportFormRepository : IRepository<StockImportForm>
    {
        private readonly ApplicationDbContext _context;
        public StockImportFormRepository(ApplicationDbContext context)
        {
            _context= context;
        }
        public async Task Create(StockImportForm item)
        {
            await _context.StockImportReports.AddAsync(item);
        }

        public async Task Delete(string id)
        {
            var form=await GetById(id);
            if(form != null)
            {
                _context.StockImportReports.Remove(form);
            }
        }

        public async Task<StockImportForm> GetById(string id)
        {
            return await _context.StockImportReports
                                 .Include(s => s.Receipt)
                                 .ThenInclude(r=>r.Details)
                                 .Include(s => s.ServiceRegisteredFrom)
                                 .FirstOrDefaultAsync(s => s.Id == id);
        }

        public IQueryable<StockImportForm> getFromDatabase()
        {
            return _context.StockImportReports;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task Update(StockImportForm item)
        {
            var form = await GetById(item.Id);
            if (form != null) {
               form.OrderDate = item.OrderDate;
            }
        }
    }
}
