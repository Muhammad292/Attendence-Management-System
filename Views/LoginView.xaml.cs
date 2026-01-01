using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
using AttendenceManagementSystem.Models;

namespace AttendenceManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
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
            if(WindowState != WindowState.Maximized)
            {
                WindowState= WindowState.Maximized;

            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string[] parts = txtbox_user.Text.Split('@');
            if (parts.Length != 2 || string.IsNullOrEmpty(parts[0]))
            {
                MessageBox.Show("Invalid email format");
                return;
                
            }
            else 
            {
                string domain = parts[1].ToLower();
               
                if (domain.ToLower()=="teacher.com")
                {
                    using (var db = new SampleContext())
                    {
                        Teacher? obj = db.Teachers.FirstOrDefault(x => x.Email == txtbox_user.Text && x.Password == txtbox_pass.Password);

                        if (obj == null)
                        {
                            MessageBox.Show("not authenticated");


                        }
                        else
                        {
                            TeacherDashboard teacherDashboard = new TeacherDashboard(obj.TeacherId, obj.Role,obj.Name);
                            Application.Current.MainWindow = teacherDashboard; // Set new main window
                            teacherDashboard.Show();
                            this.Close();
                        }
                    }
                    return;

                }

                if (domain.ToLower() == "student.com")
                {
                    using (var db = new SampleContext())
                    {
                        Student? obj = db.Students.FirstOrDefault(x => x.Email == txtbox_user.Text && x.Password == txtbox_pass.Password);

                        if (obj == null)
                        {
                            MessageBox.Show("not authenticated");


                        }
                        else
                        {
                            //StudentDashboard studentDashboard = new StudentDashboard();
                            StudentDashboard studentDashboard = new StudentDashboard(obj.StudentId,obj.Name);
                            Application.Current.MainWindow = studentDashboard; // Set new main window
                            studentDashboard.Show();
                            this.Close();
                        }
                    }
                        return;
                }

                MessageBox.Show("not authenticated");

            }

            

            

        }

        private void Reset_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ForgetPasswordView forgetPasswordView = new ForgetPasswordView();
            forgetPasswordView.ShowDialog();
        }
    }
   
}
