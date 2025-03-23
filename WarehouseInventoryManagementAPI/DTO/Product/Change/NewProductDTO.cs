using WarehouseInventoryManagementAPI.DTO.Generic;

namespace WarehouseInventoryManagementAPI.DTO.Product.Change
{
    public class NewProductDTO : NewGenericDTO
    {

        public int PricePerUnit { get; set; }
        public string MeasureUnit { get; set; }
        public required string GroupId { get; set; }
    }
}
