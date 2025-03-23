﻿using System.ComponentModel.DataAnnotations;
using WarehouseInventoryManagementAPI.Model.Entity.Product_Entity;

namespace WarehouseInventoryManagementAPI.Model.Receipt
{
    public class VendorReplenishReceiptDetail
    {
        public string ProductId { get; set; }
        public string ReceiptId { get; set; }
        public int Quantity { get; set; }
        public virtual Product ProductNav { get; set; }
        public virtual VendorReplenishReceipt ReceiptNav { get; set; }
     
    }
}
