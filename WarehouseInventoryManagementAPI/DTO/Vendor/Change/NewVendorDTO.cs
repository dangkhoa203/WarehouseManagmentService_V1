using WarehouseInventoryManagementAPI.DTO.Generic;

namespace WarehouseInventoryManagementAPI.DTO.Vendor.Change
{
    public class NewVendorDTO:NewGenericDTO
    {
        public string? Id {  get; set; } 
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? GroupId { get; set; }
    }
}
