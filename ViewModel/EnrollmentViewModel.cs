using AttendenceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace AttendenceManagementSystem.ViewModel
{
    public class EnrollmentViewModel
    {
        private readonly SampleContext _context;

        public ObservableCollection<EnrollmentInfo> EnrollmentDetails { get; set; }


        public EnrollmentViewModel(int id)
        { 
            _context = new SampleContext();
            EnrollmentDetails = new ObservableCollection<EnrollmentInfo>();
            LoadEnrollmentData( id);
        }

        private void LoadEnrollmentData(int id)
        {
            // Join Enrollment, Course, and Student to get the required fields
            var enrollmentData = _context.Enrollments
                .Include(e => e.Course)      // Include Course details
                .Include(e => e.Student)     // Include Student details
                .Where(e => e.Course.TeacherId == id) // Filter by Teacher ID
                .Select(e => new EnrollmentInfo
                {
                    CourseId = e.CourseId,
                    CourseName = e.Course.Name,
                    StudentId = e.StudentId,
                    StudentName = e.Student.Name,
                    EnrollmentDate = e.EnrollmentDate
                })
                .ToList();
            //// Join Enrollment, Course, and Student to get the required fields
            //var enrollmentData = _context.Enrollments
            //    .Include(e => e.Course)      // Include Course details
            //    .Include(e => e.Student)     // Include Student details
            //    .Select(e => new EnrollmentInfo
            //    {
            //        CourseId = e.CourseId,
            //        CourseName = e.Course.Name,
            //        StudentId = e.StudentId,
            //        StudentName = e.Student.Name,
            //        EnrollmentDate = e.EnrollmentDate
            //    })
            //    .ToList();

            // Convert List to ObservableCollection
            EnrollmentDetails = new ObservableCollection<EnrollmentInfo>(enrollmentData);
        }
    }

    // DTO (Data Transfer Object) to hold the required data
    public class EnrollmentInfo
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = "";
        public int StudentId { get; set; }
        public string StudentName { get; set; } = "";
        public DateOnly EnrollmentDate { get; set; }
    }
}
