using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Entity;

namespace WarehouseInventoryManagementAPI.Repository.Warehouse_Related_Repository
{
    public class StockRepository : IRepository<Stock>,IImportExportRepository
    {
        private readonly ApplicationDbContext _context;
        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Stock item)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.ProductId == item.ProductId);
            if (stock != null)
            {
                stock.Quantity += item.Quantity;
            }
            else
            {
                await _context.Stocks.AddAsync(item);
            }
        }

        public async Task<bool> CheckQuantity(string Id, int Quantity)
        {
            var stock = await GetById(Id);
            if (stock == null) return false;
            if (stock.Quantity < Quantity) return false;
            return true;
        }

        public async Task Create(Stock item)
        {
           await _context.Stocks.AddAsync(item);
        }

        public async Task Delete(string id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock != null)
            {
                _context.Stocks.Remove(stock);
            }
        }

        public async Task<Stock> GetById(string id)
        {
            return await _context.Stocks.Include(s => s.ProductNav).FirstOrDefaultAsync(s => s.ProductId == id);
        }

        public IQueryable<Stock> getFromDatabase()
        {
            return _context.Stocks;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> Subtract(Stock item)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.ProductId == item.ProductId);
            if (stock != null && item.Quantity<=stock.Quantity)
            {
                stock.Quantity -= item.Quantity;
                return true;
            }
            return false;
        }

        public async Task Update(Stock item)
        {
            var Stock = await GetById(item.ProductId);
            if (Stock != null)
            {
                Stock.Quantity = item.Quantity;
            }
        }
    }
}
