using WarehouseInventoryManagementAPI.DTO.Generic;

namespace WarehouseInventoryManagementAPI.DTO.Receipt.Change
{
    public class NewVendorReplenishReceiptDTO: NewGenericDTO
    {
        public required string VendorId { get; set; }
        public required DateTime DateOrder { get; set; }
        public required ICollection<NewDetailDTO> Details { get; set; }
    }
}
