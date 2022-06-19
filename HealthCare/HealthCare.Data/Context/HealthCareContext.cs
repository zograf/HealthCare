using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Data.Context
{
    public class HealthCareContext: DbContext
    {
        public DbSet<AntiTroll> AntiTrolls { get; set; }
        public DbSet<Anamnesis> Anamneses { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<Credentials> Credentials { get; set; }
        public DbSet<DaysOffRequest> DaysOffRequests { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<DrugIngredient> DrugIngredients { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Examination> Examinations { get; set; }
        public DbSet<ExaminationApproval> ExaminationApprovals { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<ReferralLetter> ReferralLetters { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Secretary> Secretaries { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<SimpleRenovation> SimpleRenovations{ get; set; }
        public DbSet<JoinRenovation> JoinRenovations { get; set; }
        public DbSet<SplitRenovation> SplitRenovations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<EquipmentRequest> EquipmentRequests { get; set; }
        public DbSet<DrugSuggestion> DrugSuggestions{ get; set; }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions{ get; set; }

        public HealthCareContext(DbContextOptions options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Anamnesis>()
                .HasOne(x => x.Examination)
                .WithOne(x => x.Anamnesis)
                .IsRequired();
            modelBuilder.Entity<Examination>()
                .HasOne(x => x.Anamnesis)
                .WithOne(x => x.Examination)
                .IsRequired(false);
        
            //TODO: Sort this?
            modelBuilder.Entity<MedicalRecord>()
                .HasMany(x => x.ReferralLetters)
                .WithOne(x => x.MedicalRecord)
                .HasForeignKey(x => x.PatientId);

            modelBuilder.Entity<MedicalRecord>()
                .HasMany(x => x.Prescriptions)
                .WithOne(x => x.MedicalRecord)
                .HasForeignKey(x => x.PatientId);

            modelBuilder.Entity<MedicalRecord>()
                .HasMany(x => x.AllergiesList)
                .WithOne(x => x.MedicalRecord)
                .HasForeignKey(x => x.PatientId);

            modelBuilder.Entity<DrugIngredient>()
                .HasKey(x => new { x.IngredientId, x.DrugId });

            modelBuilder.Entity<DrugIngredient>()
                .HasOne(x => x.Ingredient)
                .WithMany(i => i.DrugIngredients)
                .HasForeignKey(x => x.IngredientId);

            modelBuilder.Entity<Allergy>()
                .HasOne(x => x.Ingredient);


            modelBuilder.Entity<DrugIngredient>()
                .HasOne(x => x.Drug)
                .WithMany(d => d.DrugIngredients)
                .HasForeignKey(x => x.DrugId);

            modelBuilder.Entity<DrugSuggestion>()
                .HasOne(x => x.Drug);

            modelBuilder.Entity<Prescription>()
                .HasOne(x => x.Drug);
            
            modelBuilder.Entity<EquipmentRequest>()
                .HasOne(x => x.Equipment);
            //add drug suggestion
            // add questions and answers


            modelBuilder.Entity<Anamnesis>().HasKey(x => x.Id);
            modelBuilder.Entity<Drug>().HasKey(x => x.Id);
            modelBuilder.Entity<DaysOffRequest>().HasKey(x => x.Id);
            modelBuilder.Entity<DrugIngredient>().HasKey(x => new {x.DrugId, x.IngredientId});
            modelBuilder.Entity<Allergy>().HasKey(x => new { x.PatientId, x.IngredientId });
            modelBuilder.Entity<Examination>().HasKey(x => x.Id);
            modelBuilder.Entity<ExaminationApproval>().HasKey(x => x.Id);
            modelBuilder.Entity<Inventory>().HasKey(x => new {x.RoomId, x.EquipmentId });
            modelBuilder.Entity<Transfer>().HasKey(x => new {x.Id});
            modelBuilder.Entity<Operation>().HasKey(x => new {x.Id});
            modelBuilder.Entity<MedicalRecord>().HasKey(x => x.PatientId);
            modelBuilder.Entity<EquipmentType>().HasKey(x => x.Id);
            modelBuilder.Entity<Equipment>().HasKey(x => x.Id);
            modelBuilder.Entity<Prescription>().HasKey(x => x.Id);
            modelBuilder.Entity<Room>().HasKey(x => x.Id);
            modelBuilder.Entity<RoomType>().HasKey(x => x.Id);
            modelBuilder.Entity<ReferralLetter>().HasKey(x => x.Id);
            modelBuilder.Entity<Ingredient>().HasKey(x => x.Id);
            modelBuilder.Entity<Specialization>().HasKey(x => x.Id);
            modelBuilder.Entity<SimpleRenovation>().HasKey(x => x.Id);
            modelBuilder.Entity<JoinRenovation>().HasKey(x => x.Id);
            modelBuilder.Entity<SplitRenovation>().HasKey(x => x.Id);
            modelBuilder.Entity<Notification>().HasKey(x => x.Id);
            modelBuilder.Entity<EquipmentRequest>().HasKey(x => x.Id);
            modelBuilder.Entity<DrugSuggestion>().HasKey(x => x.Id);
            modelBuilder.Entity<Answer>().HasKey(x => x.Id);
            modelBuilder.Entity<Question>().HasKey(x => x.Id);

        }
    }
}
