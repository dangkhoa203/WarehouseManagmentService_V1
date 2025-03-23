using Microsoft.EntityFrameworkCore;
using NanoidDotNet;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_EntiTy;
using WarehouseInventoryManagementAPI.Model.Receipt;


namespace WarehouseInventoryManagementAPI.Model.Entity.Product_Entity
{
    public class Product : EntityImportance
    {
        public  int PricePerUnit { get; set; }
        public required string MeasureUnit { get; set; }
        public Product() : base()
        {
            Id = $"SP-{Nanoid.Generate(Nanoid.Alphabets.LowercaseLettersAndDigits, 5)}";
        }
        public virtual ProductGroup? ProductGroup { get; set; }
        public virtual ICollection<VendorReplenishReceiptDetail>? VendorReplenishReceiptDetails { get; set; }
        public virtual ICollection<CustomerBuyReceiptDetail>? CustomerBuyReceiptDetails { get; set; }
        public virtual Stock? Stocks { get; set; }
    }
}
