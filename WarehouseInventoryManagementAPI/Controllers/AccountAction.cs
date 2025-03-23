using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseInventoryManagementAPI.Data;
using WarehouseInventoryManagementAPI.Model.Entity;
using WarehouseInventoryManagementAPI.DTO.Account;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WarehouseInventoryManagementAPI.Service;

namespace WarehouseInventoryManagementAPI.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountAction : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        public AccountAction(ApplicationDbContext context, UserManager<Account> userManager, SignInManager<Account> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest("Đã đăng nhập");
            }
            if(await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return BadRequest("Email đã có người đăng ký!");
            }
            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                return BadRequest("Tên đăng nhập đã có người đăng ký!");
            }
            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest("Mật khẩu không giống với mật khẩu xác nhận!");
            }
            ServiceRegistered serviceRegistered = new ServiceRegistered();
            Account account = new()
            {
                Email = model.Email,
                UserName = model.UserName,
                FullName = model.FullName,
                ServiceRegistered = serviceRegistered,
            };
            var result = await _userManager.CreateAsync(account, model.Password);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                user.EmailConfirmed = true;
                await _context.SaveChangesAsync();
                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                //EmailService emailService = new();
                //string hostname = $"{Request.Scheme}://localhost:7088";
                //var ConfirmLink = $"{hostname}/ConfirmEmail/{WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.UserName))}/{token}";
                //bool emailResponse = await emailService.SendConfirmEmailEmail(user.Email, ConfirmLink);
                //if (!emailResponse)
                //{
                //    _context.Users.Remove(user);
                //    await _context.SaveChangesAsync();
                //    return BadRequest("Lỗi đã xảy ra!");
                //}
                return Ok("Email xác nhận tài khoản đã gửi vào email của bạn!");
            }
            return BadRequest("Lỗi đã xảy ra");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest("Đã đăng nhập");
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.Remember, false);
                if (result.Succeeded)
                {
                    return Ok("OK");
                }
                return BadRequest("Hãy kiểm tra lại thông tin đăng nhập");
            }
            return BadRequest("Hãy kiểm tra lại thông tin đăng nhập");
        }
        [HttpGet("Logout"), Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi đã xảy ra");
            }
            return Ok("Logout thàn công!");
        }

        [HttpPost("ConfirmAccount/{Username}/{code}")]
        public async Task<IActionResult> ConfirmEmail([FromRoute] string Username, [FromRoute] string code)
        {
            Account user = await _userManager.FindByNameAsync(Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Username)));
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    return BadRequest("Đã xác nhận tài khoản.");
                }
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (!result.Succeeded)
                {
                    return BadRequest("Lỗi đã xảy ra");
                }
            }
            else
            {
                return NotFound("Người dùng không tìm thấy");
            }
            return Ok("Xác nhận thành công");
        }
        [HttpGet("Account")]
        public async Task<IActionResult> getAccount()
        {
            LogedInUser logeduser;

            if (User.Identity.Name != null)
            {
                Account user = await _userManager.FindByNameAsync(User.Identity.Name);
                await _signInManager.RefreshSignInAsync(user);
                logeduser = new LogedInUser { UserEmail = user.Email, UserId = user.Id, UserName = user.UserName, UserFullName = user.FullName, isLogged = true };
            }
            else
                logeduser = new();
            return Ok(logeduser);
        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> forgetPassword([FromBody] string email)
        {
            if (email.Length == 0)
            {
                return BadRequest("Bạn chưa nhập thông tin đầy đủ!");
            }
            Account user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                EmailService emailService = new();
                string hostname = $"{Request.Scheme}://localhost:7088";
                var EmailLink = $"{hostname}/ResetPassword/{WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Id))}/{token}";
                bool emailResponse = await emailService.SendConfirmPasswordResetEmail(user.Email, EmailLink);
                if (!emailResponse)
                {
                    return BadRequest("Email not send");
                }
                return Ok("Email đặt lại mật khẩu đã được gửi tới Email đăng ký!");
            }
            return NotFound("Không có tài khoản với email này!");
        }

        [HttpPost("CheckForReset/{userid}")]
        public async Task<IActionResult> CheckForReset(string userid)
        {
            Account user = await _userManager.FindByIdAsync(Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userid)));
            if (user != null)
            {
                return Ok(true);
            }
            return Ok(false);
        }

        [HttpPut("ResetPassword/{userid}/{code}")]
        public async Task<IActionResult> ResetPassword(string userid, [FromBody] string newpassword, string code)
        {
            Account user = await _userManager.FindByIdAsync(Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userid)));
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ResetPasswordAsync(user, code, newpassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("Password change");
        }
        [HttpPost("CheckPasswordForChange"), Authorize]
        public async Task<IActionResult> CheckPassword([FromBody] string password)
        {
            Account user = await _userManager.FindByNameAsync(User.Identity.Name);
            PasswordValidator<Account> PassCheck = new();
            var result = await PassCheck.ValidateAsync(_userManager, user, password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description).ToList());
            }
            return Ok("Check OK");
        }

        [HttpPost("SendChangePasswordEmail"),Authorize]
        public async Task<IActionResult> SendChangePasswordEmail(ChangePassWordDTO model)
        {
            Account user = await _userManager.FindByNameAsync(User.Identity.Name);

            var result = await _userManager.CheckPasswordAsync(user, model.OldPassword);
            if (!result)
            {
                return BadRequest("Mật khẩu cũ không đúng!");
            }
            if (model.NewPassword.Length < 4)
            {
                return BadRequest("Mật khẩu mới không đúng!");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            string password = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(model.NewPassword));
            EmailService emailService = new();
            string hostname = $"{Request.Scheme}://localhost:7088";
            var EmailLink = $"{hostname}/ConfirmDoiMatKhau/{WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Id))}/{password}/{token}";
            bool emailResponse = await emailService.SendConfirmPasswordChangeEmail(user.Email, EmailLink);
            if (!emailResponse)
            {
                return BadRequest("Email not send");
            }
            return Ok("Send successfully");
        }

        [HttpPut("ChangePassword/{userid}/{newPassword}/{code}")]
        public async Task<IActionResult> ChangePassword(string userid, string newPassword, string code)
        {

            Account user = await _userManager.FindByIdAsync(Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userid)));
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            newPassword = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(newPassword));
            var result = await _userManager.ResetPasswordAsync(user, code, newPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("Password change");
        }

        //Email Related
        [HttpPost("SendChangeEmail"), Authorize]
        public async Task<IActionResult> SendChangeEmail([FromBody] string newEmail)
        {
            if (await _userManager.FindByEmailAsync(newEmail) != null)
            {
                return BadRequest("Email đang được sử dụng");
            }
            Account user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (newEmail == user.Email)
            {
                return BadRequest("Same email!");
            }
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            string WebEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(newEmail));
            EmailService emailService = new();
            string hostname = $"{Request.Scheme}://localhost:7088";
            var EmailLink = $"{hostname}/ConfirmDoiEmail/{WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Id))}/{WebEmail}/{token}";
            bool emailResponse = await emailService.SendConfirmEmailEmail(newEmail, EmailLink);
            if (!emailResponse)
            {
                return BadRequest("Email not send");
            }
            return Ok("Email xác nhận đã được gửi tới email mới!");
        }

        [HttpPut("ChangeEmail/{userid}/{newEmail}/{code}")]
        public async Task<IActionResult> ChangeEmail(string userid, string newEmail, string code)
        {
            Account user = await _userManager.FindByIdAsync(Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userid)));
            if (user != null)
            {
                newEmail = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(newEmail));
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = _userManager.ChangeEmailAsync(user, newEmail, code);
                if (!result.Result.Succeeded)
                {
                    return BadRequest(result.Result.Errors);
                }
                return Ok("Email have been change");
            }
            return BadRequest("Something went wrong!");
        }

        //Name Related
        [HttpPut("ChangeFullName/"), Authorize]
        public async Task<IActionResult> ChangeName([FromBody]string newname)
        {
            Account user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (newname.Length <= 0)
            {
                return BadRequest("Naming Error!");
            }
            user.FullName = newname;
            var result = _userManager.UpdateAsync(user);
            if (!result.Result.Succeeded)
            {
                return BadRequest("Something went wrong!");
            }
            LogedInUser logedInDetail = new()
            {
                isLogged = true,
                UserEmail = user.Email,
                UserName = user.UserName,
                UserFullName = user.FullName,
                UserId = user.Id
            };
            return Ok(new { message = "Change successfully", newUserInfo = logedInDetail });
        }
    }
}
