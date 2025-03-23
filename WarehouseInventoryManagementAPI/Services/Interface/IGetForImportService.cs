using WarehouseInventoryManagementAPI.Model.Entity;

namespace WarehouseInventoryManagementAPI.Services.Interface
{
    public interface IGetForImportService<T> where T : class
    {
        Task<List<T>> GetAll_ImportForm(ServiceRegistered service);
        Task<List<T>> GetAll_ReturnForm(ServiceRegistered service);
    }
}
