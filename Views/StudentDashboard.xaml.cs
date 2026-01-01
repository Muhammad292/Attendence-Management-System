using AttendenceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace AttendenceManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for StudentDashboard.xaml
    /// </summary>
    public partial class StudentDashboard : Window
    {
        private int studentid;
        Student currentStudent = new Student();
        public StudentDashboard(int id)
        {
            InitializeComponent();
            studentid = id;
            loadProfile();
            StudentFrame.Navigate(new Views.studentpages.StudentAttendence(studentid));
        }
        public StudentDashboard(int id,string name)
        {
            InitializeComponent();
            studentid = id;
            name_txtbox.Text="Welcome "+name;
            loadProfile();
            StudentFrame.Navigate(new Views.studentpages.StudentAttendence(studentid));
        }

        private void loadProfile()
        {
            try
            {
                using (SampleContext context = new SampleContext())
                {
                    var student = context.Students.FirstOrDefault(s => s.StudentId == studentid);

                    if (student != null)
                    {
                        currentStudent= student;

                        if (student.ProfileImage != null)
                        {
                            // Convert byte[] to BitmapImage
                            BitmapImage bitmap = new BitmapImage();
                            try
                            {
                                using (MemoryStream ms = new MemoryStream(student.ProfileImage))
                                {
                                    bitmap.BeginInit();
                                    bitmap.StreamSource = ms;
                                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                    bitmap.EndInit();
                                }

                                // Set the image to the Ellipse
                                ImageBrush imageBrush = new ImageBrush();
                                imageBrush.ImageSource = bitmap;
                                CircularImage.Fill = imageBrush;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                        }
                    }
                    else
                    {
                        // Set default image if no profile image is available
                        CircularImage.Fill = new SolidColorBrush(Colors.LightGray);
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                WindowState = WindowState.Maximized;

            }
            else
            {
                WindowState = WindowState.Normal;
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }

        private void Attendencebtn_Click(object sender, RoutedEventArgs e)
        {
            StudentFrame.Navigate(new Views.studentpages.StudentAttendence(studentid));
        }

        private void Logoutbtn_Click(object sender, RoutedEventArgs e)
        {
            //StudentFrame.Navigate(new Views.studentpages.StudentAccountSetting());
            LoginView login = new LoginView();
            Application.Current.MainWindow = login; // Set new main window
            login.Show();
            this.Close();
        }

        private void Enrollment_btn_Click(object sender, RoutedEventArgs e)
        {
            StudentFrame.Navigate(new Views.studentpages.StudentEnrollment(studentid));
        }

        private void CircularImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
           

                AddStudentView forgetPasswordView = new AddStudentView(currentStudent);
                forgetPasswordView.ShowDialog();
                loadProfile();


        }

        private void Challan_btn_Click(object sender, RoutedEventArgs e)
        {
            StudentFrame.Navigate(new Views.studentpages.StudentChallan(studentid));
        }
    }
}
