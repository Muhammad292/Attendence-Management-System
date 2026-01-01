using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendenceManagementSystem.ViewModel
{
     public class CourseViewModel : ObservableObject
        {
            public int CourseId
            { get; set; }
            private string name = "";
            public string Name
            {
                get => name;
                set => SetProperty(ref name, value); // Simplifies property setting
            }

        private int price=0;
        public int Price
        {
            get => price;
            set => SetProperty(ref price, value);
        }

        private int totalStudentLimit;
            public int TotalStudentLimit
            {
                get => totalStudentLimit;
                set => SetProperty(ref totalStudentLimit, value);
            }
        }
    
}
