using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AttendenceManagementSystem.Models;

public partial class SampleContext : DbContext
{
    public SampleContext()
    {
    }

    public SampleContext(DbContextOptions<SampleContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Challan> Challans { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-66E9OJG\\SQLEXPRESS;Initial Catalog=sample;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Attendan__8B69263C9A307C37");

            entity.ToTable("Attendance", tb => tb.HasTrigger("trg_InsertUpdateAttendance"));

            entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CourseName).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.StudentName).HasMaxLength(100);
            entity.Property(e => e.TeacherName).HasMaxLength(100);

            entity.HasOne(d => d.Course).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Attendanc__Cours__5EBF139D");

            entity.HasOne(d => d.Student).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Attendanc__Stude__5DCAEF64");
        });

        modelBuilder.Entity<Challan>(entity =>
        {
            entity.HasKey(e => e.ChallanId).HasName("PK__Challan__BFB043C4AB5B610A");

            entity.ToTable("Challan");

            entity.Property(e => e.ChallanId).HasColumnName("ChallanID");
            entity.Property(e => e.ChallanStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Unpaid");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CourseName).HasMaxLength(100);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.StudentName).HasMaxLength(100);
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.TeacherName).HasMaxLength(100);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D7187A1D16540");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.TotalStudentLimit).HasDefaultValue(1);

            entity.HasOne(d => d.Teacher).WithMany(p => p.Courses)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__Courses__Teacher__5629CD9C");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => new { e.CourseId, e.StudentId }).HasName("PK__Enrollme__4A012320394B2C93");

            entity.ToTable(tb => tb.HasTrigger("trg_PopulateChallan"));

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.EnrollmentDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Enrollmen__Cours__59FA5E80");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Enrollmen__Stude__5AEE82B9");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52A7969E27660");

            entity.HasIndex(e => e.Email, "UQ__Students__A9D1053460D171E0").IsUnique();

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.ProfileImage).HasMaxLength(1);
            entity.Property(e => e.RegNo).HasMaxLength(50);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teachers__EDF25944DEDAF220");

            entity.HasIndex(e => e.Email, "UQ__Teachers__A9D1053493DE1C5D").IsUnique();

            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
