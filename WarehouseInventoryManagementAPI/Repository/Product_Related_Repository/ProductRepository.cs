using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Entity.Customer_Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Product_Entity;

namespace WarehouseInventoryManagementAPI.Repository.Product_Related_Repository
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(Product item)
        {
            await _context.Products.AddAsync(item);
        }

        public async Task Delete(string id)
        {
            var Product = await _context.Products.FindAsync(id);
            if (Product != null)
            {
                Product.DeletedAt = DateTime.Now;
                Product.IsDeleted = true;

            }
        }

        public async Task<Product> GetById(string id)
        {
            return await _context.Products.Include(p=>p.ProductGroup).FirstOrDefaultAsync(c => c.Id == id);
        }

        public IQueryable<Product> getFromDatabase()
        {
            return _context.Products;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task Update(Product item)
        {
            var Product = await GetById(item.Id);
            if (Product != null)
            {
                Product.Name = item.Name;
                Product.MeasureUnit = item.MeasureUnit;
                Product.PricePerUnit = item.PricePerUnit;
                Product.ProductGroup = item.ProductGroup;
            }
        }
    }
}
