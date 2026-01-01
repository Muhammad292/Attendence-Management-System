using AttendenceManagementSystem.Models;
using AttendenceManagementSystem.ViewModel;
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
    /// Interaction logic for StudentEnrollment.xaml
    /// </summary>
    public partial class StudentEnrollment : Page
    {
        public ObservableCollection<StudentEnrolledViewModel> CoursesNotEnrolled { get; set; }=new ObservableCollection<StudentEnrolledViewModel>();

        private int studentID;
        public StudentEnrollment()
        {
            InitializeComponent();
        }

        public StudentEnrollment(int id)
        {
            InitializeComponent();
            studentID = id;
           this. DataContext = this;

            LoadCoursesNotEnrolled();
        }

        //load unenrolled student
        private void LoadCoursesNotEnrolled()
        {
            try
            {
                using (var context = new SampleContext())
                {
                    // Retrieve courses not enrolled by the student
                    var courses = context.Courses
                        .Where(c => !c.Enrollments.Any(e => e.StudentId == studentID))
                        .Select(c => new StudentEnrolledViewModel
                        {
                            CourseId = c.CourseId,
                            Name = c.Name,
                            Price = c.Price,
                            TeacherName = c.Teacher.Name,
                            EnrolledStudents = c.Enrollments.Count(),
                            TotalStudentLimit = c.TotalStudentLimit
                        })
                        .ToList();

                    // Populate the ObservableCollection
                    CoursesNotEnrolled.Clear();
                    foreach (var course in courses)
                    {
                        CoursesNotEnrolled.Add(course);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void refresh_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LoadCoursesNotEnrolled();
            search_textbox.Text = "";
            search_textbox.Focus();
        }

        private void search_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(CoursesNotEnrolled).Filter = item =>
            {
                var data = item as StudentEnrolledViewModel;
              
                // Apply filter logic
                return string.IsNullOrEmpty(search_textbox.Text) ||
                       data.Name.IndexOf(search_textbox.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       data.TeacherName.IndexOf(search_textbox.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            };
        }

        private void Enrolled_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Student_Enrollemnt_data_grid.SelectedItem is StudentEnrolledViewModel selectedData)
            {
                StudentEnrolledViewModel data = selectedData;

                if (data.TotalStudentLimit>data.EnrolledStudents) 
                {
                    try
                    {
                       
                        using (var context = new SampleContext())
                        {
                            if(context.Challans.FirstOrDefault(x=>x.StudentId==studentID && x.ChallanStatus.ToLower()=="unpaid") ==null)
                            {
                                Enrollment enrollmentdata = new Enrollment();
                                enrollmentdata.StudentId = studentID;
                                enrollmentdata.CourseId = data.CourseId;
                                context.Add(enrollmentdata);
                                context.SaveChanges();
                                CoursesNotEnrolled.Remove(data);
                            }
                            else
                            {
                                MessageBox.Show($"YOU FIRST HAVE TO CLEAR PREVIOUS DUES", "INFORMATION", MessageBoxButton.OK, MessageBoxImage.Information);

                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    return;
                }
                else
                {
                    MessageBox.Show($"Ther is not Seats Left", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
                


            }
            else
            {
                MessageBox.Show("Please select a student to Edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
