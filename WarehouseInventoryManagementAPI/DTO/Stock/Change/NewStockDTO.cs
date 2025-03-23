using WarehouseInventoryManagementAPI.DTO.Generic;

namespace WarehouseInventoryManagementAPI.DTO.Stock.Change
{
    public class NewStockDTO
    {
        public required string ProductId { get; set; }
        public required int Quantity {  get; set; }
    }
}
