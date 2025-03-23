using WarehouseInventoryManagementAPI.DTO.Generic;

namespace WarehouseInventoryManagementAPI.DTO.Receipt.Change
{
    public class NewCustomerBuyReceiptDTO:NewGenericDTO
    {
        public required string CustomerId { get; set; }
        public required DateTime DateOrder {  get; set; }
        public required ICollection<NewDetailDTO> Details { get; set; }
    }
}
