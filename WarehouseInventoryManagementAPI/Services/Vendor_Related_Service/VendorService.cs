using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.DTO.Customer.Show;
using WarehouseInventoryManagementAPI.DTO.Vendor.Change;
using WarehouseInventoryManagementAPI.DTO.Vendor.Show;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_EntiTy;
using WarehouseInventoryManagementAPI.Repository;
using WarehouseInventoryManagementAPI.Repository.Customer_Related_Repository;
using WarehouseInventoryManagementAPI.Services.Interface;

namespace WarehouseInventoryManagementAPI.Services.Vendor_Related_Service
{
    public class VendorService : IDataService<Vendor, NewVendorDTO, ShowVendorDTO>
    {
        private readonly IRepository<Vendor> _vendorRepository;
        private readonly IRepository<VendorGroup> _vendorGroupRepository;
        public VendorService(IRepository<Vendor> vendorRepository, IRepository<VendorGroup> vendorGroupRepository)
        {
            _vendorGroupRepository = vendorGroupRepository;
            _vendorRepository = vendorRepository;
        }
        public async Task<bool> Add(NewVendorDTO data, ServiceRegistered serviceRegistered)
        {
            Vendor vendor = new()
            {
                Address = data.Address,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                Name = data.Name,
                VendorGroup = await _vendorGroupRepository.GetById(data.GroupId),
                ServiceRegisteredFrom = serviceRegistered,
            };
            await _vendorRepository.Create(vendor);
            return await _vendorRepository.Save();
        }

        public async Task<bool> Delete(string id)
        {
            await _vendorRepository.Delete(id);
            return await _vendorRepository.Save();
        }

        public Task<List<ShowVendorDTO>> FilterList(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<ShowVendorDTO> FindById(string id, ServiceRegistered serviceRegistered)
        {
            var vendor = await _vendorRepository.getFromDatabase()
                  .Include(v => v.VendorGroup)
                  .Where(g => !g.IsDeleted)
                  .Where(g => g.ServiceRegisteredFrom.Id == serviceRegistered.Id)
                  .FirstOrDefaultAsync(v => v.Id == id);
            if (vendor != null)
            {
                return new ShowVendorDTO()
                {
                    Id = vendor.Id,
                    Name = vendor.Name,
                    Email = vendor.Email,
                    Address = vendor.Address,
                    PhoneNumber = vendor.PhoneNumber,
                    CreateDate = vendor.CreatedDate,
                    Group = vendor.VendorGroup != null ? new ShowVendorGroupDTO()
                    {
                        Id = vendor.VendorGroup.Id,
                        Name = vendor.VendorGroup.Name,
                    } : null,
                };
            }
            return null;
        }

        public async Task<List<ShowVendorDTO>> GetAll(ServiceRegistered service)
        {
            var vendors = await _vendorRepository.getFromDatabase()
                  .Include(v => v.VendorGroup)
                  .Where(g => !g.IsDeleted)
                  .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                  .OrderByDescending(v => v.CreatedDate)
                  .ToListAsync();
            var result = vendors
                .Select(v => new ShowVendorDTO()
                {
                    Id = v.Id,
                    Name = v.Name,
                    Email = v.Email,
                    Address = v.Address,
                    PhoneNumber = v.PhoneNumber,
                    CreateDate = v.CreatedDate,
                    Group = v.VendorGroup != null ? new ShowVendorGroupDTO()
                    {
                        Id = v.VendorGroup.Id,
                        Name = v.VendorGroup.Name,
                    } : null,
                })
                .ToList();
            return result;
        }

        public async Task<bool> Update(NewVendorDTO data)
        {
            Vendor vendor = new()
            {
                Id = data.Id,
                Address = data.Address,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                Name = data.Name,
                VendorGroup = await _vendorGroupRepository.GetById(data.GroupId)
            };
            await _vendorRepository.Update(vendor);
            return await _vendorRepository.Save();
        }
    }
}
