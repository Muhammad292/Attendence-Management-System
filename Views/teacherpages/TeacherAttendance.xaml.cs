using AttendenceManagementSystem.Models;
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
using Microsoft.EntityFrameworkCore;
using AttendenceManagementSystem.Views.studentpages;
using AttendenceManagementSystem.ViewModel;
using System.Windows.Media.Media3D;


namespace AttendenceManagementSystem.Views.teacherpages
{
    /// <summary>
    /// Interaction logic for TeacherAttendance.xaml
    /// </summary>
    public class AttendanceRepository
    {
        private readonly SampleContext _context;

        public AttendanceRepository(SampleContext context)
        {
            _context = context;
        }

        public List<TeacherAttendanceViewModel> GetAttendanceByTeacherId(int teacherId)
        {
            return [.. _context.Attendances
                           .Include(a => a.Course)
                           .Include(a => a.Student)
                           .Where(a => a.Course.TeacherId == teacherId)
                           .Select(a => new TeacherAttendanceViewModel
                           {
                               StudentId = a.StudentId,
                               StudentName = a.Student.Name,
                               Registraton=a.Student.RegNo,
                               CourseName = a.Course.Name,
                               CourseID = a.Course.CourseId,
                               Date = a.AttendanceDate,
                               Status = a.Status
                           })];
        }
        public List<Course> GetCoursesByTeacherId(int teacherId)
        {
            return [.. _context.Courses
                           .Where(course => course.TeacherId == teacherId)
                           .Include(course => course.Enrollments) // Optionally include related data
                           .Include(course => course.Attendances)];
        }


        // Get the list of students enrolled in a specific course by CourseId
        public List<StudentEnrollmentDataViewModel> GetEnrolledStudentsByCourseId(int courseId)
        {
            return [.. _context.Enrollments
                           .Where(e => e.CourseId == courseId)
                           .Include(e => e.Student) // Include the student data
                           .Select(e => new StudentEnrollmentDataViewModel
                           {
                               StudentId = e.StudentId,
                               Registraton=e.Student.RegNo,
                               StudentName = e.Student.Name
                               //EnrollmentDate = e.EnrollmentDate
                           })];
        }

        //public List<StudentEnrollmentData> GetEnrolledStudentsByCourseId(int courseId)
        //{
        //    return _context.Enrollments
        //                   .Where(e => e.CourseId == courseId)
        //                   .Include(e => e.Student) // Include the student data
        //                   .Select(e => new StudentEnrollmentData
        //                   {
        //                       StudentId = e.StudentId,
        //                       StudentName = e.Student.Name
        //                       //EnrollmentDate = e.EnrollmentDate
        //                   })
        //                   .ToList();
        //}


    }

    

   


    public partial class TeacherAttendance : Page
    {
        private int teacherID;
        private int CourseID=-1;

        public ObservableCollection<TeacherAttendanceViewModel> Attendancedata { get; set; } = new ObservableCollection<TeacherAttendanceViewModel>();
        public ObservableCollection<TeacherAttendanceViewModel> EnrolledStudentsAttendance { get; set; } = new ObservableCollection<TeacherAttendanceViewModel>();
        public ObservableCollection<TeacherAttendanceViewModel> UpdateAttendance { get; set; } = new ObservableCollection<TeacherAttendanceViewModel>();
        public ObservableCollection<string> Courses = new ObservableCollection<string>();
        //public ObservableCollection<string> Courselst = new ObservableCollection<string>();

        public TeacherAttendance(int id )
        {
            InitializeComponent();
            teacherID = id;
            LoadAttendanceData();
            this.DataContext = this;            
            Save_btn.Visibility=Visibility.Collapsed;
        }

