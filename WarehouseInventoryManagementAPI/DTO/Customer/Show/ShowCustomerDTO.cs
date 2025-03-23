using WarehouseInventoryManagementAPI.DTO.Generic;
using WarehouseInventoryManagementAPI.Model.Entity;

namespace WarehouseInventoryManagementAPI.DTO.Customer.Show
{
    public class ShowCustomerDTO : ShowGenericDTO
    {
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public ShowCustomerGroupDTO? Group { get; set; }



        public ShowCustomerDTO() : base()
        {

        }
        public ShowCustomerDTO(string id, string name, DateTime createdate, string email, string address, ShowCustomerGroupDTO customerGroup) : base(id, name, createdate)
        {
            Email = email;
            Address = address;
            Group = customerGroup;
        }
    }
}
