using AttendenceManagementSystem.Models;
using AttendenceManagementSystem.ViewModel;
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

namespace AttendenceManagementSystem.Views.teacherpages
{
    /// <summary>
    /// Interaction logic for TeacherEnrollment.xaml
    /// </summary>
    public partial class TeacherEnrollment : Page
    {
        private int teacherID;

        public TeacherEnrollment(int id)
        {
            InitializeComponent();
            teacherID = id;
            this.DataContext = new EnrollmentViewModel(id);
        }

        private void TeacherEnrollmentPage_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

       

        private void datagrid_delete_btn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
          
            var enrollmentInfo = button?.DataContext as EnrollmentInfo;
            if (enrollmentInfo != null)
            {
                if (MessageBox.Show($"Are you sure you want to unenroll {enrollmentInfo.StudentName} from {enrollmentInfo.CourseName}?",
                    "Confirm Unenroll",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (var context = new SampleContext())
                        {
                            var enrollment = context.Enrollments
                                .FirstOrDefault(e => e.StudentId == enrollmentInfo.StudentId && e.CourseId == enrollmentInfo.CourseId);

                            if (enrollment != null)
                            {
                                context.Enrollments.Remove(enrollment);
                                context.SaveChanges();
                            }
                        }

                        // Remove from UI collection
                        var viewModel = this.DataContext as EnrollmentViewModel;
                        viewModel?.EnrollmentDetails.Remove(enrollmentInfo);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error unenrolling student: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }


        }

        private void refresh_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {

            search_textbox.Text = "";
            search_textbox.Focus();
        }

        private void search_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = this.DataContext as EnrollmentViewModel;
            if (viewModel == null) return;

            // Get the CollectionView for the EnrollmentDetails
            var collectionView = CollectionViewSource.GetDefaultView(viewModel.EnrollmentDetails);

            // Apply the filter
            collectionView.Filter = item =>
            {
                if (item is not EnrollmentInfo enrollment) return false;

                // Check if the search text matches any property (case-insensitive)
                string searchText = search_textbox.Text.Trim();
                return string.IsNullOrEmpty(searchText) ||
                       enrollment.StudentName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       enrollment.CourseName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
            };

            // Refresh the view to apply the filter
            collectionView.Refresh();
        }
    }
}
