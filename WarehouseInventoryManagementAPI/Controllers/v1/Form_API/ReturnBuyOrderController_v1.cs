using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.DTO.Form.Change;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.Services.Form_Related_Service;

namespace WarehouseInventoryManagementAPI.Controllers.v1.Form_API
{
    [Route("api/v1/")]
    [ApiController]
    public class ReturnBuyOrderController_v1 : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly ReturnBuyFormService _returnBuyFormService;
        public ReturnBuyOrderController_v1(ApplicationDbContext context, UserManager<Account> userManager, ReturnBuyFormService returnBuyFormService)
        {
            _context = context;
            _userManager = userManager;
            _returnBuyFormService = returnBuyFormService;

        }
        [HttpPost("Return-Buy-Forms"), Authorize]
        public async Task<IActionResult> AddForm(NewFormDTO model)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            var result = model.UpdateStock.Value ? await _returnBuyFormService.NewFormImport(model, service) : await _returnBuyFormService.AddForm(model, service);
            if (result)
            {
                return Ok("OK");
            }
            else
                return BadRequest("Error");
        }
        [HttpDelete("Return-Buy-Forms/{id}"), Authorize]
        public async Task<IActionResult> DeleteForm([FromRoute] string id)
        {
            if (await _returnBuyFormService.DeleteForm(id))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpPut("Return-Buy-Forms"), Authorize]
        public async Task<IActionResult> UpdateForm(NewFormDTO model)
        {
            if (await _returnBuyFormService.UpdateForm(model))
                return Ok("OK");
            else
                return BadRequest("Error");
        }
        [HttpGet("Return-Buy-Forms"), Authorize]
        public async Task<IActionResult> Forms()
        {
            var service = _context.Users.Include(u => u.ServiceRegistered)
                                        .Where(u => u.UserName == User.Identity.Name)
                                        .Select(u => u.ServiceRegistered)
                                        .FirstOrDefault();
            try
            {
                return Ok(await _returnBuyFormService.GetAll(service));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
        [HttpGet("Return-Buy-Forms/{id}"), Authorize]
        public async Task<IActionResult> Form(string id)
        {
            var service = _context.Users.Include(u => u.ServiceRegistered).Where(u => u.UserName == User.Identity.Name).Select(u => u.ServiceRegistered).FirstOrDefault();
            try
            {
                var result = await _returnBuyFormService.FindById(service, id);
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
