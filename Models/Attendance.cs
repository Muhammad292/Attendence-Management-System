using System;
using System.Collections.Generic;

namespace AttendenceManagementSystem.Models;

public partial class Attendance
{
    public int AttendanceId { get; set; }

    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public DateOnly AttendanceDate { get; set; }

    public string Status { get; set; } = null!;

    public string? StudentName { get; set; }

    public string? TeacherName { get; set; }

    public string? CourseName { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
