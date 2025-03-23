using WarehouseInventoryManagementAPI.DTO.Generic;
using WarehouseInventoryManagementAPI.DTO.Product.Show;

namespace WarehouseInventoryManagementAPI.DTO.Stock.Show
{
    public class ShowStockDTO:ShowGenericDTO
    {
        public required int Quantity { get; set; }
    }
}
