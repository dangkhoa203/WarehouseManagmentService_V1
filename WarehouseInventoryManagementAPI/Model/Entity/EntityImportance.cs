namespace WarehouseInventoryManagementAPI.Model.Entity
{
    public class EntityImportance:EntityGeneric
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public EntityImportance()
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }
}
