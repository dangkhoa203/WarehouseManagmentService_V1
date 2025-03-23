using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.DTO.Customer.Change;
using WarehouseInventoryManagementAPI.DTO.Customer.Show;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Customer_Entity;
using WarehouseInventoryManagementAPI.Repository;
using WarehouseInventoryManagementAPI.Services.Interface;

namespace WarehouseInventoryManagementAPI.Services.Customer_Related_Service
{
    public class CustomerService : IDataService<Customer, NewCustomerDTO, ShowCustomerDTO>
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerGroup> _customerGroupRepository;
        public CustomerService(IRepository<Customer> customerRepository, IRepository<CustomerGroup> customerGroupRepository)
        {
            _customerGroupRepository = customerGroupRepository;
            _customerRepository = customerRepository;
        }
        public async Task<bool> Add(NewCustomerDTO data, ServiceRegistered serviceRegistered)
        {
            Customer customer = new()
            {
                Address = data.Address,
                Name = data.Name,
                Email = data.Email,
                PhoneNumber=data.PhoneNumber,
                CustomerGroup = await _customerGroupRepository.GetById(data.GroupId),
                ServiceRegisteredFrom = serviceRegistered,
            };
            await _customerRepository.Create(customer);
            return await _customerRepository.Save();
        }

        public async Task<bool> Delete(string id)
        {
            await _customerRepository.Delete(id);
            return await _customerRepository.Save();
        }

        public Task<List<ShowCustomerDTO>> FilterList(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<ShowCustomerDTO> FindById(string id, ServiceRegistered serviceRegistered)
        {
           var Customer= await _customerRepository.getFromDatabase()
                   .Include(c => c.CustomerGroup)
                   .Where(c => !c.IsDeleted)
                   .Where(g => g.ServiceRegisteredFrom.Id == serviceRegistered.Id)
                   .FirstOrDefaultAsync(c=>c.Id==id);
            if (Customer != null)
            {
                return new ShowCustomerDTO()
                {
                    Id = Customer.Id,
                    Name = Customer.Name,
                    PhoneNumber=Customer.PhoneNumber,
                    Email = Customer.Email,
                    Address = Customer.Address,
                    CreateDate = Customer.CreatedDate,
                    Group = Customer.CustomerGroup!=null ? new ShowCustomerGroupDTO(Customer.CustomerGroup.Id, Customer.CustomerGroup.Name, Customer.CustomerGroup.Description, Customer.CustomerGroup.CreatedDate):null,
                };
            }
            return null;
        }

        public async Task<List<ShowCustomerDTO>> GetAll(ServiceRegistered service)
        {
            var customers =await _customerRepository.getFromDatabase()
                   .Include(c => c.CustomerGroup)
                   .Where(c=>!c.IsDeleted)
                   .Where(g => g.ServiceRegisteredFrom.Id == service.Id)
                   .OrderByDescending(c=>c.CreatedDate)
                   .ToListAsync();
            var result = customers
                .Select(c => new ShowCustomerDTO()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Address = c.Address,
                    PhoneNumber=c.PhoneNumber,
                    CreateDate = c.CreatedDate,
                    Group = c.CustomerGroup != null ? new ShowCustomerGroupDTO(c.CustomerGroup.Id, c.CustomerGroup.Name, c.CustomerGroup.Description, c.CustomerGroup.CreatedDate):null,
                })
                .ToList();
            return result;
        }

        public async Task<bool> Update(NewCustomerDTO data)
        {
            Customer Customer = new() {
                Id=data.Id,
                Name = data.Name,
                Address = data.Address,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                CustomerGroup=await _customerGroupRepository.GetById(data.GroupId),
            };
            await _customerRepository.Update(Customer);
            return await _customerRepository.Save();
        }
    }
}
