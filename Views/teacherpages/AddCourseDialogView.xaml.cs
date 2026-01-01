using AttendenceManagementSystem.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace AttendenceManagementSystem.Views.teacherpages
{
    /// <summary>
    /// Interaction logic for AddCourseDialogView.xaml
    /// </summary>
    public partial class AddCourseDialogView : Window
    {
        private int teacherID ;
        public AddCourseDialogView( int id)
        {
            InitializeComponent();
            teacherID = id;
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Check if the window is maximized
            if (WindowState == WindowState.Maximized)
            {
                // Restore the window to normal state
                WindowState = WindowState.Normal;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool is_LimitOrPrice_Valid()
        {

            if (!Regex.IsMatch(limit_txtBox.Text.ToString(), @"^\d+$") || limit_txtBox.Text.ToString().TrimStart('0').Length < 2 || limit_txtBox.Text.ToString().TrimStart('0').Length > 4)
            {



                MessageBox.Show("Invalid for limit input");
              
                return false;
            }

            if (!Regex.IsMatch(price_txtBox.Text.ToString(), @"^\d+$") || price_txtBox.Text.ToString().TrimStart('0').Length < 2 || price_txtBox.Text.ToString().TrimStart('0').Length > 4)
            {

                MessageBox.Show("Invalid for price input");

                return false;
            }
            //return true if valid
            return true;
        }

      

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(coursename_txtBox.Text) ||string.IsNullOrWhiteSpace(coursename_txtBox.Text)|| string.IsNullOrEmpty(price_txtBox.Text) || string.IsNullOrWhiteSpace(price_txtBox.Text)||
                string.IsNullOrEmpty(limit_txtBox.Text)|| string.IsNullOrWhiteSpace(limit_txtBox.Text) || coursename_txtBox.Text.Length< 2  )
            {
                MessageBox.Show($"Error : The Data  YOU PROVIDE IS INCOMPLETE", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(  !is_LimitOrPrice_Valid()  )
            {
                //error
                return;
            }
            else
            {
               
                //save course

                try
                {
                    using (var context = new SampleContext())
                    {
                        var obj= context.Courses.FirstOrDefault(course=> course.Name.ToLower()== coursename_txtBox.Text.ToLower() && course.TeacherId==teacherID  );
                        if (obj == null)
                        {
                            var newCourse = new Course
                            {
                                Name = coursename_txtBox.Text,
                                TotalStudentLimit = Convert.ToInt32(limit_txtBox.Text.TrimStart('0')),
                                Price = Convert.ToInt32(price_txtBox.Text.TrimStart('0')),
                                TeacherId = teacherID
                            };
                            context.Courses.Add(newCourse);
                            context.SaveChanges();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show($"Error : This Course is Already Registered in your Name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error Occured By trying to Add New Course: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

    }
}
