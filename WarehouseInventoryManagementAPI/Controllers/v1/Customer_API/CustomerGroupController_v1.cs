using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.DTO.Customer.Show;
using WarehouseInventoryManagementAPI.Model.Entity.Customer_Entity;
using WarehouseInventoryManagementAPI.DTO.Customer.Change;
using WarehouseInventoryManagementAPI.Services.Interface;
using WarehouseInventoryManagementAPI.Services.Customer_Related_Service;
using WarehouseInventoryManagementAPI.Services.Vendor_Related_Service;

namespace WarehouseInventoryManagementAPI.Controllers.v1.Customer_API
{
    [Route("api/v1/")]
    [ApiController]
    public class CustomerGroupController_v1 : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly CustomerGroupService _groupService;
        public CustomerGroupController_v1(ApplicationDbContext context, UserManager<Account> userManager, CustomerGroupService service)
        {
            _context = context;
            _userManager = userManager;
            _groupService = service;
        }
        [HttpPost("customer-groups"), Authorize]
        public async Task<IActionResult> AddCustomerGroup(NewCustomerGroupDTO model)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Select(u => u).Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (await _groupService.AddGroup(model,service.ServiceRegistered))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpDelete("customer-groups/{id}"), Authorize]
        public async Task<IActionResult> DeleteCustomerGroup([FromRoute] string id)
        {
          
            if (await _groupService.DeleteGroup(id))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpPut("customer-groups"), Authorize]
        public async Task<IActionResult> UpdateCustomerGroup(NewCustomerGroupDTO customergroup)
        {
            if (await _groupService.UpdateGroup(customergroup))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpGet("customer-groups"), Authorize]
        public async Task<IActionResult> CustomerGroups()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                return Ok(await _groupService.GetAll(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
        [HttpGet("customer-groups/{id}"), Authorize]
        public async Task<IActionResult> VendorGroups(string id)
        {
            try
            {
                var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
                var result = await _groupService.GetGroupById(id,service);
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
