using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace DataModel.Models.EntityManager
{
    public class EmailManager
    {
        public async Task<bool> EmailSend(string body, string toEmailAddress)
        {
            try
            {
               
                var message = new MailMessage();
                message.To.Add(new MailAddress(toEmailAddress));  // replace with valid value 
                message.From = new MailAddress("dohateccasupprot@gmail.com");  // replace with valid value
                message.Subject = "Your email subject";
                string fullbody = @"<div style='width:300px;height:400px;border:1px solid #EFEFEF'> 
                                        <div style='height:50px;padding:10px;background-color:#0095D6;color:#FFF'><h3>Email From DData<h3></div>                   
                                        <div style='padding:10px;font-size:14px;color:#222''>Hello!<br/><br/>Thank you for using DData.<br/>" + body + "<br/><br/>All the best!!</div></div>";
                message.Body = string.Format(fullbody);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "dohateccasupprot@gmail.com",  // replace with valid value
                        Password = "Dohatec123"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return true;

                }
            }
            catch{
                return false;
            }

        }
    }
}
