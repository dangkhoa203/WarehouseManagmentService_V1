using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Entity.Product_Entity;

namespace WarehouseInventoryManagementAPI.Repository.Product_Related_Repository
{
    public class ProductGroupRepository : IRepository<ProductGroup>
    {
        private readonly ApplicationDbContext _context;
        public ProductGroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(ProductGroup item)
        {
            await _context.ProductGroups.AddAsync(item);
        }

        public async Task Delete(string id)
        {
            var Group = await GetById(id);
            if (Group != null)
            {
                foreach (var item in Group.Products)
                {
                    item.ProductGroup = null;
                }
                _context.ProductGroups.Remove(Group);
            }
        }

        public async Task<ProductGroup> GetById(string id)
        {
            return await _context.ProductGroups.Include(g=>g.Products).FirstOrDefaultAsync(c => c.Id == id);
        }

        public IQueryable<ProductGroup> getFromDatabase()
        {
           return _context.ProductGroups;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task Update(ProductGroup item)
        {
            var Group = await GetById(item.Id);
            if (Group != null)
            {
                Group.Name = item.Name;
                Group.Description = item.Description;
            }
        }
    }
}
