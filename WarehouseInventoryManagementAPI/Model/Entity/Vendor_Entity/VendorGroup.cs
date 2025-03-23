﻿using Microsoft.EntityFrameworkCore;
using NanoidDotNet;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_EntiTy;

namespace WarehouseInventoryManagementAPI.Model.Entity.Vendor_Entity
{
    public class VendorGroup:EntityGeneric
    {
        public string Description { get; set; }
        public VendorGroup() : base()
        {
            Id = $"NHOMNCC-{Nanoid.Generate(Nanoid.Alphabets.LowercaseLettersAndDigits, 5)}";
        }
        public virtual ICollection<Vendor> Vendors { get; set; }
    }
}
