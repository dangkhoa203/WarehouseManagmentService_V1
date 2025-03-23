using WarehouseInventoryManagementAPI.DTO.Generic;

namespace WarehouseInventoryManagementAPI.DTO.Vendor.Show
{
    public class ShowVendorDTO:ShowGenericDTO
    {
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public ShowVendorGroupDTO? Group { get; set; }
    }
}
