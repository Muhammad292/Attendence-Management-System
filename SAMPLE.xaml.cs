using AttendenceManagementSystem.Models;
using AttendenceManagementSystem.Views;
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
using Microsoft.Win32;



namespace AttendenceManagementSystem
{
    /// <summary>
    /// Interaction logic for SAMPLE.xaml
    /// </summary>
    public partial class SAMPLE : Window
    {
        public SAMPLE()
        {
            InitializeComponent();
            // Define the relative path to the image
            //string fg = "\\Images\\usernameInstead.jpeg";  // Remove extra backslashes

            // string filePath = openFileDialog.FileName;

            //    // Create an ImageBrush to set the image in the Ellipse
            //    ImageBrush imageBrush = new ImageBrush();
            //    imageBrush.ImageSource = new BitmapImage(new Uri(filePath));

            //    // Apply the ImageBrush to the Ellipse's Fill property
            //    innerEllipse.Fill = imageBrush;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            

        }

        private void Logoutbtn_Click(object sender, RoutedEventArgs e)
        {
            TeacherDashboard teacherDashboard = new TeacherDashboard(4,"admin", "samplerun");
            Application.Current.MainWindow = teacherDashboard; // Set new main window
            teacherDashboard.Show();
            this.Close();

            //AdminDasnboard teacherDashboard = new AdminDasnboard();
            //Application.Current.MainWindow = teacherDashboard; // Set new main window
            //teacherDashboard.Show();
            //this.Close();

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
                string filePath = openFileDialog.FileName;
                MessageBox.Show(filePath);

                // Create an ImageBrush to set the image in the Ellipse
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri(filePath));

                // Apply the ImageBrush to the Ellipse's Fill property
                innerEllipse.Fill = imageBrush;
            }
        }
    }
}
