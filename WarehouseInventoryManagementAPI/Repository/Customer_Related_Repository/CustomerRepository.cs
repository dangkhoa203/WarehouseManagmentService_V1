using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Entity.Customer_Entity;

namespace WarehouseInventoryManagementAPI.Repository.Customer_Related_Repository
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly ApplicationDbContext _context;
        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(Customer item)
        {
            await _context.Customers.AddAsync(item);
        }

        public async Task Delete(string id)
        {
            var Customer = await _context.Customers.FindAsync(id);
            if (Customer != null)
            {
                Customer.DeletedAt = DateTime.Now;
                Customer.IsDeleted = true;

            }
        }

        public async Task<Customer> GetById(string id)
        {
            return await _context.Customers.Include(c => c.CustomerGroup).FirstOrDefaultAsync(c => c.Id == id);
        }

        public IQueryable<Customer> getFromDatabase()
        {
            return _context.Customers;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task Update(Customer item)
        {
            var Customer = await GetById(item.Id);
            if (Customer != null)
            {
                Customer.Name = item.Name;
                Customer.Email = item.Email;
                Customer.Address = item.Address;
                Customer.PhoneNumber = item.PhoneNumber;
                if (item.CustomerGroup != null)
                    Customer.CustomerGroup = item.CustomerGroup;
            }
        }
    }
}
