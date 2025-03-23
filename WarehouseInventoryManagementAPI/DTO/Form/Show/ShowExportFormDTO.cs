using WarehouseInventoryManagementAPI.DTO.Generic;
using WarehouseInventoryManagementAPI.DTO.Receipt.Show;

namespace WarehouseInventoryManagementAPI.DTO.Form.Show
{
    public class ShowExportFormDTO : ShowGenericDTO
    {
        public required ShowCustomerBuyReceiptDTO Receipt { get; set; }
        public required DateTime OrderDate { get; set; }
    }
}
