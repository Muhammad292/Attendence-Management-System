using AttendenceManagementSystem.Models;
using Microsoft.Win32;
using System.IO;
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
using System.Printing;

namespace AttendenceManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for AccountSettings.xaml
    /// </summary>
    public partial class AccountSettings : Window
    {
        private string filePath = "";
        Teacher teacher = new Teacher();
        Student student = new Student();
        private bool isteacher = false;

        public AccountSettings()
        {
            InitializeComponent();

        }

        public AccountSettings(Teacher obj)
        {
            InitializeComponent();
            teacher = obj;
            isteacher = true;
            loadData();
                
        }

        public AccountSettings(Student obj)
        {
            InitializeComponent();
            student = obj;
            isteacher=false;
            loadData();

        }
        private void loadData()
        {
            //for teacher account setting
            if (isteacher) 
            {
                try
                {

                    if (teacher != null)
                    {
                        teacher_name_txtBox.Text = teacher.Name;
                        //teacher_role.Content = teacher.Role;
                        //teacher_Email_txtBox.Text = teacher.Email;
                        teacher_password_txtBox.Text = teacher.Password;

                        if (teacher.ProfileImage != null)
                        {
                            // Convert byte[] to BitmapImage
                            BitmapImage bitmap = new BitmapImage();
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
                        else
                        {
                            // Set default image if no profile image is available
                            //innerEllipse.Fill = new SolidColorBrush(Colors.LightGray);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            //for student account setting
            else
            {
                try
                {

                    if (student != null)
                    {
                        teacher_name_txtBox.Text = student.Name;
                        //std_Reg_txtBox.Text = student.RegNo;
                        //std_Email_txtBox.Text = student.Email;
                        teacher_password_txtBox.Text  = student.Password;

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
                                innerEllipse.Fill = imageBrush;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            // Set default image if no profile image is available
                            //innerEllipse.Fill = new SolidColorBrush(Colors.LightGray);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
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

        // --------------------fro null validaiation
        public bool isvalid()
        {
            string name = teacher_name_txtBox.Text;
            string password = teacher_password_txtBox.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(name) )
            {
                return false;
            }

            return true;
        }
        private void Edit_btn_Click(object sender, RoutedEventArgs e)
        {

            if (!isvalid())
            {
                MessageBox.Show($"Fill The Data Correctly", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(isteacher)
            {
                try
                {
                    teacher.Name = teacher_name_txtBox.Text;

                    teacher.Password = teacher_password_txtBox.Text;



                    using (SampleContext context = new SampleContext())
                    {
                        var std = context.Teachers.FirstOrDefault(x => x.TeacherId == teacher.TeacherId);
                        if (std != null)
                        {
                            std.Name = teacher.Name;
                            std.Role = teacher.Role;
                            std.Email = teacher.Email;
                            std.Password = teacher.Password;
                            std.ProfileImage = teacher.ProfileImage;

                            context.SaveChanges();
                            this.Close();

                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    student.Name = teacher_name_txtBox.Text;

                    student.Password = teacher_password_txtBox.Text;



                    using (SampleContext context = new SampleContext())
                    {
                        var std = context.Students.FirstOrDefault(x => x.StudentId == student.StudentId);
                        if (std != null)
                        {
                            std.Name = student.Name;
                            std.RegNo = student.RegNo;
                            std.Email = student.Email;
                            std.Password = student.Password;
                            std.ProfileImage = student.ProfileImage;

                            context.SaveChanges();
                            this.Close();

                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
           
        }

        private void innerEllipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Create an OpenFileDialog to select an image
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the filter to show only image files
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == true)
            {
                // Get the selected image file path
                filePath = openFileDialog.FileName;

                if (isteacher)
                {
                    teacher.ProfileImage = string.IsNullOrEmpty(filePath) ? null : File.ReadAllBytes(filePath);
                }
                else
                {
                    student.ProfileImage = string.IsNullOrEmpty(filePath) ? null : File.ReadAllBytes(filePath);
                }
                
                //MessageBox.Show(filePath);

                // Create an ImageBrush to set the image in the Ellipse
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri(filePath));

                // Apply the ImageBrush to the Ellipse's Fill property
                innerEllipse.Fill = imageBrush;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
