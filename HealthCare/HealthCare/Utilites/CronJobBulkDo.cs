using HealthCare.Domain.BuildingBlocks.Mail;
using HealthCare.Domain.Interfaces;

namespace HealthCare.Domain.BuildingBlocks.CronJobs
{
    public class CronJobBulkDo : CronJobService
    {
        public IServiceProvider _provider;
        public CronJobBulkDo(IScheduleConfig<CronJobNotifications> config, IServiceProvider serviceProvider) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _provider = serviceProvider;
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            using (IServiceScope scope = _provider.CreateScope())
            {
                ITransferService transferService = scope.ServiceProvider.GetRequiredService<ITransferService>();
                IRenovationService renovationService = scope.ServiceProvider.GetRequiredService<IRenovationService>();
                IEquipmentRequestService equipmentRequestService = scope.ServiceProvider.GetRequiredService<IEquipmentRequestService>();
                transferService.DoTransfers();
                renovationService.ExecuteComplexRenovations();
                equipmentRequestService.DoAllOrders();
            }
        }
    }
}
