using NanoidDotNet;
using WarehouseInventoryManagementAPI.Model.Receipt;

namespace WarehouseInventoryManagementAPI.Model.Form
{
    public class StockExportForm: FormGeneric
    {
        public StockExportForm()
        {
            Id= $"XUATKHO-{Nanoid.Generate(Nanoid.Alphabets.LowercaseLettersAndDigits, 5)}";
        }
        public DateTime OrderDate { get; set; }
        public string ReceiptId { get; set; }
        public virtual CustomerBuyReceipt Receipt { get; set; }
    }
}
