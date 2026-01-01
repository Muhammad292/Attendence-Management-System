using System;
using System.Collections.Generic;

namespace AttendenceManagementSystem.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[]? ProfileImage { get; set; }

    public string Role { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
