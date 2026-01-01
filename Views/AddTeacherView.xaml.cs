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

namespace AttendenceManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for AddTeacherView.xaml
    /// </summary>
    public partial class AddTeacherView : Window
    {

        private string filePath = "";
        Teacher teacher = new Teacher();
        private string previousEmail = "";
        public AddTeacherView()
        {
            InitializeComponent();
            teacher_role.Content = "teacher";  
            Edit_btn.Visibility = Visibility.Collapsed;
        }

        public AddTeacherView(Teacher obj)
        {
            InitializeComponent();
            Addbtn.Visibility=Visibility.Collapsed; 
            teacher = obj;
           
            loadTeacherData();
        }

        private void loadTeacherData()
        {
            try
            {
              
                if (teacher != null)
                {
                    teacher_name_txtBox.Text = teacher.Name;
                    teacher_role.Content=teacher.Role;
                    teacher_Email_txtBox.Text = teacher.Email;
                    previousEmail = teacher.Email;
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
                //}
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
        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Check if the window is maximized
            if (WindowState == WindowState.Maximized)
            {
                // Restore the window to normal state
                WindowState = WindowState.Normal;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                teacher.ProfileImage = string.IsNullOrEmpty(filePath) ? null : File.ReadAllBytes(filePath);
                //MessageBox.Show(filePath);

                // Create an ImageBrush to set the image in the Ellipse
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri(filePath));

                // Apply the ImageBrush to the Ellipse's Fill property
                innerEllipse.Fill = imageBrush;
            }
        }
        // --------------------fro eamil validaiation
        public bool isEmailValid()
        {
            string email = teacher_Email_txtBox.Text;

            if (email.Count(c => c == '@') != 1)
            {

                return false;
            }
            string[] parts = email.Split('@');
            if (parts.Length != 2 ||  string.IsNullOrEmpty(parts[0]) )
            {

                return false;
            }
            string domain = parts[1];

            if (!(domain.ToLower() == "teacher.com"))
            {
                return false;
            }

            return true;
        }


        // --------------------fro null validaiation
        public bool isvalid()
        {
            string name = teacher_name_txtBox.Text;
            string email = teacher_Email_txtBox.Text;
           
            string password = teacher_password_txtBox.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) )
            {
                return false;
            }

            return true;
        }

        public bool isPasswordValid()
        {
            if (teacher_password_txtBox.Text.Length < 8)
            {
                MessageBox.Show($"Paswword must have a length of 8 ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            else if (!isEmailValid())
            {
                MessageBox.Show($"Email format should be abc@student.com", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if(!isPasswordValid())
            {
                return;
            }
            try
            {
                teacher.Name = teacher_name_txtBox.Text;
                teacher.Email = teacher_Email_txtBox.Text;
                teacher.Password = teacher_password_txtBox.Text;
                teacher.Role = teacher_role.Content.ToString();


                

                using (SampleContext context = new SampleContext())
                {
                    if (teacher.Email != previousEmail)
                    {
                        if (context.Teachers.FirstOrDefault(x => x.Email == teacher.Email) == null)
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
                        else
                        {
                            // Throw a custom exception with a specific message
                            throw new Exception("This Email already exists.");
                        }

                    }
                    else
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

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }

        private void Addbtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isvalid())
            {
                MessageBox.Show($"Fill The Data Correctly", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!isEmailValid())
            {
                MessageBox.Show($"Email format should be abc@teacher.com", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!isPasswordValid())
            {
                return;
            }

            teacher.Name = teacher_name_txtBox.Text;
            teacher.Email = teacher_Email_txtBox.Text;
            teacher.Password = teacher_password_txtBox.Text;
            teacher.Role = teacher_role.Content.ToString();
            if (!string.IsNullOrEmpty(filePath))
            {
                teacher.ProfileImage = File.ReadAllBytes(filePath);
            }

            try
            {
                using (SampleContext context = new SampleContext())
                {
                    if (context.Teachers.FirstOrDefault(x=>x.Email==teacher.Email)==null)
                    {
                        context.Teachers.Add(teacher);
                        context.SaveChanges();
                        this.Close();
                    }
                    else
                    {
                        // Throw a custom exception with a specific message
                        throw new Exception("This Email already exists.");
                    }
                    
                }

              

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void teacher_role_Click(object sender, RoutedEventArgs e)
        {
            if(teacher_role.Content=="teacher")
            {
                teacher_role.Content = "admin";
            }
            else
            {
                teacher_role.Content = "teacher";
            }
        }

        
    }
}
