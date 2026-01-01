using AttendenceManagementSystem.Models;
using AttendenceManagementSystem.ViewModel;
using AttendenceManagementSystem.Views.studentpages;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AttendenceManagementSystem.Views.teacherpages
{
    /// <summary>
    /// Interaction logic for AddStudentPage.xaml
    /// </summary>
    public partial class AddStudentPage : Page
    {
       public ObservableCollection<Student> StudentDetails { get; set; }=new ObservableCollection<Student>();
        public AddStudentPage()
        {
            InitializeComponent();
            LoadStudentData();
            this.DataContext = this;

        }

        private void LoadStudentData()
        {
            try
            {
                using ( SampleContext context=new SampleContext())
                {
                    // Fetch students from the database and add them to the ObservableCollection
                    var students = context.Students.ToList(); // Replace 'Students' with your DbSet<Student> property
                    StudentDetails.Clear();
                    foreach (var student in students)
                    {
                        StudentDetails.Add(student);
                    }
                    Student_datagrid.ItemsSource = StudentDetails;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Student Data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            if (Student_datagrid.SelectedItem is Student selectedStudent)
            {
                // Store the selected student for editing
                Student SelectedStudent = selectedStudent;

                AddStudentView forgetPasswordView = new AddStudentView(SelectedStudent);
                forgetPasswordView.ShowDialog();
                LoadStudentData();

              
            }
            else
            {
                MessageBox.Show("Please select a student to Edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

          
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            if (Student_datagrid.SelectedItem is Student temp)
            {
                // Store the selected student for editing
                Student selectedStudent = temp;

                try
                {
                    using (SampleContext context = new SampleContext())
                    {
                       context.Students.Remove(selectedStudent);
                        context.SaveChanges();
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Open an edit dialog or navigate to an edit page
                //MessageBox.Show($"Editing Student: {SelectedStudent.Name}", "Edit", MessageBoxButton.OK, MessageBoxImage.Information);

                // Logic for editing can go here
                // Example: Navigate to an EditStudentPage with the selected student
                LoadStudentData();
            }
            else
            {
                MessageBox.Show("Please select a student to Delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Addstudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudentView forgetPasswordView = new AddStudentView();
            forgetPasswordView.ShowDialog();
            LoadStudentData();
        }

        private void refresh_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            search_textbox.Text = "";
            LoadStudentData();
            search_textbox.Focus();
        }

        private void search_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            CollectionViewSource.GetDefaultView(StudentDetails).Filter = item =>
            {
                var data = item as Student;
                return (string.IsNullOrEmpty(search_textbox.Text) ||
                        data.Name.IndexOf(search_textbox.Text, StringComparison.OrdinalIgnoreCase) >= 0||
                        data.RegNo.IndexOf(search_textbox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            };
        }
    }
}
