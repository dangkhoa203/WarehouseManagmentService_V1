using NanoidDotNet;
using WarehouseInventoryManagementAPI.Model.Receipt;

namespace WarehouseInventoryManagementAPI.Model.Form
{
    public class ReturnBuyForm: FormGeneric
    {
        public ReturnBuyForm()
        {
            Id= $"HOANTIEN-{Nanoid.Generate(Nanoid.Alphabets.LowercaseLettersAndDigits, 5)}";
        }
        public  DateTime OrderDate { get; set; }
        public string ReceiptId { get; set; }
        public virtual CustomerBuyReceipt Receipt { get; set; }
    }
}
