using System;
using System.Collections.Generic;

namespace AttendenceManagementSystem.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string Name { get; set; } = null!;

    public int TeacherId { get; set; }

    public int Price { get; set; }

    public int TotalStudentLimit { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Teacher Teacher { get; set; } = null!;
}
