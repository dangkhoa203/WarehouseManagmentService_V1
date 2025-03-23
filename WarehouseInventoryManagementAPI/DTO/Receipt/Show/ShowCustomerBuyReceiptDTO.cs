using WarehouseInventoryManagementAPI.DTO.Customer.Show;
using WarehouseInventoryManagementAPI.DTO.Generic;
using WarehouseInventoryManagementAPI.DTO.Receipt.Change;

namespace WarehouseInventoryManagementAPI.DTO.Receipt.Show
{
    public class ShowCustomerBuyReceiptDTO:ShowGenericDTO
    {
        public  ShowCustomerDTO? Customer { get; set; }
        public required DateTime DateOrder { get; set; }
        public  ICollection<ShowDetailDTO>? Details { get; set; }
    }
}
