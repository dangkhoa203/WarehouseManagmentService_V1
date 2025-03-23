
using WarehouseInventoryManagementAPI.DTO.Product.Show;

using WarehouseInventoryManagementAPI.DTO.Product.Change;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Product_Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_Entity;
using WarehouseInventoryManagementAPI.Repository;
using WarehouseInventoryManagementAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace WarehouseInventoryManagementAPI.Services.Product_Related_Service
{
    public class ProductGroupService : IGroupService<ProductGroup, NewProductGroupDTO, ShowProductGroupDTO>
    {
        private readonly IRepository<ProductGroup> _productGroupRepository;
        public ProductGroupService(IRepository<ProductGroup> productGroupRepository)
        {
            _productGroupRepository = productGroupRepository;
        }
        public async Task<bool> AddGroup(NewProductGroupDTO group, ServiceRegistered serviceRegistered)
        {
            ProductGroup Group = new()
            {
                Name = group.Name,
                Description = group.Description,
                ServiceRegisteredFrom=serviceRegistered
            };
            await _productGroupRepository.Create(Group);
            return await _productGroupRepository.Save();
        }

        public async Task<bool> DeleteGroup(string id)
        {
            await _productGroupRepository.Delete(id);
            return await _productGroupRepository.Save();
        }

        public Task<List<ShowProductGroupDTO>> FilterList(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShowProductGroupDTO>> GetAll(ServiceRegistered service)
        {
            var Group = await _productGroupRepository.getFromDatabase()
                 .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                 .OrderByDescending(g => g.CreatedDate)
                 .ToListAsync();
            var result = Group
                .Select(g => new ShowProductGroupDTO()
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    CreateDate=g.CreatedDate
                })
                .ToList();
            return result;
        }

        public async Task<ShowProductGroupDTO> GetGroupById(string id, ServiceRegistered service)
        {
            var Group = await _productGroupRepository.getFromDatabase()
                   .Select(g => g)
                   .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                   .FirstOrDefaultAsync(g => g.Id == id);
            if (Group != null)
            {
                return new ShowProductGroupDTO()
                {
                    Id=Group.Id,
                    Name = Group.Name,
                    Description = Group.Description,
                };
            }
            return null;
        }

        public async Task<bool> UpdateGroup(NewProductGroupDTO group)
        {
            ProductGroup Group = new()
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
            };
            await _productGroupRepository.Update(Group);
            return await _productGroupRepository.Save();
        }
    }
}
