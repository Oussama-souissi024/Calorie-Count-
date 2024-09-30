using System.Net.Mail;
using System.Net;
using CalorieCount.Core.Interfaces;

namespace CalorieCount.Services
{
	public class EmailSenderServices : IEmailSender
	{
		private readonly string _smtpServer = "smtp.gmail.com";
		private readonly int _smtpPort = 587;
		private readonly string _smtpUsername = "oussama.souissi024@gmail.com";
		private readonly string _smtpPassword = "metk oojb yzqa ddqo"; 

		public async Task SendEmailAsync(string email, string subject, string message)
		{
			var mailMessage = new MailMessage
			{
				From = new MailAddress(_smtpUsername, "CalorieCount"),
				Subject = subject,
				Body = message,
				IsBodyHtml = true
			};

			mailMessage.To.Add(email);

			try
			{
				using var smtpClient = new SmtpClient(_smtpServer, _smtpPort)
				{
					Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
					EnableSsl = true // Enable SSL for secure communication
				};

				await smtpClient.SendMailAsync(mailMessage);
				Console.WriteLine("Email sent successfully.");
			}
			catch (SmtpException smtpEx)
			{
				// Log the specific SMTP error with status code and message
				Console.WriteLine($"SMTP error: {smtpEx.StatusCode} - {smtpEx.Message}");
				throw;
			}
			catch (Exception ex)
			{
				// Handle any other general exceptions
				Console.WriteLine($"An error occurred: {ex.Message}");
				throw;
			}
		}
	}
}
