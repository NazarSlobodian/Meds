using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Meds.Server.Models.DbModels;

public partial class Wv1Context : DbContext
{
    public Wv1Context()
    {
    }

    public Wv1Context(DbContextOptions<Wv1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<CollectionPoint> CollectionPoints { get; set; }

    public virtual DbSet<LabWorker> LabWorkers { get; set; }

    public virtual DbSet<Laboratory> Laboratories { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Receptionist> Receptionists { get; set; }

    public virtual DbSet<TestBatch> TestBatches { get; set; }

    public virtual DbSet<TestNormalValue> TestNormalValues { get; set; }

    public virtual DbSet<TestOrder> TestOrders { get; set; }

    public virtual DbSet<TestPanel> TestPanels { get; set; }

    public virtual DbSet<TestResult> TestResults { get; set; }

    public virtual DbSet<TestType> TestTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=wv1;uid=root;pwd=password", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<CollectionPoint>(entity =>
        {
            entity.HasKey(e => e.CollectionPointId).HasName("PRIMARY");

            entity.ToTable("collection_points");

            entity.HasIndex(e => e.Address, "address_UNIQUE").IsUnique();

            entity.HasIndex(e => e.ContactNumber, "contact_number_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.Property(e => e.CollectionPointId).HasColumnName("collectionPointID");
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

        modelBuilder.Entity<LabWorker>(entity =>
        {
            entity.HasKey(e => e.LabWorkerId).HasName("PRIMARY");

            entity.ToTable("lab_workers");

            entity.HasIndex(e => e.ContactNumber, "contactNumber_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.LaboratoryId, "laboratoryID_idx");

            entity.Property(e => e.LabWorkerId).HasColumnName("labWorkerID");
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

            entity.HasOne(d => d.Laboratory).WithMany(p => p.LabWorkers)
                .HasForeignKey(d => d.LaboratoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("laboratoryID");
        });

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

            entity.HasMany(d => d.TestTypes).WithMany(p => p.Laboratories)
                .UsingEntity<Dictionary<string, object>>(
                    "TestPerformer",
                    r => r.HasOne<TestType>().WithMany()
                        .HasForeignKey("TestTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("perfTestId"),
                    l => l.HasOne<Laboratory>().WithMany()
                        .HasForeignKey("LaboratoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("perfLabId"),
                    j =>
                    {
                        j.HasKey("LaboratoryId", "TestTypeId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("test_performers");
                        j.HasIndex(new[] { "TestTypeId" }, "perfTestId_idx");
                        j.IndexerProperty<int>("LaboratoryId").HasColumnName("laboratoryID");
                        j.IndexerProperty<int>("TestTypeId").HasColumnName("testTypeID");
                    });
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PRIMARY");

            entity.ToTable("patients");

            entity.HasIndex(e => e.ContactNumber, "contactNumber_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

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
        });

        modelBuilder.Entity<Receptionist>(entity =>
        {
            entity.HasKey(e => e.ReceptionistId).HasName("PRIMARY");

            entity.ToTable("receptionists");

            entity.HasIndex(e => e.CollectionPointId, "collectionPointID_idx");

            entity.HasIndex(e => e.ContactNumber, "contactNumber_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.Property(e => e.ReceptionistId).HasColumnName("receptionistID");
            entity.Property(e => e.CollectionPointId).HasColumnName("collectionPointID");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .HasColumnName("contactNumber");
            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("fullName");

            entity.HasOne(d => d.CollectionPoint).WithMany(p => p.Receptionists)
                .HasForeignKey(d => d.CollectionPointId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("collectionPointID");
        });

        modelBuilder.Entity<TestBatch>(entity =>
        {
            entity.HasKey(e => e.TestBatchId).HasName("PRIMARY");

            entity.ToTable("test_batches");

            entity.HasIndex(e => e.PatientId, "batchesPatientID");

            entity.HasIndex(e => e.ReceptionistId, "batchesReceptionistID_idx");

            entity.HasIndex(e => e.DateOfCreation, "idx_test_batches_dateOfCreation");

            entity.Property(e => e.TestBatchId).HasColumnName("testBatchID");
            entity.Property(e => e.BatchStatus)
                .HasDefaultValueSql("'queued'")
                .HasColumnType("enum('queued','processing','done')")
                .HasColumnName("batchStatus");
            entity.Property(e => e.Cost)
                .HasPrecision(7, 2)
                .HasColumnName("cost");
            entity.Property(e => e.DateOfCreation)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("dateOfCreation");
            entity.Property(e => e.PatientId).HasColumnName("patientID");
            entity.Property(e => e.ReceptionistId).HasColumnName("receptionistID");

            entity.HasOne(d => d.Patient).WithMany(p => p.TestBatches)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("batchesPatientID");

            entity.HasOne(d => d.Receptionist).WithMany(p => p.TestBatches)
                .HasForeignKey(d => d.ReceptionistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("receptionistID");
        });

        modelBuilder.Entity<TestNormalValue>(entity =>
        {
            entity.HasKey(e => e.TestNormalValueId).HasName("PRIMARY");

            entity.ToTable("test_normal_values");

            entity.HasIndex(e => e.TestTypeId, "normalValuesTestTypeID_idx");

            entity.Property(e => e.TestNormalValueId).HasColumnName("testNormalValueID");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.MaxAge)
                .HasDefaultValueSql("'999'")
                .HasColumnName("maxAge");
            entity.Property(e => e.MaxResValue)
                .HasPrecision(5, 2)
                .HasColumnName("maxResValue");
            entity.Property(e => e.MinAge).HasColumnName("minAge");
            entity.Property(e => e.MinResValue)
                .HasPrecision(5, 2)
                .HasColumnName("minResValue");
            entity.Property(e => e.TestTypeId).HasColumnName("testTypeID");

            entity.HasOne(d => d.TestType).WithMany(p => p.TestNormalValues)
                .HasForeignKey(d => d.TestTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("normalValuesTestTypeID");
        });

        modelBuilder.Entity<TestOrder>(entity =>
        {
            entity.HasKey(e => e.TestOrderId).HasName("PRIMARY");

            entity.ToTable("test_orders");

            entity.HasIndex(e => e.LaboratoryId, "ordersLaboratoryID_idx");

            entity.HasIndex(e => e.TestBatchId, "ordersTestBatchID");

            entity.HasIndex(e => e.TestPanelId, "ordersTestPanelID_idx");

            entity.HasIndex(e => e.TestTypeId, "ordersTestTypeID_idx");

            entity.Property(e => e.TestOrderId).HasColumnName("testOrderID");
            entity.Property(e => e.LaboratoryId).HasColumnName("laboratoryID");
            entity.Property(e => e.TestBatchId).HasColumnName("testBatchID");
            entity.Property(e => e.TestPanelId).HasColumnName("testPanelID");
            entity.Property(e => e.TestTypeId).HasColumnName("testTypeID");

            entity.HasOne(d => d.Laboratory).WithMany(p => p.TestOrders)
                .HasForeignKey(d => d.LaboratoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ordersLaboratoryID");

            entity.HasOne(d => d.TestBatch).WithMany(p => p.TestOrders)
                .HasForeignKey(d => d.TestBatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ordersTestBatchID");

            entity.HasOne(d => d.TestPanel).WithMany(p => p.TestOrders)
                .HasForeignKey(d => d.TestPanelId)
                .HasConstraintName("ordersTestPanelID");

            entity.HasOne(d => d.TestType).WithMany(p => p.TestOrders)
                .HasForeignKey(d => d.TestTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ordersTestTypeID");
        });

        modelBuilder.Entity<TestPanel>(entity =>
        {
            entity.HasKey(e => e.TestPanelId).HasName("PRIMARY");

            entity.ToTable("test_panels");

            entity.HasIndex(e => e.Name, "name_UNIQUE").IsUnique();

            entity.Property(e => e.TestPanelId).HasColumnName("testPanelID");
            entity.Property(e => e.Cost)
                .HasPrecision(7, 2)
                .HasColumnName("cost");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasMany(d => d.TestTypes).WithMany(p => p.TestPanels)
                .UsingEntity<Dictionary<string, object>>(
                    "TestPanelsContent",
                    r => r.HasOne<TestType>().WithMany()
                        .HasForeignKey("TestTypeId")
                        .HasConstraintName("testTypeID"),
                    l => l.HasOne<TestPanel>().WithMany()
                        .HasForeignKey("TestPanelId")
                        .HasConstraintName("testPanelID"),
                    j =>
                    {
                        j.HasKey("TestPanelId", "TestTypeId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("test_panels_contents");
                        j.HasIndex(new[] { "TestTypeId" }, "testTypeID_idx");
                        j.IndexerProperty<int>("TestPanelId").HasColumnName("testPanelID");
                        j.IndexerProperty<int>("TestTypeId").HasColumnName("testTypeID");
                    });
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.HasKey(e => e.TestOrderId).HasName("PRIMARY");

            entity.ToTable("test_results");

            entity.HasIndex(e => e.TestOrderId, "testOrderID_idx");

            entity.Property(e => e.TestOrderId)
                .ValueGeneratedOnAdd()
                .HasColumnName("testOrderID");
            entity.Property(e => e.DateOfTest)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("dateOfTest");
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

            entity.HasIndex(e => e.Name, "name_UNIQUE").IsUnique();

            entity.Property(e => e.TestTypeId).HasColumnName("testTypeID");
            entity.Property(e => e.Cost)
                .HasPrecision(7, 2)
                .HasColumnName("cost");
            entity.Property(e => e.MeasurementsUnit)
                .HasMaxLength(15)
                .HasColumnName("measurementsUnit");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.Hash)
                .HasMaxLength(128)
                .HasColumnName("hash");
            entity.Property(e => e.Login)
                .HasMaxLength(320)
                .HasColumnName("login");
            entity.Property(e => e.ReferencedId).HasColumnName("referencedID");
            entity.Property(e => e.Role)
                .HasColumnType("enum('patient','receptionist','lab_worker','lab_admin','admin')")
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
