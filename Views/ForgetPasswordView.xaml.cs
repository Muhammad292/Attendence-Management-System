using AttendenceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// Interaction logic for ForgetPasswordView.xaml
    /// </summary>
    public partial class ForgetPasswordView : Window
    {
        private int id;
        public ForgetPasswordView()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void frogot_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtbox_user.Text))
            {
                string[] parts = txtbox_user.Text.Split('@');
                if (parts.Length != 2 || string.IsNullOrEmpty(parts[0] ))
                {
                    MessageBox.Show("Invalid email format");
                    return;

                }
                else
                {
                    //string domain = parts[1].ToLower();
                    string domain = parts[1];
                    if (domain.Contains("teacher.com"))
                    {

                        try
                        {
                            using (var db = new SampleContext())
                            {
                                Teacher? obj = db.Teachers.FirstOrDefault(std => std.Email == txtbox_user.Text);

                                if (obj == null)
                                {
                                    MessageBox.Show("Account not found");
                                    return;

                                }
                                else
                                {
                                     id = obj.TeacherId;

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        forgot_frame.Navigate(new Views.forgotPages.ChangePassword(id,true));
                        Header_Grid.Visibility = Visibility.Collapsed;
                        Body_grid.Visibility = Visibility.Collapsed;

                        //MessageBox.Show("Now your new password is your Email");
                        //this.Close();


                    }
                    else if (domain.Contains("student.com"))
                    {
                        try
                        {
                            using (var db = new SampleContext())
                            {
                                Student? obj = db.Students.FirstOrDefault(std => std.Email == txtbox_user.Text);

                                if (obj == null)
                                {
                                    MessageBox.Show("Account not found");
                                    return;
                                }
                                else
                                {
                                    id = obj.StudentId;
                                }
                              

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        forgot_frame.Navigate(new Views.forgotPages.ChangePassword( id,false ) );
                        Header_Grid.Visibility = Visibility.Collapsed;
                        Body_grid.Visibility = Visibility.Collapsed;
                        //MessageBox.Show("Now your new password is your Email");
                        //this.Close();
                    }

                    //MessageBox.Show("Account not found");

                }
            }
            else
            {
                MessageBox.Show($"Please fill the Data", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            

        }
    }
}
