using WarehouseInventoryManagementAPI.DTO.Customer.Show;
using WarehouseInventoryManagementAPI.DTO.Generic;
using WarehouseInventoryManagementAPI.DTO.Vendor.Show;

namespace WarehouseInventoryManagementAPI.DTO.Receipt.Show
{
    public class ShowVendorReplenishReceiptDTO:ShowGenericDTO
    {
        public ShowVendorDTO? Vendor { get; set; }
        public required DateTime DateOrder { get; set; }
        public ICollection<ShowDetailDTO>? Details { get; set; }
    }
}
