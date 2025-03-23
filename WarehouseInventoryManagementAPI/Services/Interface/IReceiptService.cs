using WarehouseInventoryManagementAPI.DTO.Generic;
using WarehouseInventoryManagementAPI.Model.Entity;

namespace WarehouseInventoryManagementAPI.Services.Interface
{
    public interface IReceiptService<T, N, S> where T : class where N : NewGenericDTO where S : ShowGenericDTO
    {
        Task<List<S>> GetAll(ServiceRegistered service);
        Task<List<S>> FilterList(string query);
        Task<bool> Add(N data, ServiceRegistered serviceRegistered);
        Task<bool> Delete(string id);
        Task<bool> Update(N data);
        Task<S> FindById(ServiceRegistered service, string id);
    }
}
