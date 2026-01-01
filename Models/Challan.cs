using System;
using System.Collections.Generic;

namespace AttendenceManagementSystem.Models;

public partial class Challan
{
    public int ChallanId { get; set; }

    public int StudentId { get; set; }

    public string StudentName { get; set; } = null!;

    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public int TeacherId { get; set; }

    public string TeacherName { get; set; } = null!;

    public int Price { get; set; }

    public string ChallanStatus { get; set; } = null!;

    public DateOnly EnrollmentDate { get; set; }
}
