using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Курсач_сайко_1125;

public partial class Dayn1Context : DbContext
{
    public Dayn1Context()
    {
    }

    public Dayn1Context(DbContextOptions<Dayn1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<LoginSt> LoginSts { get; set; }

    public virtual DbSet<Nozap> Nozaps { get; set; }

    public virtual DbSet<Yeszap> Yeszaps { get; set; }

    public virtual DbSet<Zap> Zaps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=student;database=dayn1", Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.3.2-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("login");

            entity.HasIndex(e => e.PassportNumber, "FK_login_role_Id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.PassportNumber).HasColumnName("passportNumber");
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        modelBuilder.Entity<LoginSt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("login_st");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(255)
                .HasColumnName("passportNumber");
            entity.Property(e => e.StudentEmail).HasMaxLength(255);
            entity.Property(e => e.StudentGpa).HasMaxLength(255);
            entity.Property(e => e.StudentName).HasMaxLength(255);
            entity.Property(e => e.StudentPassword).HasMaxLength(255);
            entity.Property(e => e.StudentSpec).HasMaxLength(255);
        });

        modelBuilder.Entity<Nozap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("nozap");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Gpa).HasPrecision(3, 2);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(255)
                .HasColumnName("passportNumber");
            entity.Property(e => e.Spec).HasMaxLength(255);
        });

        modelBuilder.Entity<Yeszap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("yeszap");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Gpa).HasPrecision(3, 2);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(255)
                .HasColumnName("passportNumber");
            entity.Property(e => e.Spec).HasMaxLength(255);
        });

        modelBuilder.Entity<Zap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("zap");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Gpa).HasPrecision(3, 2);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(255)
                .HasColumnName("passportNumber");
            entity.Property(e => e.Spec).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
