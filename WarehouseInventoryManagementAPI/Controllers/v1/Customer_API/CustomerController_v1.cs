using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.DTO.Customer.Change;
using WarehouseInventoryManagementAPI.DTO.Customer.Show;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Customer_Entity;
using WarehouseInventoryManagementAPI.Services.Customer_Related_Service;
using WarehouseInventoryManagementAPI.Services.Interface;
using WarehouseInventoryManagementAPI.Services.Vendor_Related_Service;

namespace WarehouseInventoryManagementAPI.Controllers.v1.Customer_API
{
    [Route("api/v1/")]
    [ApiController]
    public class CustomerController_v1 : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly CustomerService _customerService;
        public CustomerController_v1(ApplicationDbContext context, UserManager<Account> userManager, CustomerService customerService)
        {
            _context = context;
            _userManager = userManager;
            _customerService = customerService;
        }

        [HttpPost("Customers"), Authorize]
        public async Task<IActionResult> AddCustomer(NewCustomerDTO customer)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            if (await _customerService.Add(customer, service))
            {
                return Ok("OK");
            }
            else
                return BadRequest("Error");
        }
        [HttpDelete("Customers/{id}"), Authorize]
        public async Task<IActionResult> DeleteCustomerGroup([FromRoute] string id)
        {
            if (await _customerService.Delete(id))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpPut("Customers"), Authorize]
        public async Task<IActionResult> UpdateCustomerGroup(NewCustomerDTO customer)
        {
            if (await _customerService.Update(customer))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpGet("Customers"), Authorize]
        public async Task<IActionResult> Customers()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered)
                                        .Where(u => u.UserName == User.Identity.Name)
                                        .Select(s => s.ServiceRegistered)
                                        .FirstOrDefault();
            try
            {
                return Ok(await _customerService.GetAll(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }

        }
        [HttpGet("Customers/{id}"), Authorize]
        public async Task<IActionResult> Vendors(string id)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                var result = await _customerService.FindById(id, service);
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
