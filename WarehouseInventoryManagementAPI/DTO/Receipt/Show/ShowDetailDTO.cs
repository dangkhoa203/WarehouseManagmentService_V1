using WarehouseInventoryManagementAPI.DTO.Generic;
using WarehouseInventoryManagementAPI.DTO.Product.Show;

namespace WarehouseInventoryManagementAPI.DTO.Receipt.Show
{
    public class ShowDetailDTO
    {
        public  string ProductId { get; set; }
        public  string ProductName { get; set; }
        public  int Quantity { get; set; }
        public  int TotalPrice { get; set; }
    }
}
