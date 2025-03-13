using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using Meds.Server.Models.DBModels;
namespace Meds.Server.Models;

public partial class DatabaseforkpzContext : DbContext
{
    public DatabaseforkpzContext()
    {
    }

    public DatabaseforkpzContext(DbContextOptions<DatabaseforkpzContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Laboratory> Laboratories { get; set; }

    public virtual DbSet<Lvivstat> Lvivstats { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Technician> Technicians { get; set; }

    public virtual DbSet<TestBatch> TestBatches { get; set; }

    public virtual DbSet<TestCollection> TestCollections { get; set; }

    public virtual DbSet<TestNormalValue> TestNormalValues { get; set; }

    public virtual DbSet<TestOrder> TestOrders { get; set; }

    public virtual DbSet<TestResult> TestResults { get; set; }

    public virtual DbSet<TestType> TestTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=wv1;user=root;password=password", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.39-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Laboratory>(entity =>
        {
            entity.HasKey(e => e.LaboratoryId).HasName("PRIMARY");

            entity.ToTable("laboratories");

            entity.HasIndex(e => e.Address, "address_UNIQUE").IsUnique();

            entity.HasIndex(e => e.ContactNumber, "contactNumber_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.Property(e => e.LaboratoryId).HasColumnName("laboratoryID");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .HasColumnName("contactNumber");
            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .HasColumnName("email");
        });

        modelBuilder.Entity<Lvivstat>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("lvivstats");

            entity.Property(e => e.Delta).HasPrecision(30, 2);
            entity.Property(e => e.Income).HasPrecision(29, 2);
            entity.Property(e => e.Lab).HasMaxLength(100);
            entity.Property(e => e.Period).HasMaxLength(7);
            entity.Property(e => e.TestTaken).HasColumnName("Test Taken");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PRIMARY");

            entity.ToTable("patients");

            entity.HasIndex(e => e.UserId, "patUserID_idx").IsUnique();

            entity.HasIndex(e => new { e.FullName, e.Gender, e.DateOfBirth, e.Email, e.ContactNumber }, "unique_patient").IsUnique();

            entity.Property(e => e.PatientId).HasColumnName("patientID");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .HasColumnName("contactNumber");
            entity.Property(e => e.DateOfBirth).HasColumnName("dateOfBirth");
            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("fullName");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.UserId)
                .HasConstraintName("patUserID");
        });

        modelBuilder.Entity<Technician>(entity =>
        {
            entity.HasKey(e => e.TechnicianId).HasName("PRIMARY");

            entity.ToTable("technicians");

            entity.HasIndex(e => e.ContactNumber, "contactNumber_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.LaboratoryId, "laboratoryID_idx");

            entity.HasIndex(e => e.UserId, "userID_UNIQUE").IsUnique();

            entity.Property(e => e.TechnicianId).HasColumnName("technicianID");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .HasColumnName("contactNumber");
            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("fullName");
            entity.Property(e => e.LaboratoryId).HasColumnName("laboratoryID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Laboratory).WithMany(p => p.Technicians)
                .HasForeignKey(d => d.LaboratoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("laboratoryID");

            entity.HasOne(d => d.User).WithOne(p => p.Technician)
                .HasForeignKey<Technician>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("techUserID");
        });

        modelBuilder.Entity<TestBatch>(entity =>
        {
            entity.HasKey(e => e.TestBatchId).HasName("PRIMARY");

            entity.ToTable("test_batches");

            entity.HasIndex(e => e.PatientId, "batchesPatientID");

            entity.HasIndex(e => e.TechnicianId, "batchesTechnicianID_idx");

            entity.HasIndex(e => e.DateOfCreation, "idx_test_batches_dateOfCreation");

            entity.Property(e => e.TestBatchId).HasColumnName("testBatchID");
            entity.Property(e => e.BatchStatus)
                .HasMaxLength(1)
                .HasDefaultValueSql("'q'")
                .IsFixedLength()
                .HasColumnName("batchStatus");
            entity.Property(e => e.DateOfCreation)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("dateOfCreation");
            entity.Property(e => e.PatientId).HasColumnName("patientID");
            entity.Property(e => e.TechnicianId).HasColumnName("technicianID");

            entity.HasOne(d => d.Patient).WithMany(p => p.TestBatches)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("batchesPatientID");

            entity.HasOne(d => d.Technician).WithMany(p => p.TestBatches)
                .HasForeignKey(d => d.TechnicianId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("batchesTechnicianID");
        });

        modelBuilder.Entity<TestCollection>(entity =>
        {
            entity.HasKey(e => e.TestCollectionId).HasName("PRIMARY");

            entity.ToTable("test_collections");

            entity.HasIndex(e => e.CollectionName, "collectionName_UNIQUE").IsUnique();

            entity.Property(e => e.TestCollectionId).HasColumnName("testCollectionID");
            entity.Property(e => e.CollectionName)
                .HasMaxLength(100)
                .HasColumnName("collectionName");

            entity.HasMany(d => d.TestTypes).WithMany(p => p.TestCollections)
                .UsingEntity<Dictionary<string, object>>(
                    "IncludedTestsForCollection",
                    r => r.HasOne<TestType>().WithMany()
                        .HasForeignKey("TestTypeId")
                        .HasConstraintName("includedTestsForCollectionTestTypeID"),
                    l => l.HasOne<TestCollection>().WithMany()
                        .HasForeignKey("TestCollectionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("includedTestsForCollectionTestCollection"),
                    j =>
                    {
                        j.HasKey("TestCollectionId", "TestTypeId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("included_tests_for_collection");
                        j.HasIndex(new[] { "TestTypeId" }, "includedTestsForCollectionTestTypeID_idx");
                        j.IndexerProperty<int>("TestCollectionId").HasColumnName("testCollectionID");
                        j.IndexerProperty<int>("TestTypeId").HasColumnName("testTypeID");
                    });
        });

        modelBuilder.Entity<TestNormalValue>(entity =>
        {
            entity.HasKey(e => new { e.TestNormalValuesId, e.TestTypeId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("test_normal_values");

            entity.HasIndex(e => e.TestTypeId, "normalValuesTestTypeID_idx");

            entity.HasIndex(e => e.TestNormalValuesId, "testNormalValuesID_UNIQUE").IsUnique();

            entity.Property(e => e.TestNormalValuesId)
                .ValueGeneratedOnAdd()
                .HasColumnName("testNormalValuesID");
            entity.Property(e => e.TestTypeId).HasColumnName("testTypeID");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.MaxAge).HasColumnName("maxAge");
            entity.Property(e => e.MaxResValue)
                .HasPrecision(5, 2)
                .HasColumnName("maxResValue");
            entity.Property(e => e.MinAge).HasColumnName("minAge");
            entity.Property(e => e.MinResValue)
                .HasPrecision(5, 2)
                .HasColumnName("minResValue");

            entity.HasOne(d => d.TestType).WithMany(p => p.TestNormalValues)
                .HasForeignKey(d => d.TestTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("normalValuesTestTypeID");
        });

        modelBuilder.Entity<TestOrder>(entity =>
        {
            entity.HasKey(e => e.TestOrderId).HasName("PRIMARY");

            entity.ToTable("test_orders");

            entity.HasIndex(e => e.TestBatchId, "ordersTestBatchID");

            entity.HasIndex(e => e.TestTypeId, "ordersTestTypeID_idx");

            entity.Property(e => e.TestOrderId).HasColumnName("testOrderID");
            entity.Property(e => e.TestBatchId).HasColumnName("testBatchID");
            entity.Property(e => e.TestTypeId).HasColumnName("testTypeID");

            entity.HasOne(d => d.TestBatch).WithMany(p => p.TestOrders)
                .HasForeignKey(d => d.TestBatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ordersTestBatchID");

            entity.HasOne(d => d.TestType).WithMany(p => p.TestOrders)
                .HasForeignKey(d => d.TestTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ordersTestTypeID");
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.HasKey(e => e.TestOrderId).HasName("PRIMARY");

            entity.ToTable("test_results");

            entity.HasIndex(e => e.TestOrderId, "testOrderID_idx");

            entity.Property(e => e.TestOrderId)
                .ValueGeneratedOnAdd()
                .HasColumnName("testOrderID");
            entity.Property(e => e.DateOfTest).HasColumnName("dateOfTest");
            entity.Property(e => e.Result)
                .HasPrecision(5, 2)
                .HasColumnName("result");

            entity.HasOne(d => d.TestOrder).WithOne(p => p.TestResult)
                .HasForeignKey<TestResult>(d => d.TestOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("resultsOrderID");
        });

        modelBuilder.Entity<TestType>(entity =>
        {
            entity.HasKey(e => e.TestTypeId).HasName("PRIMARY");

            entity.ToTable("test_types");

            entity.HasIndex(e => e.TestName, "name_UNIQUE").IsUnique();

            entity.Property(e => e.TestTypeId).HasColumnName("testTypeID");
            entity.Property(e => e.Cost)
                .HasPrecision(7, 2)
                .HasColumnName("cost");
            entity.Property(e => e.DaysTillOverdue).HasColumnName("daysTillOverdue");
            entity.Property(e => e.MeasurementsUnit)
                .HasMaxLength(15)
                .HasColumnName("measurementsUnit");
            entity.Property(e => e.TestName)
                .HasMaxLength(100)
                .HasColumnName("testName");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.Login)
                .HasMaxLength(320)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
