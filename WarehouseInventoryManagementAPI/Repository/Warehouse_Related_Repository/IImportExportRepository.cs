using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Receipt;

namespace WarehouseInventoryManagementAPI.Repository.Warehouse_Related_Repository
{
    public interface IImportExportRepository
    {
        Task Add(Stock item);
        Task<bool> Subtract(Stock item);
        Task<bool> CheckQuantity(string Id,int Quantity);
    }
}
