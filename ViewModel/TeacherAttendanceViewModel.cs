using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendenceManagementSystem.ViewModel
{
    public class TeacherAttendanceViewModel:ObservableObject
    { 
            public int StudentId { get; set; }
            public int CourseID { get; set; }
        public string Registraton { get; set; } = "";
        public string? StudentName { get; set; }
            public string? CourseName { get; set; }
            public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
            private string status = "Absent";
            public string Status
            {
                get => status;
                set => SetProperty(ref status, value);
            }

        
    }

}
