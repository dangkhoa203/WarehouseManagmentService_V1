using NanoidDotNet;
using WarehouseInventoryManagementAPI.Model.Receipt;

namespace WarehouseInventoryManagementAPI.Model.Form
{
    public class StockImportForm: FormGeneric
    {
        public StockImportForm()
        {
            Id = $"NHAPKHO-{Nanoid.Generate(Nanoid.Alphabets.LowercaseLettersAndDigits, 5)}";
        }
        public DateTime OrderDate { get; set; }
        public string ReceiptId { get; set; }
        public virtual VendorReplenishReceipt Receipt { get; set; }
    }
}
