using HealthCare.Domain.BuildingBlocks.Mail;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.BuildingBlocks.CronJobs
{
    public class CronJobNotifications : CronJobService
    {
        public IServiceProvider _provider;
        public CronJobNotifications(IScheduleConfig<CronJobNotifications> config, IServiceProvider serviceProvider) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _provider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        public override async Task DoWork(CancellationToken cancellationToken)
        {
            Console.WriteLine("radi");
            using (IServiceScope scope = _provider.CreateScope())
            {
                IPrescriptionService service = scope.ServiceProvider.GetRequiredService<IPrescriptionService>();
                List<string> emails = await service.GetAllReminders();
                Console.WriteLine(emails.Count);
                foreach (string item in emails)
                { 
                    MailSender sender = new MailSender("usi2022hospital@gmailcom", item);
                    sender.SetBody("Podsetnik za lek.");
                    sender.SetSubject("Uskoro morate popiti lek!");
                    sender.Send();
                }
            }
        }
    }
}
