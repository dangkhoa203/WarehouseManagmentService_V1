using WarehouseInventoryManagementAPI.DTO.Generic;

namespace WarehouseInventoryManagementAPI.DTO.Form.Change
{
    public class NewFormDTO:NewGenericDTO
    {
        public required string ReceiptId { get; set; }
        public required DateTime OrderDate { get; set; }
        public bool? UpdateStock { get; set; }
    }
}
