using AttendenceManagementSystem.Models;
using AttendenceManagementSystem.ViewModel;
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

namespace AttendenceManagementSystem.Views.studentpages
{
    /// <summary>
    /// Interaction logic for StudentChallan.xaml
    /// </summary>
    public partial class StudentChallan : Page
    {
        private int studentID;
        private int paid =0;
        private int unpaid=0;
        public ObservableCollection<Challan> ChallanData { get; set; } = new ObservableCollection<Challan>();

        public StudentChallan(int id)
        {
            InitializeComponent();
            studentID = id;
            loadDATA();
            this.DataContext = this;
        }

        private void loadDATA()
        {
            try
            {
                using (var context = new SampleContext())
                {
                    var challans = context.Challans.Where(c => c.StudentId == studentID).ToList();

                    ChallanData.Clear();
                    foreach (var challan in challans)
                    {
                        if (challan.ChallanStatus.ToLower() == "unpaid")
                        {
                            unpaid += challan.Price;
                            ChallanData.Add(challan);
                        }
                        else
                        {
                            paid += challan.Price;
                            ChallanData.Add(challan);
                        }


                    }

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading challan data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            u.Text = unpaid.ToString();
            p.Text = paid.ToString();
            StudentChallan_dataGrid.ItemsSource = ChallanData; // Bind the collection to DataGrid

        }

        private void search_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ChallanData).Filter = item =>
            {
                var data = item as Challan;

                // Apply filter logic
                return string.IsNullOrEmpty(search_textbox.Text) ||
                       data.ChallanId.ToString().IndexOf(search_textbox.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       data.CourseName.IndexOf(search_textbox.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            };
        }
    }
}
