using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_EntiTy;

namespace WarehouseInventoryManagementAPI.Repository.Vendor_Related_Repository
{
    public class VendorRepository : IRepository<Vendor>
    {
        private readonly ApplicationDbContext _context;
        public VendorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(Vendor item)
        {
            await _context.Vendors.AddAsync(item);
        }

        public async Task Delete(string id)
        {
            var Vendor = await _context.Vendors.FindAsync(id);
            if (Vendor != null)
            {
                Vendor.DeletedAt = DateTime.Now;
                Vendor.IsDeleted = true;
            }
        }

        public async Task<Vendor> GetById(string id)
        {
            return await _context.Vendors.Include(v=>v.VendorGroup).FirstOrDefaultAsync(c => c.Id == id);
        }

        public IQueryable<Vendor> getFromDatabase()
        {
            return _context.Vendors;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task Update(Vendor item)
        {
            var Vendor = await GetById(item.Id);
            if (Vendor != null)
            {
                Vendor.Email = item.Email;
                Vendor.Name = item.Name;
                Vendor.Address = item.Address;
                Vendor.PhoneNumber = item.PhoneNumber;
                Vendor.VendorGroup = item.VendorGroup;
            }
        }
    }
}
