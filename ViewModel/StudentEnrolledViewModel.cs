using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendenceManagementSystem.ViewModel
{
    public class StudentEnrolledViewModel
    {
            public int CourseId { get; set; }
            public string? Name { get; set; }
            public int? Price { get; set; } = 0;
            public string? TeacherName { get; set; }
            public int EnrolledStudents { get; set; }
            public int TotalStudentLimit { get; set; }
      
    }
}
