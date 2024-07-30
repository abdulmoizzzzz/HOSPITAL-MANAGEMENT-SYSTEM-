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
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data;

namespace AbdulMoiz_F20605021_pbl
{    // HOSPITAL MANAGEMENT SYSTEM //ABDULMOIZ_F20605021
    /// <summary>
    /// Interaction logic for Medicines.xaml
    /// </summary>
    public partial class Medicines : Window
    {

        private readonly SqlConnection SqlCon = new SqlConnection("Data Source=DESKTOP-VHAVSBO\\SQLEXPRESS;Initial Catalog=HMS_ABDULMOIZ;Integrated Security=True");

        public Medicines()
        {
            InitializeComponent();
            populate(); // Call populate method to load initial data
        }


        void populate()
        {
            SqlCon.Open();
            string query = "select * from Medicinetbl";
            SqlDataAdapter da = new SqlDataAdapter(query, SqlCon);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(da);
            var ds = new DataSet();
            da.Fill(ds);
            Medicinedatagrid.ItemsSource = ds.Tables[0].DefaultView; // Use DefaultView for WPF DataGrid
            SqlCon.Close();
        }


        //INSERTION LOGIC
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Get values from TextBoxes or other controls
            string medicineName = tbMedicinename.Text.Trim();
            string medicineType = tbMedicineType.Text.Trim();
            string prescribedByDoctor = tbbydoctor.Text.Trim();

            // Validate input
            if (string.IsNullOrEmpty(medicineName) || string.IsNullOrEmpty(medicineType) || string.IsNullOrEmpty(prescribedByDoctor))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }


            // Insert data into the database
            InsertData(medicineName, medicineType, prescribedByDoctor);

            // Refresh the DataGrid
            populate();

            // Clear TextBoxes or other controls
            ClearInputs();



        }
        private void InsertData(string medicineName, string medicineType, string prescribedByDoctor)
        {
            try
            {
                SqlCon.Open();
                string query = "INSERT INTO Medicinetbl (Medicinename, MedicineType, bydoctor) VALUES (@Medicinename, @MedicineType, @bydoctor)";
                SqlCommand cmd = new SqlCommand(query, SqlCon);
                cmd.Parameters.AddWithValue("@Medicinename", medicineName);
                cmd.Parameters.AddWithValue("@MedicineType", medicineType);
                cmd.Parameters.AddWithValue("@bydoctor", prescribedByDoctor);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Medicine added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                SqlCon.Close();
            }
        }

        private void ClearInputs()
        {
            // Clear TextBoxes or other controls
            tbMedicinename.Clear();
            tbMedicineType.Clear();
            tbbydoctor.Clear();
        }



        //UPDATION LOGIC
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)Medicinedatagrid.SelectedItem;

            if (selectedRow != null)
            {
                // Extract the MedicineId from the selected row
                string medicineId = selectedRow["Medicineid"].ToString();

                // Get values from TextBoxes or other controls
                string updatedName = tbMedicinename.Text.Trim();
                string updatedType = tbMedicineType.Text.Trim();
                string updatedPrescribedByDoctor = tbbydoctor.Text.Trim();

                // Validate input
                if (string.IsNullOrEmpty(updatedName) || string.IsNullOrEmpty(updatedType) || string.IsNullOrEmpty(updatedPrescribedByDoctor))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                

                // Update data into the database
                UpdateData(medicineId, updatedName, updatedType, updatedPrescribedByDoctor);

                // Refresh the DataGrid
                populate();
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }

            void UpdateData(string updatedMedicineId, string updatedName, string updatedType, string updatedPrescribedByDoctor)
            {
                try
                {
                    SqlCon.Open();
                    string query = "UPDATE Medicinetbl SET Medicinename = @Medicinename, MedicineType = @MedicineType, bydoctor = @bydoctor WHERE Medicineid = @Medicineid";
                    SqlCommand cmd = new SqlCommand(query, SqlCon);
                    cmd.Parameters.AddWithValue("@Medicineid", updatedMedicineId);
                    cmd.Parameters.AddWithValue("@Medicinename", updatedName);
                    cmd.Parameters.AddWithValue("@MedicineType", updatedType);
                    cmd.Parameters.AddWithValue("@bydoctor", updatedPrescribedByDoctor);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Medicine updated successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    SqlCon.Close();
                }
            }




        }



        //DELETION LOGIC
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)Medicinedatagrid.SelectedItem;

            if (selectedRow != null)
            {
                // Extract the MedicineId from the selected row
                string medicineId = selectedRow["Medicineid"].ToString();

                // Delete data from the database
                DeleteData(medicineId);

                // Refresh the DataGrid
                populate();
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }

        }


        private void DeleteData(string medicineId)
        {
            try
            {
                SqlCon.Open();
                string query = "DELETE FROM Medicinetbl WHERE Medicineid = @Medicineid";
                SqlCommand cmd = new SqlCommand(query, SqlCon);
                cmd.Parameters.AddWithValue("@Medicineid", medicineId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Medicine deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                SqlCon.Close();
            }
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Home HomeWindow = new Home();
            HomeWindow.Show();
            this.Visibility = Visibility.Hidden;

        }

        // Validation for irregular characters
        private bool IsValidInput(string input)
        {
            // we can customize this regular expression based on your specific requirements
            string pattern = "^[a-zA-Z0-9]*$"; // Only allows alphanumeric characters
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }



    }
}
