using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Receipt;

namespace WarehouseInventoryManagementAPI.Repository.Receipt_Related_Repository
{
    public class CustomerBuyReceiptRepository : IRepository<CustomerBuyReceipt>
    {
        private readonly ApplicationDbContext _context;
        public CustomerBuyReceiptRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(CustomerBuyReceipt item)
        {
            await _context.CustomerBuyReceipts.AddAsync(item);
        }

        public async Task Delete(string id)
        {
            var receipt = await _context.CustomerBuyReceipts.FindAsync(id);
            if (receipt != null)
            {
                receipt.IsDeleted = true;
                receipt.DeletedAt = DateTime.Now;
            }
        }

        public async Task<CustomerBuyReceipt> GetById(string id)
        {
            return await _context.CustomerBuyReceipts.Include(r => r.ServiceRegisteredFrom).Include(r => r.Details).FirstOrDefaultAsync(r=>r.Id==id);
        }

        public IQueryable<CustomerBuyReceipt> getFromDatabase()
        {
            return _context.CustomerBuyReceipts;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task Update(CustomerBuyReceipt item)
        {
            var receipt = await GetById(item.Id);
            if (receipt != null)
            {
                receipt.Customer=item.Customer;
                receipt.Details=item.Details;
                receipt.DateOrder = item.DateOrder;
            }
        }
    }
}
