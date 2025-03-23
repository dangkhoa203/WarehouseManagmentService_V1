using WarehouseInventoryManagementAPI.DTO.Form.Change;
using WarehouseInventoryManagementAPI.DTO.Stock.Change;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Receipt;

namespace WarehouseInventoryManagementAPI.Services.Interface
{
    public interface IImportService<T> where T : class
    {
        Task<bool> NewFormImport(NewFormDTO data, ServiceRegistered serviceRegistered);
    }
}
