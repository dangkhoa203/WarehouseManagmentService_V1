using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Entity.Customer_Entity;

namespace WarehouseInventoryManagementAPI.Repository.Customer_Related_Repository
{
    public class CustomerGroupRepository : IRepository<CustomerGroup>
    {
        private readonly ApplicationDbContext _context;
        public CustomerGroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(CustomerGroup item)
        {
            await _context.CustomerGroups.AddAsync(item);
        }

        public async Task Delete(string id)
        {
            var CustomerGroup = await GetById(id);
            if (CustomerGroup != null)
            {
                foreach (var item in CustomerGroup.Customers)
                {
                    item.CustomerGroup = null;
                }
                _context.CustomerGroups.Remove(CustomerGroup);
            }
        }

        public async Task<CustomerGroup> GetById(string id)
        {
            return await _context.CustomerGroups.Include(g=>g.Customers).FirstOrDefaultAsync(g=>g.Id==id);
        }

        public IQueryable<CustomerGroup> getFromDatabase()
        {
            return _context.CustomerGroups;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task Update(CustomerGroup item)
        {
            var CustomerGroup = await GetById(item.Id);
            if (CustomerGroup != null)
            {
                CustomerGroup.Name = item.Name;
                CustomerGroup.Description = item.Description;
            }
        }
    }
}
