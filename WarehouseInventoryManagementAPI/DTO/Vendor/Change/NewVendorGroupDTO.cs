using WarehouseInventoryManagementAPI.DTO.Generic;

namespace WarehouseInventoryManagementAPI.DTO.Vendor.Change
{
    public class NewVendorGroupDTO: NewGenericDTO
    {
        public string? Id { get; set; }
        public string Description { get; set; }
    }
}
