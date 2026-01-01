using AttendenceManagementSystem.Models;
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
    /// Interaction logic for AddTeacherPage.xaml
    /// </summary>
    public partial class AddTeacherPage : Page
    {
        public ObservableCollection<Teacher> TeacherDetails { get; set; } = new ObservableCollection<Teacher>();
        private int CurentTeacherID;
        public AddTeacherPage(int id)
        {
            InitializeComponent();
            CurentTeacherID = id;
            LoadTeacherData();
            this.DataContext = this;
            
        }



         private void LoadTeacherData()
        {
            try
            {
                using (SampleContext context = new SampleContext())
                {
                    // Fetch students from the database and add them to the ObservableCollection
                    var teachers_lst = context.Teachers.ToList(); // Replace 'Students' with your DbSet<Student> property
                    TeacherDetails.Clear();
                    foreach (var student in teachers_lst)
                    {
                        TeacherDetails.Add(student);
                    }
                    teacher_datagrid.ItemsSource = TeacherDetails;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Teachers Data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void search_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(TeacherDetails).Filter = item =>
            {
                var data = item as Teacher;
                return (string.IsNullOrEmpty(search_textbox.Text) ||
                        data.Name.IndexOf(search_textbox.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        data.Role.IndexOf(search_textbox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            };
        }

        private void refresh()
        {
            search_textbox.Text = "";
            LoadTeacherData();
            search_textbox.Focus();
        }
        private void refresh_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
           refresh();
        }

        private void Addteacher_btn_Click(object sender, RoutedEventArgs e)
        {
            AddTeacherView forgetPasswordView = new AddTeacherView();
            forgetPasswordView.ShowDialog();
            refresh();
            
        }

        private void EditTeacher_btn_Click(object sender, RoutedEventArgs e)
        {

            if (teacher_datagrid.SelectedItem is Teacher temp)
            {
                // Store the selected teacher for editing
                Teacher selectedTeacher = temp;

                AddTeacherView forgetPasswordView = new AddTeacherView( selectedTeacher);
                forgetPasswordView.ShowDialog();
                refresh();

            }
            else
            {
                MessageBox.Show("Please select a student to Edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
           
           
        }

        private void DeleteTeacher_btn_Click(object sender, RoutedEventArgs e)
        {
            if (teacher_datagrid.SelectedItem is Teacher temp)
            {
                // Store the selected teacher for editing
                Teacher selectedTeacher = temp;
                if(CurentTeacherID==selectedTeacher.TeacherId)
                {
                    MessageBox.Show($"Error : This ID is cuurrntly is you using", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    
                    using (SampleContext context = new SampleContext())
                    {
                        context.Teachers.Remove(selectedTeacher);
                        context.SaveChanges();
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                refresh();
            }
            else
            {
                MessageBox.Show("Please select a student to Delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }
    }
}
