using System;
using System.Collections.Generic;

namespace AttendenceManagementSystem.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string RegNo { get; set; } = null!;

    public byte[]? ProfileImage { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
