using System.ComponentModel.DataAnnotations;
using WarehouseInventoryManagementAPI.Model.Entity.Product_Entity;

namespace WarehouseInventoryManagementAPI.Model.Entity
{
    public class Stock
    {
        [Key]
        public string ProductId { get; set; }
        public Product ProductNav { get; set; }
        public int Quantity { get; set; }
        public virtual ServiceRegistered ServiceRegisteredFrom { get; set; }
    }
}
