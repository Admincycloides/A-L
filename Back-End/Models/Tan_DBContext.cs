using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AnL.Models
{
    public partial class Tan_DBContext : DbContext
    {
        public Tan_DBContext()
        {
        }

        public Tan_DBContext(DbContextOptions<Tan_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActivityDetails> ActivityDetails { get; set; }
        public virtual DbSet<ActivityMapping> ActivityMapping { get; set; }
        public virtual DbSet<ClientDetails> ClientDetails { get; set; }
        public virtual DbSet<EmployeeDetails> EmployeeDetails { get; set; }
        public virtual DbSet<ProjectDetails> ProjectDetails { get; set; }
        public virtual DbSet<TimesheetDetails> TimesheetDetails { get; set; }
        public virtual DbSet<UserData> UserData { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Tan_DB;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityDetails>(entity =>
            {
                entity.HasKey(e => e.ActivityId)
                    .HasName("PK_Activity Details");

                entity.ToTable("Activity_Details");

                entity.Property(e => e.ActivityId)
                    .HasColumnName("Activity_ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActivityDescription)
                    .HasColumnName("Activity_Description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ActivityName)
                    .HasColumnName("Activity_Name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EnabledFlag)
                    .HasColumnName("Enabled_Flag")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ActivityMapping>(entity =>
            {
                entity.HasKey(e => e.UniqueId);

                entity.ToTable("Activity Mapping");

                entity.Property(e => e.UniqueId)
                    .HasColumnName("Unique ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActivityId).HasColumnName("Activity ID");

                entity.Property(e => e.ProjectId).HasColumnName("Project ID");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.ActivityMapping)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Activity Details-ActivityMap");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ActivityMapping)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Project Details-ActivityMap");
            });

            modelBuilder.Entity<ClientDetails>(entity =>
            {
                entity.HasKey(e => e.ClientId);

                entity.ToTable("Client Details");

                entity.Property(e => e.ClientId)
                    .HasColumnName("Client ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasColumnName("Client Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactEmailAddress)
                    .HasColumnName("Contact Email Address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactNumber)
                    .HasColumnName("Contact Number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PointOfContactName)
                    .HasColumnName("Point of Contact Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmployeeDetails>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK_Employee_ID");

                entity.ToTable("Employee_Details");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("Employee_ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactNumber)
                    .IsRequired()
                    .HasColumnName("Contact_Number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasColumnName("Email_Address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EnabledFlag)
                    .HasColumnName("Enabled_Flag")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("First_Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("Last_Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ManagerId)
                    .IsRequired()
                    .HasColumnName("Manager_ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SupervisorFlag)
                    .HasColumnName("Supervisor_Flag")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProjectDetails>(entity =>
            {
                entity.HasKey(e => e.ProjectId)
                    .HasName("PK_Project Details");

                entity.ToTable("Project_Details");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("Project_ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ClientId).HasColumnName("Client_ID");

                entity.Property(e => e.CurrentStatus)
                    .IsRequired()
                    .HasColumnName("Current_Status")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EnabledFlag)
                    .IsRequired()
                    .HasColumnName("Enabled_Flag")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate)
                    .HasColumnName("End_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ProjectDescription)
                    .IsRequired()
                    .HasColumnName("Project_Description")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectName)
                    .HasColumnName("Project_Name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SredProject)
                    .IsRequired()
                    .HasColumnName("SRED_Project")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate)
                    .HasColumnName("Start_Date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ProjectDetails)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Client Details-ProjectDet");
            });

            modelBuilder.Entity<TimesheetDetails>(entity =>
            {
                entity.HasKey(e => e.UniqueId)
                    .HasName("PK_Timesheet Details");

                entity.ToTable("Timesheet_Details");

                entity.Property(e => e.UniqueId).HasColumnName("Unique_ID");

                entity.Property(e => e.ActivityId).HasColumnName("Activity_ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("Employee_ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumberOfHours).HasColumnName("Number_of_Hours");

                entity.Property(e => e.ProjectId).HasColumnName("Project_ID");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.TimesheetDetails)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Activity Details-timesheet");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TimesheetDetails)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Project Details-timesheet");
            });

            modelBuilder.Entity<UserData>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Otp).HasColumnName("OTP");

                entity.Property(e => e.OtpexpiryDate)
                    .HasColumnName("OTPExpiryDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TokenExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