        private void LoadAttendanceData()
        {
            try
            {
                using (var context = new SampleContext())
                {
                    var repository = new AttendanceRepository(context);
                    var attendanceList = repository.GetAttendanceByTeacherId(teacherID);
                    var Courselist = repository.GetCoursesByTeacherId(teacherID);

                    Attendancedata.Clear();
                    Courses.Clear();


                    foreach (var item in attendanceList)
                    {
                        if (!Courses.Contains(item.CourseName))
                        {
                            Courses.Add(item.CourseName); // Populate course filter
                        }


                        Attendancedata.Add(item);
                    }

                    //foreach (var item in Courselist)
                    //{
                    //    Courselst.Add(item);
                    //}
                    CourseComboBox2.ItemsSource = Courselist;
                }

                AttendanceDataGrid.ItemsSource = Attendancedata; // Bind data to DataGrid
                CourseComboBox.ItemsSource = Courses; // Bind courses to ComboBox

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating course: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            
        }

        private void TeacherAttendancePage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //CollectionViewSource.GetDefaultView(Attendancedata).Filter = null;
            //AttendanceDatePicker.SelectedDate = null;
            //CourseComboBox.SelectedItem = null;
            //CourseComboBox2.SelectedItem = null;
            //AttendanceDataGrid2.ItemsSource = null;
        }

        private void OnDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAttendanceData();
        }

