using AttendenceManagementSystem.Models;
using Microsoft.Identity.Client;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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
    /// Interaction logic for AddStudentView.xaml
    /// </summary>
    public partial class AddStudentView : Window
    {
        private string filePath="";
        Student student=new Student();
        private string previousEmail = "";
        public AddStudentView()
        {
            InitializeComponent();
           EditStudent_btn.Visibility=Visibility.Collapsed;         
            
        }

        public AddStudentView(Student obj) 
        {
            InitializeComponent();
            Addstudent.Visibility = Visibility.Collapsed;
            student = obj;
            
            loadStudentData();

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {  
            if (e.LeftButton == MouseButtonState.Pressed)
            {
               

                // Allow the window to be dragged
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
                student.ProfileImage = string.IsNullOrEmpty(filePath) ? null : File.ReadAllBytes(filePath);
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
            string email = std_Email_txtBox.Text;
           
            if(email.Count(c => c == '@') != 1 )
            {
               
                return false;
            }
            string[] parts = email.Split('@');
            if (parts.Length != 2 || string.IsNullOrEmpty(parts[0]))
            {
               
                return false;
            }
            string domain = parts[1];

            if (  !(domain.ToLower() == "student.com"))
            {
                return false;
            }

                return true;
        }


        // --------------------fro null validaiation
        public bool isvalid()
        {
            string name=std_name_txtBox.Text;
            string email = std_Email_txtBox.Text;
            string reg = std_Reg_txtBox.Text;
            string password = std_password_txtBox.Text;

            if( string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(reg) || string.IsNullOrEmpty(password) || 
                string.IsNullOrWhiteSpace(password)|| string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(reg)    )
            {
                return false;
            }

                return true;
        }
        private void Addstudent_Click(object sender, RoutedEventArgs e)
        {
            
            if(!isvalid())
            {
                MessageBox.Show($"Fill The Data Correctly", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if(!isEmailValid())
            {
                MessageBox.Show($"Email format should be abc@student.com", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            else if (!isPasswordValid())
            {
                return;
            }


            student.Name = std_name_txtBox.Text;
            student.RegNo = std_Reg_txtBox.Text;
            student.Email = std_Email_txtBox.Text;
            student.Password = std_password_txtBox.Text;
            if(!string.IsNullOrEmpty(filePath) )
            {
                student.ProfileImage =  File.ReadAllBytes(filePath);
            }
           
            try
            {
                using ( SampleContext context=new SampleContext())
                {
                    if (context.Students.FirstOrDefault(x => x.Email == student.Email) == null)
                    {
                        if (context.Students.FirstOrDefault(x => x.RegNo == student.RegNo) == null)
                        {
                            context.Students.Add(student);
                            context.SaveChanges();
                            this.Close();
                        }
                        else
                        {
                            // Throw a custom exception with a specific message
                            throw new Exception("This Registration number is already exists.");
                        }
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

        public bool isPasswordValid()
        {
            if(std_password_txtBox.Text.Length<8)
            {
                MessageBox.Show($"Paswword must have a length of 8 ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void loadStudentData()
        {
            try
            {
                
                    if (student != null)
                    {
                        std_name_txtBox.Text = student.Name;
                        std_Reg_txtBox.Text = student.RegNo;
                        std_Email_txtBox.Text = student.Email;
                        previousEmail = student.Email;
                        std_password_txtBox.Text = student.Password;

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
                            innerEllipse.Fill = new SolidColorBrush(Colors.LightGray);
                        }
                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void EditStudent_btn_Click(object sender, RoutedEventArgs e)
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
                student.Name = std_name_txtBox.Text;
                student.RegNo = std_Reg_txtBox.Text;
                student.Email = std_Email_txtBox.Text;
                student.Password = std_password_txtBox.Text;

                using (SampleContext context = new SampleContext())
                {
                    if (student.Email != previousEmail)
                    {
                        //var EmailCheck= context.Students.FirstOrDefault(x => x.Email == student.Email);
                        if (context.Students.FirstOrDefault(x => x.Email == student.Email) == null)
                        {
                            //var RegistrationCheck = context.Students.FirstOrDefault(x => x.RegNo == student.RegNo);
                            if (context.Students.FirstOrDefault(x => x.RegNo == student.RegNo) == null)
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
                            else
                            {
                                // Throw a custom exception with a specific message
                                throw new Exception("This Registration number is already exists.");
                            }

                        }
                        else
                        {
                            // Throw a custom exception with a specific message
                            throw new Exception("This Email account is already exists.");
                        }

                    }
                    else
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

                

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
    }
}
