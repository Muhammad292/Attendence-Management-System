using AttendenceManagementSystem.Models;
using Microsoft.IdentityModel.Tokens;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AttendenceManagementSystem.Views.forgotPages
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Page
    {
        private int userID;
        private bool isTeacher;
        public ChangePassword()
        {
            InitializeComponent();
        }

        public ChangePassword(int id,bool flag)
        {
            InitializeComponent();
            userID = id;
            isTeacher = flag;
        }

        private void frogot_btn_Click(object sender, RoutedEventArgs e)
        {
            string password=txtbox_user.Password;
            string cnfrmPassword = txtbox_user_cnfim.Text;
            if(txtbox_user.Password.Length<8|| txtbox_user_cnfim.Text.Length<8||password != cnfrmPassword || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(cnfrmPassword) ||string.IsNullOrWhiteSpace(password) ||string.IsNullOrWhiteSpace(cnfrmPassword)  )
            {
                MessageBox.Show($"Error: Both Data have  minimum 8  characters and all caharacter & sequence should be same", "ERROR",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else
            {
                if(isTeacher)
                {
                    try
                    {
                        using (var db = new SampleContext())
                        {
                            Teacher? obj = db.Teachers.FirstOrDefault(teacher => teacher.TeacherId == userID);

                            if (obj == null)
                            {
                                MessageBox.Show("Account not found");
                                return;

                            }
                            else
                            {
                                obj.Password = password;
                                db.SaveChanges();

                            }

                        }
                            MessageBox.Show($"Information : Password change Successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                            Window currentWindow = Window.GetWindow(this);
                            currentWindow?.Close();
                    }
                    catch (Exception ex)
                     {
                        MessageBox.Show($"Error : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    try
                    {
                        using (var db = new SampleContext())
                        {
                            Student? obj = db.Students.FirstOrDefault(std => std.StudentId == userID);

                            if (obj == null)
                            {
                                MessageBox.Show("Account not found");
                                
                            }
                            else
                            {
                                obj.Password = password;
                                db.SaveChanges();

                            }

                        }
                        MessageBox.Show($"Information : Password change Successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                        Window currentWindow = Window.GetWindow(this);
                        currentWindow?.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            
        }

        private void back_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Close the current window hosting the frame
            Window currentWindow = Window.GetWindow(this);
            currentWindow?.Close();

            //// Open the main window again
            //ForgetPasswordView mainWindow = new ForgetPasswordView();
            //mainWindow.Show();
        }
    }
}
