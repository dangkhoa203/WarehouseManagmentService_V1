using System.ComponentModel.DataAnnotations;
using WarehouseInventoryManagementAPI.Model.Entity;

namespace WarehouseInventoryManagementAPI.Model.Receipt
{
    public class ReceiptGeneric
    {
        [Key]
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ReceiptGeneric()
        {
            CreatedDate = DateTime.Now;
            IsDeleted = false;
            DeletedAt = null;
        }
        public virtual ServiceRegistered ServiceRegisteredFrom { get; set; }
    }
}
