using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TaskApp.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Rsuserinfo> Rsuserinfos { get; set; }

    public virtual DbSet<TaskModel> Tasks { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rsuserinfo>(entity =>
        {
            entity.HasKey(e => new { e.Userid, e.Username });

            entity.ToTable("RSUSERINFO");

            entity.HasIndex(e => e.Username, "UQ__RSUSERIN__B15BE12EAF5C653C").IsUnique();

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnName("USERID");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
            entity.Property(e => e.Creadtets)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREADTETS");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Fullname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FULLNAME");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("ISACTIVE");
            entity.Property(e => e.Modefied)
                .HasColumnType("datetime")
                .HasColumnName("MODEFIED");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PASSWORDHASH");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
        });

        modelBuilder.Entity<TaskModel>(entity =>
        {
            entity.HasKey(e => e.Taskid).HasName("PK__TASKS__27AB85761FFA40DF");

            entity.ToTable("TASKS");

            entity.Property(e => e.Taskid).HasColumnName("TASKID");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("CREATEDAT");
            entity.Property(e => e.Createdby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Isdone).HasColumnName("ISDONE");
            entity.Property(e => e.Modifiedat).HasColumnName("MODIFIEDAT");
            entity.Property(e => e.Notificationon).HasColumnName("NOTIFICATIONON");
            entity.Property(e => e.Starttime)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("STARTTIME");
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SUBJECT");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("TITLE");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.Tasks)
                .HasPrincipalKey(p => p.Username)
                .HasForeignKey(d => d.Createdby)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TASKS_RSUSERINFO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
