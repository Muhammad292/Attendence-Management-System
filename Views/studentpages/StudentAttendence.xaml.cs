using AttendenceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AttendenceManagementSystem.Views.studentpages
{
    /// <summary>
    /// Interaction logic for StudentAttendence.xaml
    /// </summary>
    public class AttendanceRepository
    {
        private readonly SampleContext _context;

        public AttendanceRepository(SampleContext context)
        {
            _context = context;
        }

        public List<Attendancedata> GetAttendanceWithCourseNameByStudentId(int studentId)
        {
            return _context.Attendances
                           .Include(a => a.Course)
                           .Where(a => a.StudentId == studentId)
                           .Select(a => new Attendancedata
                           {
                               Course = a.Course.Name,
                               Date = a.AttendanceDate,
                               Status = a.Status
                           })
                           .ToList();
        }
    }


    public class Attendancedata
    {
        public DateOnly Date { get; set; }
        public string? Course { get; set; }
        public string? Status { get; set; }

    }

    public partial class StudentAttendence : Page
    {
        private int studentid;
        public ObservableCollection<Attendancedata> attendanceData = new ObservableCollection<Attendancedata>();
        public ObservableCollection<string> course = new ObservableCollection<string>();


        public StudentAttendence(int id)
        {
            InitializeComponent();
            studentid = id;
            getattendace();
            this.DataContext = this;

        }
        private void StudentAttendencepage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CollectionViewSource.GetDefaultView(attendanceData).Filter = null;
            AttendanceDatePicker.SelectedDate = null;
            CourseComboBox.SelectedItem = null;
            //AttendanceDataGrid.ItemsSource = attendanceData;

        }
        private void getattendace()
        {
            using (var context = new SampleContext())
            {
                var repository = new AttendanceRepository(context);
                List<Attendancedata> attendanceList = repository.GetAttendanceWithCourseNameByStudentId(studentid);

                // Populate the ObservableCollection
                attendanceData.Clear(); // Clear existing data
                course.Clear();
                foreach (var item in attendanceList)
                {
                    if (!course.Contains(item.Course))
                    {
                        // Add the course if it does not exist
                        course.Add(item.Course);


                    }

                    attendanceData.Add(item); // Add each attendance record

                }

            }

            AttendanceDataGrid.ItemsSource = attendanceData;
            CourseComboBox.ItemsSource = course;

        }
        
       
        private void OnCourseChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAttendanceData();
        }
        private void OnDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAttendanceData();
        }

        private void FilterAttendanceData()
        {
            // Get selected course (if any)
            string? selectedCourse = CourseComboBox.SelectedItem as string;

            // Get selected date (if any)
            DateOnly? selectedDate = null;
            if (AttendanceDatePicker.SelectedDate.HasValue)
            {
                selectedDate = DateOnly.FromDateTime(AttendanceDatePicker.SelectedDate.Value);
            }

            CollectionViewSource.GetDefaultView(attendanceData).Filter = item =>
            {
                var data = item as Attendancedata;
                return (string.IsNullOrEmpty(selectedCourse) || data.Course == selectedCourse) &&
                       (!selectedDate.HasValue || data.Date == selectedDate.Value);
            };


            //// Filter the data based on the selected course and/or date
            //ObservableCollection<Attendancedata> filteredAttendanceData = new ObservableCollection<Attendancedata>(
            //    attendanceData.Where(a =>
            //        (string.IsNullOrEmpty(selectedCourse) || a.Course == selectedCourse) &&  // Filter by course if selected
            //        (!selectedDate.HasValue || a.Date == selectedDate.Value)  // Filter by date if selected
            //    )
            //);

            //// Update DataGrid with the filtered data
            //AttendanceDataGrid.ItemsSource = filteredAttendanceData;
        }

    }





}
