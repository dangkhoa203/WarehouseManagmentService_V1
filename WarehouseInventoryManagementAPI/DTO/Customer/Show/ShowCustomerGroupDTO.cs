using WarehouseInventoryManagementAPI.DTO.Generic;

namespace WarehouseInventoryManagementAPI.DTO.Customer.Show
{
    public class ShowCustomerGroupDTO : ShowGenericDTO
    {
        public string Description { get; set; }

        public ShowCustomerGroupDTO() : base()
        {
            Description = " ";
        }
        public ShowCustomerGroupDTO(string id, string name, string description, DateTime createdate) : base(id, name, createdate)
        {
            Description = description;
        }
    }
}
