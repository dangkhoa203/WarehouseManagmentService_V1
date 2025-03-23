using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.DTO.Customer.Change;
using WarehouseInventoryManagementAPI.DTO.Customer.Show;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Customer_Entity;
using WarehouseInventoryManagementAPI.Repository;
using WarehouseInventoryManagementAPI.Services.Interface;

namespace WarehouseInventoryManagementAPI.Services.Customer_Related_Service
{
    public class CustomerGroupService : IGroupService<CustomerGroup, NewCustomerGroupDTO, ShowCustomerGroupDTO>
    {
        private readonly IRepository<CustomerGroup> _repository;

        public CustomerGroupService(IRepository<CustomerGroup> repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddGroup(NewCustomerGroupDTO group, ServiceRegistered serviceRegistered)
        {
            CustomerGroup customerGroup = new()
            {
                Name = group.Name,
                Description = group.Description,
                ServiceRegisteredFrom = serviceRegistered,
            };
            await _repository.Create(customerGroup);
            return await _repository.Save();
        }

        public async Task<bool> DeleteGroup(string id)
        {
            await _repository.Delete(id);
            return await _repository.Save();
        }

        public Task<List<ShowCustomerGroupDTO>> FilterList(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShowCustomerGroupDTO>> GetAll(ServiceRegistered service)
        {
            var customerGroup = await _repository.getFromDatabase()
                     .Select(g => g)
                     .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                     .OrderByDescending(g => g.CreatedDate)
                     .ToListAsync();
            var result = customerGroup
                .Select(g => new ShowCustomerGroupDTO()
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    CreateDate = g.CreatedDate,
                })
                .ToList();
            return result;
        }

        public async Task<ShowCustomerGroupDTO> GetGroupById(string id, ServiceRegistered service)
        {
            var Group = await _repository.getFromDatabase()
                   .Select(g => g)
                   .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                   .FirstOrDefaultAsync(g=>g.Id==id);
            if (Group != null)
            {
                return new ShowCustomerGroupDTO {
                    Id = Group.Id,
                    Name = Group.Name,
                    Description = Group.Description,
                    CreateDate = Group.CreatedDate
                };
            }
            return null;
        }

        public async Task<bool> UpdateGroup(NewCustomerGroupDTO group)
        {
            CustomerGroup CustomerGroup = new()
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
            };
            await _repository.Update(CustomerGroup);
            return await _repository.Save();
        }
    }
}
