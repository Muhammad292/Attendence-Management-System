using AttendenceManagementSystem.Models;
using AttendenceManagementSystem.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace AttendenceManagementSystem.Views.teacherpages
{

    public class AttendanceEditRepository
    {
        private readonly SampleContext _context;

        public AttendanceEditRepository(SampleContext context)
        {
            _context = context;
        }

        public List<TeacherAttendanceViewModel> GetAttendanceByTeacherId(int teacherId, DateOnly date, string course)
        {
            return [.. _context.Attendances
                    .Include(a => a.Course)
                    .Include(a => a.Student)
                    .Where(a => a.Course.TeacherId == teacherId && a.AttendanceDate == date &&a.CourseName == course )
                    .Select(a => new TeacherAttendanceViewModel
                                {
                                    StudentId = a.StudentId,
                                    StudentName = a.Student.Name,
                                    CourseName = a.Course.Name,
                                    Registraton=a.Student.RegNo,
                                    CourseID = a.Course.CourseId,
                                    Date = a.AttendanceDate,
                                    Status = a.Status
                                 })];

        }



    }


    public partial class TeacherAttendanceEdit : Page
    {
        private int teacherID;
        private string coursename;
        private DateOnly _date;

        public ObservableCollection<TeacherAttendanceViewModel> Attendancedata { get; set; } = new ObservableCollection<TeacherAttendanceViewModel>();

        public TeacherAttendanceEdit(int id, string selectedCourse, DateOnly? selectedDate)
        {
            InitializeComponent();
            teacherID = id;
            coursename = selectedCourse;
            _date = (DateOnly)selectedDate;
            LoadAttendanceData();
            this.DataContext = this;
        }

        private void LoadAttendanceData()
        {

            try
            {
                using (var context = new SampleContext())
                {
                    var repository = new AttendanceEditRepository(context);
                    var attendanceList = repository.GetAttendanceByTeacherId(teacherID, _date, coursename);


                    Attendancedata.Clear();



                    foreach (var item in attendanceList)
                    {
                        Attendancedata.Add(item);
                    }

                }

                AttendancedataGridForUpdate.ItemsSource = Attendancedata; // Bind data to DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating course: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void TeacherAttendanceEdit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //currently not required
        }

        private void Status_btn_tab3_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;

            if (button != null)
            {
                // Get the data item (AttendanceData) associated with this row
                TeacherAttendanceViewModel? Attendancedata = button.DataContext as TeacherAttendanceViewModel;

                if (Attendancedata != null)
                {
                    // Toggle the status between "Present" and "Absent"
                    Attendancedata.Status = Attendancedata.Status == "Absent" ? "Present" : "Absent";

                    // If you need to update the database or perform any other action, 
                    // you can call your repository method here.
                    // For example:
                    // _attendanceRepository.UpdateStatus(attendanceData);

                    // Optionally, notify the UI of changes (if you have INotifyPropertyChanged)
                    // This will update the UI binding automatically.
                }
            }
        }

        private void Update_brn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new SampleContext())
                {
                    // Loop through all items in the ObservableCollection
                    foreach (var attendance in Attendancedata)
                    {
                        // Find the corresponding record in the database
                        var dbAttendance = context.Attendances.FirstOrDefault(a =>
                            a.StudentId == attendance.StudentId &&
                            a.CourseId == attendance.CourseID &&
                            a.AttendanceDate == attendance.Date);

                        if (dbAttendance != null)
                        {
                            // Update the status
                            dbAttendance.Status = attendance.Status;
                            // Save changes to the database
                            context.SaveChanges();
                        }
                    }



                    MessageBox.Show("Attendance statuses updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating attendance: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back_btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Trigger navigation
            var previousPage = new TeacherAttendance(teacherID);
            NavigationService.GetNavigationService(this)?.Navigate(previousPage);
        }

        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new SampleContext())
                {
                    // Loop through all items in the ObservableCollection
                    foreach (var attendance in Attendancedata) // Use ToList to avoid modifying the collection during iteration
                    {
                        // Find the corresponding record in the database
                        var dbAttendance = context.Attendances.FirstOrDefault(a =>
                            a.StudentId == attendance.StudentId &&
                            a.CourseId == attendance.CourseID &&
                            a.AttendanceDate == attendance.Date);

                        if (dbAttendance != null)
                        {
                            // Remove the record from the database
                            context.Attendances.Remove(dbAttendance);
                            context.SaveChanges();
                        }
                    }

                  

                    MessageBox.Show("Selected attendance records deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadAttendanceData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting attendance records: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //for dATA GRID

        private void deletebtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var attendance = button?.DataContext as TeacherAttendanceViewModel;

            if (attendance != null)
            {
                if (MessageBox.Show($"Are you sure you want to delete ?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (var context = new SampleContext())
                        {
                            var dbAttendance = context.Attendances.FirstOrDefault(a =>
                                        a.StudentId == attendance.StudentId &&
                                        a.CourseId == attendance.CourseID &&
                                        a.AttendanceDate == attendance.Date);
                            if (dbAttendance != null)
                            {
                                // Remove the record from the database
                                context.Attendances.Remove(dbAttendance);
                                context.SaveChanges();
                            }


                        }
                        LoadAttendanceData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting course: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }


        //exporti csv
        public static void ExportDataGridToCsv(DataGrid dataGrid, string filePath)
        {
            try
            {
                StringBuilder csvContent = new StringBuilder();

                // Write header row
                foreach (var column in dataGrid.Columns)
                {
                    csvContent.Append(column.Header.ToString() + ",");
                }
                csvContent.AppendLine();

                // Write data rows
                foreach (var item in dataGrid.Items)
                {
                    if (item is not null)
                    {
                        foreach (var column in dataGrid.Columns)
                        {
                            // Get the value of the cell
                            if (column.GetCellContent(item) is TextBlock cellContent)
                            {
                                csvContent.Append(cellContent.Text + ",");
                            }
                        }
                        csvContent.AppendLine();
                    }
                }

                // Save to file
                File.WriteAllText(filePath, csvContent.ToString());
                MessageBox.Show("Data exported successfully!", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            // Prompt user to select file location
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = "ExportedData.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                // Call the export method
                ExportDataGridToCsv(AttendancedataGridForUpdate, saveFileDialog.FileName);
            }
        }
    }
}
