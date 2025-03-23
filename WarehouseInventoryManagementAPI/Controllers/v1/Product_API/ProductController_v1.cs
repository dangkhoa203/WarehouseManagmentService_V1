using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Entity;

using WarehouseInventoryManagementAPI.Services.Product_Related_Service;
using WarehouseInventoryManagementAPI.DTO.Product.Change;
using WarehouseInventoryManagementAPI.Services.Customer_Related_Service;
namespace WarehouseInventoryManagementAPI.Controllers.v1.Product_API
{
    [Route("api/v1/")]
    [ApiController]
    public class ProductController_v1 : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly ProductService _productService;
        public ProductController_v1(ApplicationDbContext context, UserManager<Account> userManager, ProductService productService)
        {
            _context = context;
            _userManager = userManager;
            _productService = productService;
        }

        [HttpPost("Products"), Authorize]
        public async Task<IActionResult> AddProduct(NewProductDTO model)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            if (await _productService.Add(model, service))
            {
                return Ok("OK");
            }
            else
                return BadRequest("Error");
        }
        [HttpDelete("Products/{id}"), Authorize]
        public async Task<IActionResult> DeleteProducts([FromRoute] string id)
        {
            if (await _productService.Delete(id))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpPut("Products"), Authorize]
        public async Task<IActionResult> UpdateProduct(NewProductDTO product)
        {
            if (await _productService.Update(product))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpGet("Products"), Authorize]
        public async Task<IActionResult> Products()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered)
                                        .Where(u => u.UserName == User.Identity.Name)
                                        .Select(u=>u.ServiceRegistered)
                                        .FirstOrDefault();
            try
            { 
                return Ok(await _productService.GetAll(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }

        }
        [HttpGet("Products/{id}"), Authorize]
        public async Task<IActionResult> Products(string id)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                var result =await _productService.FindById(id,service);
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
