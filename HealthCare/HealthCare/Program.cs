using HealthCare.Data.Context;
using HealthCare.Domain.BuildingBlocks.CronJobs;
using HealthCare.Domain.BuildingBlocks.Mail;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Domain.Services;
using HealthCare.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
//builder.Services.AddControllers().AddJsonOptions(x =>
//   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);


//Repositories
builder.Services.AddTransient<IAntiTrollRepository, AntiTrollRepository>();
builder.Services.AddTransient<IAnamnesisRepository, AnamnesisRepository>();
builder.Services.AddTransient<IAllergyRepository, AllergyRepository>();
builder.Services.AddTransient<ICredentialsRepository, CredentialsRepository>();
builder.Services.AddTransient<IDaysOffRequestRepository, DaysOffRequesRepository>();
builder.Services.AddTransient<IDoctorRepository, DoctorRepository>();
builder.Services.AddTransient<IDrugRepository, DrugRepository>();
builder.Services.AddTransient<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddTransient<IEquipmentTypeRepository, EquipmentTypeRepository>();
builder.Services.AddTransient<IExaminationRepository, ExaminationRepository>();
builder.Services.AddTransient<IExaminationApprovalRepository, ExaminationApprovalRepository>();
builder.Services.AddTransient<IIngredientRepository, IngredientRepository>();
builder.Services.AddTransient<IInventoryRepository, InventioryRepository>();
builder.Services.AddTransient<IManagerRepository, ManagerRepository>();
builder.Services.AddTransient<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddTransient<IOperationRepository, OperationRepository>();
builder.Services.AddTransient<IPatientRepository, PatientRepository>();
builder.Services.AddTransient<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddTransient<IReferralLetterRepository, ReferralLetterRepository>();
builder.Services.AddTransient<IRoomRepository, RoomRepository>();
builder.Services.AddTransient<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddTransient<ISecretaryRepository, SecretaryRepository>();
builder.Services.AddTransient<ISpecializationRepository, SpecializationRepository>();
builder.Services.AddTransient<ITransferRepository, TransferRepository>();
builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddTransient<IJoinRenovationRepository, JoinRenovationRepository>();
builder.Services.AddTransient<ISimpleRenovationRepository, SimpleRenovationRepository>();
builder.Services.AddTransient<ISplitRenovationRepository, SplitRenovationRepository>();
builder.Services.AddTransient<INotificationRepository, NotificationRepository>();
builder.Services.AddTransient<IEquipmentRequestRepository, EquipmentRequestRepository>();
builder.Services.AddTransient<IDrugSuggestionRepository, DrugSuggestionRepository>();
builder.Services.AddTransient<IDrugIngredientRepository, DrugIngredientRepository>();
builder.Services.AddTransient<IAnswerRepository, AnswerRepository>();
builder.Services.AddTransient<IQuestionRepository, QuestionRepository>();

