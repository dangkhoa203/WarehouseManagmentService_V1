using WarehouseInventoryManagementAPI.DTO.Generic;

namespace WarehouseInventoryManagementAPI.DTO.Receipt.Change
{
    public class NewDetailDTO
    {
        public required string ProductId { get; set; }
        public required int Quantity { get; set; }
    }
}
