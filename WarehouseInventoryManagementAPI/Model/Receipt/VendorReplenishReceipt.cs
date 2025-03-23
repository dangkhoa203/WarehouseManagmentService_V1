using NanoidDotNet;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_EntiTy;
using WarehouseInventoryManagementAPI.Model.Form;

namespace WarehouseInventoryManagementAPI.Model.Receipt
{
    public class VendorReplenishReceipt:ReceiptGeneric
    {
        public VendorReplenishReceipt()
        {
            Id= $"HDNHAPHANG-{Nanoid.Generate(Nanoid.Alphabets.LowercaseLettersAndDigits, 5)}";
        }
        public DateTime DateOrder {  get; set; }
        public virtual ReturnReplenishForm? ReturnReplenishForm { get; set; }
        public virtual StockImportForm? StockImportReport { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<VendorReplenishReceiptDetail> Details { get; set; }
    }
}
