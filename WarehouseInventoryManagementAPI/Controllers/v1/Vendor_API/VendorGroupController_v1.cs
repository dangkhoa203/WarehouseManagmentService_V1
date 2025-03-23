using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.DTO.Customer.Show;
using WarehouseInventoryManagementAPI.DTO.Vendor.Change;
using WarehouseInventoryManagementAPI.DTO.Vendor.Show;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Customer_Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Vendor_Entity;
using WarehouseInventoryManagementAPI.Services.Interface;
using WarehouseInventoryManagementAPI.Services.Vendor_Related_Service;

namespace WarehouseInventoryManagementAPI.Controllers.v1.Vendor_API
{
    [Route("api/v1")]
    [ApiController]
    public class VendorGroupController_v1 : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly VendorGroupService _vendorGroupService;
        public VendorGroupController_v1(ApplicationDbContext context, UserManager<Account> userManager,VendorGroupService vendorGroupService)
        {
            _context = context;
            _userManager = userManager;
            _vendorGroupService = vendorGroupService;
        }
        [HttpPost("vendor-groups"),Authorize]
        public async Task<IActionResult> AddVendorGroup(NewVendorGroupDTO group)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u=>u.ServiceRegistered).FirstOrDefault();
            if (await _vendorGroupService.AddGroup(group,service))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpDelete("vendor-groups/{id}"), Authorize]
        public async Task<IActionResult> DeleteVendorGroup([FromRoute]string id)
        {
            if (await _vendorGroupService.DeleteGroup(id))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpPut("vendor-groups"), Authorize]
        public async Task<IActionResult> UpdateVendorGroup(NewVendorGroupDTO group)
        {
            if (await _vendorGroupService.UpdateGroup(group))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpGet("vendor-groups"), Authorize]
        public async Task<IActionResult> VendorGroups()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                return Ok(await _vendorGroupService.GetAll(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
        [HttpGet("vendor-groups/{id}"), Authorize]
        public async Task<IActionResult> VendorGroups(string id)
        {
            try
            {
                var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
                var result = await _vendorGroupService.GetGroupById(id, service);
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
