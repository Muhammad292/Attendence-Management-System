using System;
using System.Collections.Generic;

namespace AttendenceManagementSystem.Models;

public partial class Enrollment
{
    public int CourseId { get; set; }

    public int StudentId { get; set; }

    public DateOnly EnrollmentDate { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
