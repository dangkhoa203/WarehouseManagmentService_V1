using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Receipt;

namespace WarehouseInventoryManagementAPI.Repository.Receipt_Related_Repository
{
    public class VendorReplenishReceiptRepository : IRepository<VendorReplenishReceipt>
    {
        private readonly ApplicationDbContext _context;
        public VendorReplenishReceiptRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(VendorReplenishReceipt item)
        {
            await _context.VendorReplenishReceipts.AddAsync(item);
        }

        public async Task Delete(string id)
        {
            var receipt = await _context.VendorReplenishReceipts.FindAsync(id);
            if (receipt != null)
            {
                receipt.IsDeleted = true;
                receipt.DeletedAt = DateTime.Now;
            }
        }

        public async Task<VendorReplenishReceipt> GetById(string id)
        {
            return await _context.VendorReplenishReceipts.Include(r => r.ServiceRegisteredFrom).Include(r => r.Details).Include(r=>r.StockImportReport).FirstOrDefaultAsync(r => r.Id == id);
        }

        public IQueryable<VendorReplenishReceipt> getFromDatabase()
        {
            return _context.VendorReplenishReceipts;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task Update(VendorReplenishReceipt item)
        {
            var receipt = await GetById(item.Id);
            if (receipt != null)
            {
                receipt.Vendor = item.Vendor;
                receipt.Details = item.Details;
                receipt.DateOrder = item.DateOrder;
            }
        }
    }
}
