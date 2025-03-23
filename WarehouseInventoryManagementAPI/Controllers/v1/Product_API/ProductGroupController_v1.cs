using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Model.Entity.Product_Entity;
using WarehouseInventoryManagementAPI.DTO.Product.Show;
using WarehouseInventoryManagementAPI.DTO.Product.Change;
using WarehouseInventoryManagementAPI.Services.Product_Related_Service;
using WarehouseInventoryManagementAPI.Services.Vendor_Related_Service;
using WarehouseInventoryManagementAPI.Services.Interface;

namespace WarehouseInventoryManagementAPI.Controllers.v1.Product_API
{
    [Route("api/v1/")]
    [ApiController]
    public class ProductGroupController_v1 : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly ProductGroupService _productGroupService;
        public ProductGroupController_v1(ApplicationDbContext context, UserManager<Account> userManager, ProductGroupService productGroupService)
        {
            _context = context;
            _userManager = userManager;
            _productGroupService= productGroupService;
        }
        [HttpPost("Product-Groups"), Authorize]
        public async Task<IActionResult> AddProductGroup(NewProductGroupDTO model)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u=>u.ServiceRegistered).FirstOrDefault();
            if (await _productGroupService.AddGroup(model,service))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpDelete("Product-Groups/{id}"), Authorize]
        public async Task<IActionResult> DeleteProductGroup([FromRoute] string id)
        {
            if (await _productGroupService.DeleteGroup(id))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpPut("Product-Groups"), Authorize]
        public async Task<IActionResult> UpdateProductGroup(NewProductGroupDTO productGroup)
        {

            if (await _productGroupService.UpdateGroup(productGroup))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpGet("Product-Groups"), Authorize]
        public async Task<IActionResult> ProductGroups()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u=>u.ServiceRegistered).FirstOrDefault();
            try
            {
                return Ok(await _productGroupService.GetAll(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
        [HttpGet("Product-Groups/{id}"), Authorize]
        public async Task<IActionResult> ProductGroup(string id)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                var result = await _productGroupService.GetGroupById(id, service);
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
