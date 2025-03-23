using WarehouseInventoryManagementAPI.DTO.Generic;

namespace WarehouseInventoryManagementAPI.DTO.Product.Show
{
    public class ShowProductDTO:ShowGenericDTO
    {
        public int PricePerUnit { get; set; }
        public  string MeasureUnit { get; set; }

        public ShowProductGroupDTO? Group { get; set; }

    }
}
