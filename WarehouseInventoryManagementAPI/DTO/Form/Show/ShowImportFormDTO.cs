using WarehouseInventoryManagementAPI.DTO.Generic;
using WarehouseInventoryManagementAPI.DTO.Receipt.Show;

namespace WarehouseInventoryManagementAPI.DTO.Form.Show
{
    public class ShowImportFormDTO:ShowGenericDTO
    {
        public required ShowVendorReplenishReceiptDTO Receipt { get; set; }
        public required DateTime OrderDate { get; set; }
    }
}
