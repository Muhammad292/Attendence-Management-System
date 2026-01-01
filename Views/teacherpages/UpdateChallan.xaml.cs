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
    /// Interaction logic for UpdateChallan.xaml
    /// </summary>
    public partial class UpdateChallan : Page
    {
        private int teacherID;
    
        public ObservableCollection<Challan> ChallanData { get; set; } = new ObservableCollection<Challan>();
        public UpdateChallan(int id)
        {
            InitializeComponent();
            teacherID = id;
            loadDATA();
            this.DataContext = this;
        }

        private void loadDATA()
        {
            try
            {
                using (var context = new SampleContext())
                {
                    var challans = context.Challans.Where(c => c.ChallanStatus.ToLower() == "unpaid").ToList();

                    ChallanData.Clear();
                    foreach (var challan in challans)
                    {
                       
                        ChallanData.Add(challan);

                    }

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading challan data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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

        private void paid_btn_Click(object sender, RoutedEventArgs e)
        {
            if (StudentChallan_dataGrid.SelectedItem is Challan temp)
            {
                // Store the selected teacher for editing
                Challan selectedChallan = temp;
                try
                {
                    using (var context = new SampleContext())
                    {
                        var obj = context.Challans.FirstOrDefault(c => c.ChallanId == selectedChallan.ChallanId);
                        if (obj != null)
                        { 
                            obj.ChallanStatus = "Paid";                           ;
                            context.SaveChanges(); // Commit changes to the database
                            ChallanData.Remove(selectedChallan);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating challan: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }



            }
            else
            {
                MessageBox.Show("Please select a challan", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            if (StudentChallan_dataGrid.SelectedItem is Challan temp)
            {
                // Store the selected teacher for editing
                Challan selectedChallan = temp;
                try
                {
                    using (var context = new SampleContext())
                    {
                        var obj = context.Challans.FirstOrDefault(c => c.ChallanId == selectedChallan.ChallanId);
                        if (obj != null)
                        {
                            context.Challans.Remove(obj);
                            context.SaveChanges();
                            
                            ChallanData.Remove(selectedChallan);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error Deleting challan: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }



            }
            else
            {
                MessageBox.Show("Please select a challan", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
