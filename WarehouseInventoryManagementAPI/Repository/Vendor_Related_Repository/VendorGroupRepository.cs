using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_EntiTy;

namespace WarehouseInventoryManagementAPI.Repository.Vendor_Related_Repository
{
    public class VendorGroupRepository : IRepository<VendorGroup>
    {
        private readonly ApplicationDbContext _context;
        public VendorGroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(VendorGroup item)
        {
            await _context.VendorGroups.AddAsync(item);
        }

        public async Task Delete(string id)
        {
            var Group = await GetById(id);
            if (Group != null)
            {
                foreach(var item in Group.Vendors)
                {
                    item.VendorGroup = null;
                }
                _context.VendorGroups.Remove(Group);
            }
        }

        public async Task<VendorGroup> GetById(string id)
        {
            return await _context.VendorGroups.Include(g=>g.ServiceRegisteredFrom).Include(g=>g.Vendors).FirstOrDefaultAsync(c => c.Id == id);
        }

        public IQueryable<VendorGroup> getFromDatabase()
        {
           return _context.VendorGroups;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task Update(VendorGroup item)
        {
            var Group = await _context.VendorGroups.FindAsync(item.Id);
            if (Group != null)
            {
                Group.Name=item.Name;
                Group.Description = item.Description;
            }
        }
    }
}
