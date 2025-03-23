using System.ComponentModel.DataAnnotations;

namespace WarehouseInventoryManagementAPI.Model.Entity
{
    public abstract class EntityGeneric
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public EntityGeneric()
        {
            CreatedDate = DateTime.Now;
        }
        public virtual ServiceRegistered ServiceRegisteredFrom { get; set; }
    }
}
