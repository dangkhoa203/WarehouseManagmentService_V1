using WarehouseInventoryManagementAPI.Model.Entity;

namespace WarehouseInventoryManagementAPI.Services.Interface
{
    public interface IGetForExportService<T> where T : class
    {
        Task<List<T>> GetAll_ExportForm(ServiceRegistered service);
        Task<List<T>> GetAll_ReturnForm(ServiceRegistered service);
    }
}