//Domain
builder.Services.AddTransient<IAntiTrollService, AntiTrollService>();
builder.Services.AddTransient<IAnamnesisService, AnamnesisService>();
builder.Services.AddTransient<IAllergyService, AllergyService>();
builder.Services.AddTransient<IAppointmentService, AppointmentService>();
builder.Services.AddTransient<ICredentialsService, CredentialsService>();
builder.Services.AddTransient<IDaysOffRequestService, DaysOffRequestService>();
builder.Services.AddTransient<IDoctorService, DoctorService>();
builder.Services.AddTransient<IDrugService, DrugService>();
builder.Services.AddTransient<IDrugIngredientService, DrugIngredientService>();
builder.Services.AddTransient<IEquipmentService, EquipmentService>();
builder.Services.AddTransient<IEquipmentTypeService, EquipmentTypeService>();
builder.Services.AddTransient<IExaminationApprovalService, ExaminationApprovalService>();
builder.Services.AddTransient<IExaminationService, ExaminationService>();
builder.Services.AddTransient<IIngredientService, IngredientService>();
builder.Services.AddTransient<IInventoryService, InventoryService>();
builder.Services.AddTransient<IManagerService, ManagerService>();
builder.Services.AddTransient<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddTransient<IOperationService, OperationService>();
builder.Services.AddTransient<IPatientService, PatientService>();
builder.Services.AddTransient<IPrescriptionService, PrescriptionService>();
builder.Services.AddTransient<IReferralLetterService, ReferralLetterService>();
builder.Services.AddTransient<IRoomService, RoomService>();
builder.Services.AddTransient<IRoomTypeService, RoomTypeService>();
builder.Services.AddTransient<ISecretaryService, SecretaryService>();
builder.Services.AddTransient<ISpecializationService, SpecializationService>();
builder.Services.AddTransient<ITransferService, TransferService>();
builder.Services.AddTransient<IUserRoleService, UserRoleService>();
builder.Services.AddTransient<IRenovationService, RenovationService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<IEquipmentRequestService, EquipmentRequestService>();
builder.Services.AddTransient<IDrugSuggestionService, DrugSuggestionService>();
builder.Services.AddTransient<IAnswerService, AnswerService>();
builder.Services.AddTransient<IQuestionService, QuestionService>();
builder.Services.AddTransient<IAvailabilityService, AvailabilityService>();
builder.Services.AddTransient<IFilteringExaminationService, FilteringExaminationService>();
builder.Services.AddTransient<IRecommendExaminationService, RecommendExaminationService>();
builder.Services.AddTransient<ISurveyService, SurveyService>();
builder.Services.AddTransient<ISimpleRenovationService, SimpleRenovationService>();
builder.Services.AddTransient<ISplitRenovationService, SplitRenovationService>();
builder.Services.AddTransient<IJoinRenovationService, JoinRenovationService>();
builder.Services.AddTransient<IUrgentAppointmentService, UrgentAppointmentService>();


var connectionString = builder.Configuration.GetConnectionString("HealthCareConnection");
builder.Services.AddDbContext<HealthCareContext>(x => x.UseSqlServer(connectionString));
//builder.Services.AddDbContext<HealthCareContext>(x => x.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
builder.Services.AddDbContext<HealthCareContext>(x => x.EnableSensitiveDataLogging());

//builder.Services.AddSingleton<CronJobNotifications>();

//builder.Services.AddCors(options => 
//{
//    options.AddPolicy("CorsPolicy", 
//        corsBuilder => corsBuilder.WithOrigins("http://localhost:7195").AllowAnyMethod()
//           .AllowAnyHeader()
//            .AllowCredentials());
//});

builder.Services.AddCors(feature =>
                feature.AddPolicy(
                    "CorsPolicy",
                    apiPolicy => apiPolicy
                                    //.AllowAnyOrigin()
                                    //.WithOrigins("http://localhost:4200")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .SetIsOriginAllowed(host => true)
                                    .AllowCredentials()
                                ));


//builder.Services.AddCronJob<CronJobNotifications>(c =>
//{
//    c.TimeZoneInfo = TimeZoneInfo.Local;
//    c.CronExpression = @"*/5 * * * *";
//});
//MailSender sender = new MailSender("usi2022hospital@gmailcom", "lazzarmilanovic@gmail.com");
//sender.SetBody("test");
//sender.SetSubject("test");
//sender.Send();


// Cron jobs
builder.Services.AddCronJob<CronJobNotifications>(c =>
{
    c.TimeZoneInfo = TimeZoneInfo.Local;
    c.CronExpression = @"* * * * *";
});


builder.Services.AddCronJob<CronJobBulkDo>(c =>
{
    c.TimeZoneInfo = TimeZoneInfo.Local;
    c.CronExpression = @"0 1-23/2 * * *";
});


var app = builder.Build();


// Configure the HTTP request pipeline.`
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    
}
else 
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());
//app.MapRazorPages();




app.Run();
