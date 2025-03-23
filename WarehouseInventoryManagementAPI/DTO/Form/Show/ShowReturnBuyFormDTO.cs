using WarehouseInventoryManagementAPI.DTO.Generic;
using WarehouseInventoryManagementAPI.DTO.Receipt.Show;

namespace WarehouseInventoryManagementAPI.DTO.Form.Show
{
    public class ShowReturnBuyFormDTO : ShowGenericDTO
    {
        public required ShowCustomerBuyReceiptDTO Receipt { get; set; }
        public required DateTime OrderDate { get; set; }
    }
}