        private void OnCourseChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAttendanceData();
        }

        private void FilterAttendanceData()
        {
            string? selectedCourse = CourseComboBox.SelectedItem as string;
            DateOnly? selectedDate = AttendanceDatePicker.SelectedDate.HasValue
                ? DateOnly.FromDateTime(AttendanceDatePicker.SelectedDate.Value)
                : null;

            CollectionViewSource.GetDefaultView(Attendancedata).Filter = item =>
            {
                var data = item as TeacherAttendanceViewModel;
                return (string.IsNullOrEmpty(selectedCourse) || data.CourseName == selectedCourse) &&
                       (!selectedDate.HasValue || data.Date == selectedDate.Value);
            };
        }


        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;

            if (button != null)
            {
                // Get the data item (AttendanceData) associated with this row
                TeacherAttendanceViewModel? Attendancedata = button.DataContext as TeacherAttendanceViewModel;

                if (Attendancedata != null)
                {
                    // Toggle the status between "Present" and "Absent"
                    Attendancedata.Status = Attendancedata.Status == "Absent" ? "Present" : "Absent";

                    // If you need to update the database or perform any other action, 
                    // you can call your repository method here.
                    // For example:
                    // _attendanceRepository.UpdateStatus(attendanceData);

                    // Optionally, notify the UI of changes (if you have INotifyPropertyChanged)
                    // This will update the UI binding automatically.
                }
            }
        }

        private void ShowEnrolledStudents(int courseId)
        {
            CourseID = courseId;
            try
            {
                using (var context = new SampleContext())
                {
                    var repository = new AttendanceRepository(context);
                    var enrolledStudents = repository.GetEnrolledStudentsByCourseId(courseId);
                    EnrolledStudentsAttendance.Clear();

                    foreach (var item in enrolledStudents)
                    {
                        // Create a new instance of TeacherAttendanceViewModel for each student
                        var temp = new TeacherAttendanceViewModel
                        {
                            StudentId = item.StudentId,
                            CourseID = courseId,
                            Registraton = item.Registraton,
                            StudentName = item.StudentName
                        };

                        EnrolledStudentsAttendance.Add(temp);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating course: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!EnrolledStudentsAttendance.Any(x => x.CourseID == CourseID))
            {
                Save_btn.Visibility = Visibility.Collapsed;
            }
            else
            {
                Save_btn.Visibility = Visibility.Visible;
            }

            AttendanceDataGrid2.ItemsSource = EnrolledStudentsAttendance; // Bind the data to DataGrid
        }


        //private void ShowEnrolledStudents(int courseId)
        //{
        //    CourseID = courseId ;
        //    try
        //    {
        //        using (var context = new SampleContext())
        //        {
        //            var repository = new AttendanceRepository(context);
        //            var enrolledStudents = repository.GetEnrolledStudentsByCourseId(courseId);
        //            EnrolledStudentsAttendance.Clear();

        //            TeacherAttendanceViewModel temp = new TeacherAttendanceViewModel();
        //            foreach (var item in enrolledStudents)
        //            {
        //                temp.StudentId = item.StudentId;
        //                temp.CourseID = courseId;
        //                temp.Registraton= item.Registraton;
        //                temp.StudentName = item.StudentName;

        //                EnrolledStudentsAttendance.Add(temp);

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error updating course: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }


        //    if (EnrolledStudentsAttendance.FirstOrDefault(x =>x.CourseID == CourseID)==null)
        //    {
        //        //Save_btn.IsEnabled = false;
        //        Save_btn.Visibility = Visibility.Collapsed;

        //    }
        //    else
        //    {
        //        //Save_btn.IsEnabled = true;
        //        Save_btn.Visibility = Visibility.Visible;

        //    }

        //    AttendanceDataGrid2.ItemsSource = EnrolledStudentsAttendance; // Bind the data to DataGrid
        //}


        private void CourseComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CourseComboBox2.SelectedItem is Course selectedCourse)
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                if (Attendancedata.FirstOrDefault(x => x.CourseID == selectedCourse.CourseId && x.Date == today )!=null)
                {
                    MessageBox.Show("The attendance of today have been made successfully");
                    return;
                }
                    ShowEnrolledStudents(selectedCourse.CourseId); // Show students for the selected course
               
            }
           
        }

        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
           
            //MessageBox.Show(EnrolledStudentsAttendance.FirstOrDefault(x=>x.Status=="Present"&& x.CourseID==CourseID)?.Status ??"-1");

            try
            {
                using (var context = new SampleContext())
                {
                    // Loop through all items in the ObservableCollection
                    foreach (var attendance in EnrolledStudentsAttendance)
                    {
                        // Create a new Attendance entity with only the required fields
                        var newAttendance = new Models.Attendance
                        {
                            StudentId = attendance.StudentId,
                            CourseId = attendance.CourseID,
                            
                            AttendanceDate = attendance.Date,
                            Status = attendance.Status
                            // The following fields will be handled by the database trigger:
                            // StudentName, TeacherName, CourseName
                        };

                        // Add the new entity to the database
                        context.Attendances.Add(newAttendance);
                        
                    }

                    // Save changes to the database
                    context.SaveChanges();
                    Save_btn.Visibility = Visibility.Collapsed;
                    LoadAttendanceData();

                    MessageBox.Show("New attendance records added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding new attendance records: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

       
        private void Edit_btn_Click(object sender, RoutedEventArgs e)
        {
            string? selectedCourse = CourseComboBox.SelectedItem as string;
            DateOnly? selectedDate = AttendanceDatePicker.SelectedDate.HasValue
                ? DateOnly.FromDateTime(AttendanceDatePicker.SelectedDate.Value)
                : null;

            if (selectedCourse != null && selectedDate != null)
            {
                if (Attendancedata.FirstOrDefault(x => x.Date == selectedDate) != null)
                {
                    // Trigger navigation
                    var nextPage = new TeacherAttendanceEdit(teacherID, selectedCourse, selectedDate);
                    NavigationService.GetNavigationService(this)?.Navigate(nextPage);
                    return;

                }

                MessageBox.Show("Selected dateattendance doesnot exist");
                return;
            }
            else if (AttendanceDataGrid.SelectedItem is TeacherAttendanceViewModel temp)
            {
                TeacherAttendanceViewModel selectedRow = temp;
                var nextPage = new TeacherAttendanceEdit(teacherID, selectedRow.CourseName, selectedRow.Date);
                NavigationService.GetNavigationService(this)?.Navigate(nextPage);
                return;
            }

                MessageBox.Show("Select The Row or Give Course & Date");


        }


    }
}
