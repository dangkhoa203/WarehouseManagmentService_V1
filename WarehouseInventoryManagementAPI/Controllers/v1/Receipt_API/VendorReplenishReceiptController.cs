using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.DTO.Receipt.Change;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Services.Receipt_Related_Service;

namespace WarehouseInventoryManagementAPI.Controllers.v1.Receipt_API
{
    [Route("api/v1/")]
    [ApiController]
    public class VendorReplenishReceiptController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly VendorReplenishReceiptService _vendorReplenishReceiptService;
        public VendorReplenishReceiptController(ApplicationDbContext context, UserManager<Account> userManager, VendorReplenishReceiptService vendorReplenishReceiptService)
        {
            _context = context;
            _userManager = userManager;
            _vendorReplenishReceiptService= vendorReplenishReceiptService;
        }
        [HttpPost("Vendor-Receipts"), Authorize]
        public async Task<IActionResult> NewReceipt(NewVendorReplenishReceiptDTO model)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            if (await _vendorReplenishReceiptService.Add(model, service))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpDelete("Vendor-Receipts/{id}"), Authorize]
        public async Task<IActionResult> DeleteReceipt(string id)
        {
            if (await _vendorReplenishReceiptService.Delete(id))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpPut("Vendor-Receipts"), Authorize]
        public async Task<IActionResult> UpdateReceipt(NewVendorReplenishReceiptDTO model)
        {
            if (await _vendorReplenishReceiptService.Update(model))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpGet("Vendor-Receipts"), Authorize]
        public async Task<IActionResult> Receipts()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                return Ok(await _vendorReplenishReceiptService.GetAll(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
        [HttpGet("Vendor-Receipts/{id}"), Authorize]
        public async Task<IActionResult> Receipt(string id)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                var result = await _vendorReplenishReceiptService.FindById(service, id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
        [HttpGet("Vendor-Receipts/Import"), Authorize]
        public async Task<IActionResult> ReceiptsImport()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                return Ok(await _vendorReplenishReceiptService.GetAll_ImportForm(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
        [HttpGet("Vendor-Receipts/Return"), Authorize]
        public async Task<IActionResult> ReceiptsReturn()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                return Ok(await _vendorReplenishReceiptService.GetAll_ReturnForm(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}
