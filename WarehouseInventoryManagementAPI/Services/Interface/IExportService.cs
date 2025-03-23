using WarehouseInventoryManagementAPI.DTO.Form.Change;
using WarehouseInventoryManagementAPI.Model.Entity;

namespace WarehouseInventoryManagementAPI.Services.Interface
{
    public interface IExportService<T> where T : class
    {
        Task<bool> NewFormExport(NewFormDTO data, ServiceRegistered serviceRegistered);
        Task<bool> CheckStock(NewFormDTO data);
    }
}
