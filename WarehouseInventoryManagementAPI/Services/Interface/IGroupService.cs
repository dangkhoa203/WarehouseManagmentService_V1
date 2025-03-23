using WarehouseInventoryManagementAPI.DTO.Generic;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Repository;

namespace WarehouseInventoryManagementAPI.Services.Interface
{
    public interface IGroupService<T, N, S> where T : class where N : NewGenericDTO where S : ShowGenericDTO
    {
        Task<List<S>> GetAll(ServiceRegistered service);
        Task<List<S>> FilterList(string query);
        Task<bool> AddGroup(N group, ServiceRegistered serviceRegistered);
        Task<bool> DeleteGroup(string id);
        Task<bool> UpdateGroup(N group);
        Task<S> GetGroupById(string id, ServiceRegistered service);
    }
}
