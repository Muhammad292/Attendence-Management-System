using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendenceManagementSystem.ViewModel
{
    public class StudentEnrollmentDataViewModel:ObservableObject
    {
            public int StudentId { get; set; }

            private string registraton = "";
            public string Registraton
             {
                get => registraton;
                set => SetProperty(ref registraton, value); // Simplifies property setting; }
                                                     //public DateOnly EnrollmentDate { get; set; }
            }
        private string name = "";
            public string StudentName
            {
                get => name;
                set => SetProperty(ref name, value); // Simplifies property setting; }
                                                     //public DateOnly EnrollmentDate { get; set; }
            }

    }
}
