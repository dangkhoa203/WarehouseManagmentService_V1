using WarehouseInventoryManagementAPI.DTO.Generic;

namespace WarehouseInventoryManagementAPI.DTO.Customer.Change
{
    public class NewCustomerDTO: NewGenericDTO
    {

        public required string Address { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }

        public required string GroupId { get; set; }
    }
}
