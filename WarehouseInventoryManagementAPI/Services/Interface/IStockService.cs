using WarehouseInventoryManagementAPI.DTO.Form.Change;
using WarehouseInventoryManagementAPI.DTO.Stock.Change;
using WarehouseInventoryManagementAPI.DTO.Stock.Show;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Form;
using WarehouseInventoryManagementAPI.Model.Receipt;

namespace WarehouseInventoryManagementAPI.Services.Interface
{
    public interface IStockService
    {
        Task<List<ShowStockDTO>> GetAll(ServiceRegistered service);
        Task<List<ShowStockDTO>> FilterList(string query);
        Task<bool> Add(NewStockDTO data, ServiceRegistered serviceRegistered);
        Task<bool> Delete(string id);
        Task<bool> Update(NewStockDTO data);
        Task<ShowStockDTO> FindById(string id);
    }
}
