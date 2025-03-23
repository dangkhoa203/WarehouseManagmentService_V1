using Microsoft.EntityFrameworkCore;
using NanoidDotNet;

namespace WarehouseInventoryManagementAPI.Model.Entity.Product_Entity
{
    public class ProductGroup:EntityGeneric
    {
        public  string Description { get; set; }
        
        public ProductGroup() : base()
        {
            Id= $"NHOMSP-{Nanoid.Generate(Nanoid.Alphabets.LowercaseLettersAndDigits, 5)}";
        }
        public virtual ICollection<Product> Products { get; set; }
    }
}
