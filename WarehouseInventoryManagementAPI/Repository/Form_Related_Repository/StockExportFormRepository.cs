using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Form;

namespace WarehouseInventoryManagementAPI.Repository.Form_Related_Repository
{
    public class StockExportFormRepository : IRepository<StockExportForm>
    {
        private readonly ApplicationDbContext _context;
        public StockExportFormRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(StockExportForm item)
        {
            await _context.StockExportReports.AddAsync(item);
        }

        public async Task Delete(string id)
        {
            var form = await GetById(id);
            if (form != null)
            {
                _context.StockExportReports.Remove(form);
            }
        }

        public async Task<StockExportForm> GetById(string id)
        {
            return await _context.StockExportReports
                                 .Include(s => s.Receipt)
                                 .ThenInclude(r => r.Details)
                                 .Include(s => s.ServiceRegisteredFrom)
                                 .FirstOrDefaultAsync(s => s.Id == id);
        }

        public IQueryable<StockExportForm> getFromDatabase()
        {
            return _context.StockExportReports;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task Update(StockExportForm item)
        {
            var form = await GetById(item.Id);
            if (form != null)
            {
                form.OrderDate = item.OrderDate;
            }
        }
    }
}
