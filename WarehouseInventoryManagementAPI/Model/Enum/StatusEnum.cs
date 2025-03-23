using System.ComponentModel;

namespace WarehouseInventoryManagementAPI.Model.Enum
{
    public enum StatusEnum
    {
        [Description("Draft")]
        Draft = 0,
        [Description("Pending")]
        Pending=1,
        [Description("Cancelled")]
        Cancelled = 2,
        [Description("Confirmed")]
        Confirmed = 3
    }
}
