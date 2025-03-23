using Microsoft.Identity.Client;
using NanoidDotNet;
using System.ComponentModel.DataAnnotations;
using WarehouseInventoryManagementAPI.Model.Entity.Customer_Entity;

namespace WarehouseInventoryManagementAPI.Model.Entity
{
    public class ServiceRegistered
    {
        [Key]
        public string Id { get; set; }
        public ServiceRegistered()
        {
            Id = $"SERVICE-{Nanoid.Generate(Nanoid.Alphabets.LowercaseLettersAndDigits, 4)}";
        }
        public virtual ICollection<Account>? Accounts { get; set; }

    }
}
