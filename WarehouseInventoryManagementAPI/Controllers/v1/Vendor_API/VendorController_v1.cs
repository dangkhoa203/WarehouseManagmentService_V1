using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.DTO.Vendor.Change;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Services.Product_Related_Service;
using WarehouseInventoryManagementAPI.Services.Vendor_Related_Service;

namespace WarehouseInventoryManagementAPI.Controllers.v1.Vendor_API
{
    [Route("api/v1")]
    [ApiController]
    public class VendorController_v1 : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly VendorService _vendorService;
        public VendorController_v1(ApplicationDbContext context, UserManager<Account> userManager, VendorService vendorService)
        {
            _context = context;
            _userManager = userManager;
            _vendorService = vendorService;
        }
        [HttpPost("Vendors"), Authorize]
        public async Task<IActionResult> AddVendor(NewVendorDTO model)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            if (await _vendorService.Add(model, service))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpDelete("Vendors/{id}"), Authorize]
        public async Task<IActionResult> DeleteVendor([FromRoute] string id)
        {
            if (await _vendorService.Delete(id))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpPut("Vendors"), Authorize]
        public async Task<IActionResult> UpdateVendor(NewVendorDTO vendor)
        {
            if (await _vendorService.Update(vendor))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpGet("Vendors"), Authorize]
        public async Task<IActionResult> Vendors()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                return Ok(await _vendorService.GetAll(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
        [HttpGet("Vendors/{id}"), Authorize]
        public async Task<IActionResult> Vendors(string id)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                var result = await _vendorService.FindById(id,service);
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
    }

}
