using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.DTO.Vendor.Change;
using WarehouseInventoryManagementAPI.DTO.Vendor.Show;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_Entity;
using WarehouseInventoryManagementAPI.Repository;
using WarehouseInventoryManagementAPI.Repository.Vendor_Related_Repository;
using WarehouseInventoryManagementAPI.Services.Interface;

namespace WarehouseInventoryManagementAPI.Services.Vendor_Related_Service
{
    public class VendorGroupService : IGroupService<VendorGroup, NewVendorGroupDTO, ShowVendorGroupDTO>
    {
        private readonly IRepository<VendorGroup> _vendorGroupRepository;
        public VendorGroupService(IRepository<VendorGroup> vendorGroupRepository)
        {
            _vendorGroupRepository = vendorGroupRepository;
        }
        public async Task<bool> AddGroup(NewVendorGroupDTO group, ServiceRegistered serviceRegistered)
        {
            VendorGroup Group = new()
            {
                Name = group.Name,
                Description = group.Description,
                ServiceRegisteredFrom=serviceRegistered
            };
            await _vendorGroupRepository.Create(Group);
            return await _vendorGroupRepository.Save();
        }

        public async Task<bool> DeleteGroup(string id)
        {
            await _vendorGroupRepository.Delete(id);
            return await _vendorGroupRepository.Save();
        }

        public Task<List<ShowVendorGroupDTO>> FilterList(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShowVendorGroupDTO>> GetAll(ServiceRegistered service)
        {
            var Group = await _vendorGroupRepository.getFromDatabase()
                 .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                 .OrderByDescending(g => g.CreatedDate)
                 .ToListAsync();
            var result = Group
                .Select(g => new ShowVendorGroupDTO()
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description=g.Description,
                    CreateDate=g.CreatedDate
                })
                .ToList();
            return result;
        }

        public async Task<ShowVendorGroupDTO> GetGroupById(string id, ServiceRegistered service)
        {
            var Group = await _vendorGroupRepository.getFromDatabase()
                   .Select(g => g)
                   .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                   .FirstOrDefaultAsync(g => g.Id == id);
            if (Group != null )
            {
                return new ShowVendorGroupDTO()
                {
                    Id=Group.Id,
                    CreateDate=Group.CreatedDate,
                    Name = Group.Name,
                    Description= Group.Description,
                };
            }
            return null;
        }

        public async Task<bool> UpdateGroup(NewVendorGroupDTO group)
        {
            VendorGroup Group = new()
            {
                Id=group.Id,
                Name = group.Name,
                Description = group.Description,
            };
            await _vendorGroupRepository.Update(Group);
            return await _vendorGroupRepository.Save();
        }
    }
}
