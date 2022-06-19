using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.BuildingBlocks.Mail
{
    public class MailSender
    {
        private static SmtpClient _client = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            EnableSsl = true
        };
        private string _body;
        private string _subject;
        private string _from;
        private string _to;

        static bool mailSent = false;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }

        public MailSender(string from, string to)
        {
            _client.UseDefaultCredentials = false;
            _client.Credentials = new System.Net.NetworkCredential("usi2022Hospital@gmail.com", "usi2022!");
            _from = from;
            _to = to;
        }

        public void SetBody(string body)
        {
            _body = body;
        }
        public void SetSubject(string subject)
        {
            _subject = subject;
        }

        public bool Send()
        {
            //MailAddress from = new MailAddress(_from);
            //MailAddress to = new MailAddress(_to);
            //MailMessage message = new MailMessage(from, to);
            //message.Body = _body;
            //message.BodyEncoding = System.Text.Encoding.UTF8;
            //message.Subject = _subject;
            //message.SubjectEncoding = System.Text.Encoding.UTF8;
            //string userState = "reminder";
            //_client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            //_client.SendAsync(message, userState);
            //message.Dispose();
            _client.Send(_from, _to, _subject, _body);
            return false;
        }

    }
}
