using AttendenceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace AttendenceManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for TeacherDasnboard.xaml
    /// </summary>
    public partial class TeacherDashboard : Window
    {
        private int teacherID;
        Teacher cuurentTeacher=new Teacher();
        public TeacherDashboard(int id)
        {
            InitializeComponent();
            teacherID = id;
        }

        public TeacherDashboard(int id, string role, string name)
        {
            InitializeComponent();
            teacherID = id;
            name_txtbox.Text = "Welcome " + name+" ("+role+")";
            if (role.ToLower() == "teacher")
            {
                    AddTeacherbtn.Visibility = Visibility.Collapsed;
                    AddStudentbtn.Visibility = Visibility.Collapsed;
                   Challan_btn.Visibility = Visibility.Collapsed;
            }
            loadProfile();
            TeacherFrame.Navigate(new Views.teacherpages.TeacherAttendance(teacherID));
        }

        private void loadProfile()
        {
            try
            {
                using (SampleContext context = new SampleContext())
                {
                    var teacher = context.Teachers.FirstOrDefault(s => s.TeacherId == teacherID);

                    if (teacher != null)
                    {
                        cuurentTeacher = teacher;

                        if (teacher.ProfileImage != null)
                        {
                            // Convert byte[] to BitmapImage
                            BitmapImage bitmap = new BitmapImage();
                            try
                            {
                                using (MemoryStream ms = new MemoryStream(teacher.ProfileImage))
                                {
                                    bitmap.BeginInit();
                                    bitmap.StreamSource = ms;
                                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                    bitmap.EndInit();
                                }

                                // Set the image to the Ellipse
                                ImageBrush imageBrush = new ImageBrush();
                                imageBrush.ImageSource = bitmap;
                                innerEllipse.Fill = imageBrush;
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
                        innerEllipse.Fill = new SolidColorBrush(Colors.LightGray);
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
            TeacherFrame.Navigate(new Views.teacherpages.TeacherAttendance(teacherID));
        }

        private void Coursebtn_Click(object sender, RoutedEventArgs e)
        {
            TeacherFrame.Navigate(new Views.teacherpages.TeacherCourse(teacherID));
        }

        private void Logoutbtn_Click(object sender, RoutedEventArgs e)
        {
            LoginView login = new LoginView();
            Application.Current.MainWindow = login; // Set new main window
            login.Show();
            this.Close();
        }

        private void Enrollment_btn_Click(object sender, RoutedEventArgs e)
        {
           TeacherFrame.Navigate(new Views.teacherpages.TeacherEnrollment(teacherID));

        }

        private void AddStudentbtn_Click(object sender, RoutedEventArgs e)
        {
            TeacherFrame.Navigate(new Views.teacherpages.AddStudentPage());
            //AddStudentView forgetPasswordView = new AddStudentView();
            //forgetPasswordView.ShowDialog();
        }

        private void AddTeacherbtn_Click(object sender, RoutedEventArgs e)
        {
            TeacherFrame.Navigate(new Views.teacherpages.AddTeacherPage(teacherID));
            //AddTeacherView forgetPasswordView = new AddTeacherView();
            //forgetPasswordView.ShowDialog();
        }

        private void innerEllipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AccountSettings forgetPasswordView = new AccountSettings(cuurentTeacher);
            forgetPasswordView.ShowDialog();
            name_txtbox.Text = "Welcome " + cuurentTeacher.Name + " (" + cuurentTeacher.Role + ")";
            loadProfile();
        }

        private void Challan_btn_Click(object sender, RoutedEventArgs e)
        {
            TeacherFrame.Navigate(new Views.teacherpages.UpdateChallan(teacherID));
        }
    }
}
