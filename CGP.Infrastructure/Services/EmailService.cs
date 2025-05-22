using CGP.Application.Interfaces;
using CGP.Contract.DTO.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<bool> SendEmailAsync(EmailDTO request)
        {
            try
            {
                var apiKey = _configuration["Mailjet:ApiKey"];
                var apiSecret = _configuration["Mailjet:ApiSecret"];
                var fromEmail = _configuration["Mailjet:FromEmail"];
                var fromName = _configuration["Mailjet:FromName"];

                var options = new RestClientOptions("https://api.mailjet.com/v3.1/send")
                {
                    Authenticator = new HttpBasicAuthenticator(apiKey, apiSecret)
                };

                var client = new RestClient(options);

                var requestBody = new
                {
                    Messages = new[]
                    {
                new
                {
                    From = new { Email = fromEmail, Name = fromName },
                    To = new[] { new { Email = request.To, Name = request.To } },
                    Subject = request.Subject,
                    HTMLPart = request.Body
                }
                    }
                };

                var restRequest = new RestRequest()
                    .AddHeader("Content-Type", "application/json")
                    .AddJsonBody(JsonSerializer.Serialize(requestBody));

                var response = await client.ExecutePostAsync(restRequest);

                if (response.IsSuccessful)
                {
                    _logger.LogInformation("Email sent successfully to {Recipient}", request.To);
                    return true;
                }

                _logger.LogError("Failed to send email: {Error}", response.Content);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error sending email: {Exception}", ex.Message);
                return false;
            }
        }



        public async Task SendVerificationEmailAsync(string email, string token)
        {
            var verificationUrl = $"https://localhost:7238/api/auth/verify?token={token}";
            var emailDto = new EmailDTO
            {
                To = email,
                Subject = "Email Verification",
                Body =
                    $"Please verify your email by clicking on the following link: <a href='{verificationUrl}'>Verify Email</a>"
            };

            await SendEmailAsync(emailDto);
        }

        public async Task SendOtpEmailAsync(string email, string otp)
        {
            var emailDto = new EmailDTO
            {
                To = email,
                Subject = "Email Verification OTP",
                Body = $"Your OTP for email verification is: {otp}"
            };

            await SendEmailAsync(emailDto);
        }

        //PENDING MAIL
        public async Task SendPendingEmailAsync(string email)
        {
            var emailDto = new EmailDTO
            {
                To = email,
                Subject = "Account Pending Approval",
                Body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                        <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                            <h2 style='color: #333;'>Thank you for registering as an shop!</h2>
                            <p style='color: #555;'>We have received your registration and it is currently being reviewed by our admin team.</p>
                            <p style='color: #555;'>You will receive a notification once your account is approved. Please be patient during this process.</p>
                            <p style='color: #555;'>Thank you for your understanding.</p>
                            <p style='color: #555;'>Best regards,<br />ProManager</p>
                        </div>
                    </body>
                    </html>"
            };

            await SendEmailAsync(emailDto);
        }
        public async Task SendActiveEmailAsync(string email)
        {
            var emailDto = new EmailDTO
            {
                To = email,
                Subject = "Account Reactivation",
                Body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                    <h2 style='color: #333;'>Welcome Back!</h2>
                    <p style='color: #555;'>We are excited to inform you that your account has been reactivated. You can now log in and continue using our system.</p>
                    <p style='color: #555;'>Thank you for being a valued member of our community.</p>
                    <p style='color: #555;'>Best regards,<br />ProManger</p>
                </div>
            </body>
            </html>"
            };

            await SendEmailAsync(emailDto);
        }

        public async Task SendDeactiveEmailAsync(string email, string reason)
        {
            var emailDto = new EmailDTO
            {
                To = email,
                Subject = "Account Deactivation Notice",
                Body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                    <h2 style='color: #333;'>Account Deactivation</h2>
                    <p style='color: #555;'>We regret to inform you that your account has been deactivated.</p>
                    <p style='color: #555;'>Reason: {reason}</p>
                    <p style='color: #555;'>If you have any questions or need further assistance, please contact our support team.</p>
                    <p style='color: #555;'>We appreciate your understanding.</p>
                    <p style='color: #555;'>Best regards,<br />ProManager</p>
                </div>
            </body>
            </html>"
            };

            await SendEmailAsync(emailDto);
        }
        public async Task SendRejectionEmailAsync(string email, string reason)
        {
            var emailDto = new EmailDTO
            {
                To = email,
                Subject = "Instructor Rejection",
                Body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                        <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                            <h2 style='color: #333;'>Registration Update</h2>
                            <p style='color: #555;'>We regret to inform you that your account registration has been rejected.</p>
                            <p style='color: #555;'>Reason: {reason}</p>
                            <p style='color: #555;'>If you have any questions or need further assistance, please contact our support team.</p>
                            <p style='color: #555;'>Best regards,<br />ProManager</p>
                        </div>
                    </body>
                    </html>"
            };

            await SendEmailAsync(emailDto);
        }

        public async Task SendApprovalEmailAsync(string email)
        {
            var emailDto = new EmailDTO
            {
                To = email,
                Subject = "Account Approval",
                Body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                        <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                            <h2 style='color: #333;'>Congratulations!</h2>
                            <p style='color: #555;'>Your account has been approved. You can now log in and start using the system.</p>
                            <p style='color: #555;'>Thank you for joining our system.</p>
                            <p style='color: #555;'>Best regards,<br />ProManager</p>
                        </div>
                    </body>
                    </html>"
            };

            await SendEmailAsync(emailDto);
        }

        public async Task SendApprovalServiceAsync(string email)
        {
            var emailDto = new EmailDTO
            {
                To = email,
                Subject = "Service Approval",
                Body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                        <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                            <h2 style='color: #333;'>Congratulations!</h2>
                            <p style='color: #555;'>Your service has been approved.</p>
                            <p style='color: #555;'>Thank you for use my website.</p>
                            <p style='color: #555;'>Best regards,<br />ProManager</p>
                        </div>
                    </body>
                    </html>"
            };

            await SendEmailAsync(emailDto);
        }

        public async Task SendRejectServiceAsync(string email, string reason)
        {
            var emailDto = new EmailDTO
            {
                To = email,
                Subject = "Service Rejection",
                Body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                        <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                            <h2 style='color: #333;'>Service Rejection</h2>
                            <p style='color: #555;'>We regret to inform you that your service registration has been rejected.</p>
                            <p style='color: #555;'>Reason: {reason}</p>
                            <p style='color: #555;'>If you have any questions or need further assistance, please contact our support team.</p>
                            <p style='color: #555;'>Best regards,<br />ProManager</p>
                        </div>
                    </body>
                    </html>"
            };

            await SendEmailAsync(emailDto);
        }
    }
}
