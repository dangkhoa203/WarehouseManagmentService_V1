using System.Net.Mail;
using System.Net;
using System.Net.Http;

namespace WarehouseInventoryManagementAPI.Service {
    public class EmailService {
        public EmailService()
        {
            
        }
        public async Task<bool> SendConfirmEmailEmail(string toEmail, string Body) {
            try {
                using (SmtpClient client = new SmtpClient("smtp.gmail.com")) {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("dkwebsoftware@gmail.com", "tnmx nqah rtlq ekwp");
                    client.EnableSsl = true;
                    using (var message = new MailMessage("dkwebsoftware@gmail.com", toEmail)) {
                        message.Subject = "Xác nhận tài khoản.";
                        message.Body = $"Xác nhận tài khoản qua địa chỉ <a href='{Body}'>sau</a>.";
                        message.BodyEncoding = System.Text.Encoding.UTF8;
                        message.SubjectEncoding = System.Text.Encoding.UTF8;
                        message.IsBodyHtml = true;
                        await client.SendMailAsync(message);
                    }
                    return true;
                }
            }
            catch (Exception ex) {
                return false;
            }
        }
        public async Task<bool> SendConfirmPasswordResetEmail(string toEmail, string Body) {
            try {
                using (SmtpClient client = new SmtpClient("smtp.gmail.com")) {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("dkwebsoftware@gmail.com", "tnmx nqah rtlq ekwp");
                    client.EnableSsl = true;
                    using (var message = new MailMessage("dkwebsoftware@gmail.com", toEmail)) {
                        message.Subject = "Đặt lại mật khẩu.";
                        message.Body = $"Vào địa chỉ <a href='{Body}'>này</a> để đặt lại mật khẩu.";
                        message.BodyEncoding = System.Text.Encoding.UTF8;
                        message.SubjectEncoding = System.Text.Encoding.UTF8;
                        message.IsBodyHtml = true;
                        await client.SendMailAsync(message);
                    }
                    return true;
                }
            }
            catch (Exception ex) {
                return false;
            }
        }
        public async Task<bool> SendConfirmEmailChangeEmail(string toEmail, string Body) {
            try {
                using (SmtpClient client = new SmtpClient("smtp.gmail.com")) {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("dkwebsoftware@gmail.com", "tnmx nqah rtlq ekwp");
                    client.EnableSsl = true;
                    using (var message = new MailMessage("dkwebsoftware@gmail.com", toEmail)) {
                        message.Subject = "Thay đổi email.";
                        message.Body = $"Xác nhận email mới của bạn tại <a href='{Body}'>đây</a>.";
                        message.BodyEncoding = System.Text.Encoding.UTF8;
                        message.SubjectEncoding = System.Text.Encoding.UTF8;
                        message.IsBodyHtml = true;
                        await client.SendMailAsync(message);
                    }
                    return true;
                }
            }
            catch (Exception ex) {
                return false;
            }
        }
        public async Task<bool> SendConfirmPasswordChangeEmail(string toEmail, string Body) {
            try {
                using (SmtpClient client = new SmtpClient("smtp.gmail.com")) {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("dkwebsoftware@gmail.com", "tnmx nqah rtlq ekwp");
                    client.EnableSsl = true;
                    using (var message = new MailMessage("dkwebsoftware@gmail.com", toEmail)) {
                        message.Subject = "Thay đổi mật khẩu.";
                        message.Body = $"Xác nhận thay đổi mật khẩu tại <a href='{Body}'>đây</a>.";
                        message.BodyEncoding = System.Text.Encoding.UTF8;
                        message.SubjectEncoding = System.Text.Encoding.UTF8;
                        message.IsBodyHtml = true;
                        await client.SendMailAsync(message);
                    }
                    return true;
                }
            }
            catch (Exception ex) {
                return false;
            }
        }
    }
}
