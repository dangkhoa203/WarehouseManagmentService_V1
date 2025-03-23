using WarehouseInventoryManagementAPI.DTO.Generic;
using WarehouseInventoryManagementAPI.Model.Entity;

namespace WarehouseInventoryManagementAPI.Services.Interface
{
    public interface IFormService<T, N, S> where T : class where N : NewGenericDTO where S : ShowGenericDTO
    {
        Task<List<S>> GetAll(ServiceRegistered service);
        Task<List<S>> FilterList(string query);
        Task<bool> AddForm(N data, ServiceRegistered serviceRegistered);
        Task<bool> DeleteForm(string id);
        Task<bool> UpdateForm(N data);
        Task<S> FindById(ServiceRegistered service, string id);
    }
}
