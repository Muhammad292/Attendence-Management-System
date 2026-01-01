using AttendenceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using AttendenceManagementSystem.ViewModel;
using System.Security.RightsManagement;

namespace AttendenceManagementSystem.Views.teacherpages
{
    /// <summary>
    /// Interaction logic for TeacherCourse.xaml
    /// </summary>
  

    public partial class TeacherCourse : Page
    {
        private int teacherID;
        public ObservableCollection<CourseViewModel> Courses { get; set; } = new ObservableCollection<CourseViewModel>();

        public TeacherCourse(int id )
        {
            InitializeComponent();
            teacherID=id;
            this.DataContext = this;
            LoadCourses();
        }
        private void LoadCourses()
        {
            try
            {
                using (var context = new SampleContext())
                {
                    var courseData = context.Courses.Where(c => c.TeacherId == teacherID).Select(c => new CourseViewModel
                    {
                        CourseId = c.CourseId,
                        Name = c.Name,
                        TotalStudentLimit = c.TotalStudentLimit,
                        Price=c.Price
                    }).ToList();

                    Courses.Clear();
                    foreach (var course in courseData)
                    {
                        Courses.Add(course);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating course: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
            CourseDataGrid.ItemsSource = Courses; // Bind the collection to DataGrid
           

           
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
                var button = sender as Button;
                var course = button?.DataContext as CourseViewModel;

                if (course != null)
                {
                    if (MessageBox.Show($"Are you sure you want to delete {course.Name}?",
                        "Confirm Delete",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            using (var context = new SampleContext())
                            {
                                var dbCourse = context.Courses.FirstOrDefault(c => c.CourseId == course.CourseId);
                                if (dbCourse != null)
                                {
                                    context.Courses.Remove(dbCourse);
                                    context.SaveChanges();
                                }
                            }

                            Courses.Remove(course); // Remove from UI collection
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting course: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            

        }
       
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var course = button?.DataContext as CourseViewModel;

           
            if (course != null )
            {
                try
                {
                    using (var context = new SampleContext())
                    {
                        // Find the course in the database and update the values
                        var dbCourse = context.Courses.FirstOrDefault(c => c.CourseId == course.CourseId);
                        if (dbCourse != null)
                        {
                            dbCourse.Name = course.Name;
                            dbCourse.TotalStudentLimit = course.TotalStudentLimit;
                            dbCourse.Price = course.Price;
                            context.SaveChanges(); // Commit changes to the database
                            
                        }
                    }
                    //MessageBox.Show("Course updated successfully!");
                    // Refresh the DataGrid after update
                    LoadCourses(); // Make sure you have a method to reload courses from the database
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating course: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

           

        }

        private void TeacherCourserPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LoadCourses();
        }

        private void AddCourseButton_Click(object sender, RoutedEventArgs e)
        {
            AddCourseDialogView win = new AddCourseDialogView( teacherID);
            win.ShowDialog();
            LoadCourses();
            return;
        }

        private void search_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(Courses).Filter = item =>
            {
                var data = item as CourseViewModel;

                // Apply filter logic
                return string.IsNullOrEmpty(search_textbox.Text) ||  data.Name.IndexOf(search_textbox.Text, StringComparison.OrdinalIgnoreCase) >= 0 ;
            };
        }
    }
    

}
