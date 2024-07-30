using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace AbdulMoiz_F20605021_pbl   // HOSPITAL MANAGEMENT SYSTEM //ABDULMOIZ_F20605021
{
    public partial class Diagnosis : Window
    {
        private readonly SqlConnection SqlCon = new SqlConnection("Data Source=DESKTOP-VHAVSBO\\SQLEXPRESS;Initial Catalog=HMS_ABDULMOIZ;Integrated Security=True");

        public Diagnosis()
        {
            InitializeComponent();
            populate(); // Call populate method to load initial data
        }

        void populate()
        {
            SqlCon.Open();
            string query = "select * from Diagnosistbl";
            SqlDataAdapter da = new SqlDataAdapter(query, SqlCon);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(da);
            var ds = new DataSet();
            da.Fill(ds);
            Diagnosisdatagrid.ItemsSource = ds.Tables[0].DefaultView; // Use DefaultView for WPF DataGrid

            // Fetch patient IDs from Patienttbl and bind them to the Patientid ComboBox
            SqlCommand patientCmd = new SqlCommand("SELECT Patientid FROM Patienttbl", SqlCon);
            SqlDataReader reader = patientCmd.ExecuteReader();

            while (reader.Read())
            {
                // Assuming you have a ComboBox named Patientid in your XAML
                Patientid.Items.Add(new ComboBoxItem
                {
                    Content = reader["Patientid"],
                    Tag = reader["Patientid"]
                });
            }

            reader.Close();
            SqlCon.Close();
        }

        // INSERTION LOGIC
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Get values from controls
            string patientId = ((ComboBoxItem)Patientid.SelectedItem).Content.ToString(); // Use Content for Patient ID
            string patientName = GetPatientName(patientId);
            string symptoms = tbSymptoms.Text;
            string diagnosis = tbDiagnosis.Text;
            string medicines = tbMedicines.Text;

            // Validate input if needed

            // Insert data into the database
            InsertData(patientId, patientName, symptoms, diagnosis, medicines);

            // Refresh the DataGrid
            populate();

            // Clear TextBoxes or other controls
            ClearInputs();
        }

        // UPDATE LOGIC
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)Diagnosisdatagrid.SelectedItem;

            if (selectedRow != null)
            {
                // Extract the DiagnosisId from the selected row
                string diagnosisId = selectedRow["Diagnosisid"].ToString();

                // Get values from controls
                string updatedPatientId = ((ComboBoxItem)Patientid.SelectedItem).Content.ToString(); // Use Content for Patient ID
                string updatedPatientName = GetPatientName(updatedPatientId);
                string updatedSymptoms = tbSymptoms.Text;
                string updatedDiagnosis = tbDiagnosis.Text;
                string updatedMedicines = tbMedicines.Text;

                // Validate input if needed

                // Update data in the database
                UpdateData(diagnosisId, updatedPatientId, updatedPatientName, updatedSymptoms, updatedDiagnosis, updatedMedicines);

                // Refresh the DataGrid
                populate();
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }

        // DELETE LOGIC
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)Diagnosisdatagrid.SelectedItem;

            if (selectedRow != null)
            {
                // Extract the DiagnosisId from the selected row
                string diagnosisId = selectedRow["Diagnosisid"].ToString();

                // Delete data from the database
                DeleteData(diagnosisId);

                // Refresh the DataGrid
                populate();
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void InsertData(string patientId, string patientName, string symptoms, string diagnosis, string medicines)
        {
            try
            {
                SqlCon.Open();
                string query = "INSERT INTO Diagnosistbl (Patientid, Patientname, Symptoms, Diagnosis, Medicines) " +
                               "VALUES (@Patientid, @Patientname, @Symptoms, @Diagnosis, @Medicines)";
                SqlCommand cmd = new SqlCommand(query, SqlCon);
                cmd.Parameters.AddWithValue("@Patientid", patientId);
                cmd.Parameters.AddWithValue("@Patientname", patientName);
                cmd.Parameters.AddWithValue("@Symptoms", symptoms);
                cmd.Parameters.AddWithValue("@Diagnosis", diagnosis);
                cmd.Parameters.AddWithValue("@Medicines", medicines);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Diagnosis added successfully!");
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

        private void UpdateData(string diagnosisId, string updatedPatientId, string updatedPatientName, string updatedSymptoms, string updatedDiagnosis, string updatedMedicines)
        {
            try
            {
                SqlCon.Open();
                string query = "UPDATE Diagnosistbl SET Patientid = @Patientid, Patientname = @Patientname, " +
                               "Symptoms = @Symptoms, Diagnosis = @Diagnosis, Medicines = @Medicines " +
                               "WHERE Diagnosisid = @Diagnosisid";
                SqlCommand cmd = new SqlCommand(query, SqlCon);
                cmd.Parameters.AddWithValue("@Diagnosisid", diagnosisId);
                cmd.Parameters.AddWithValue("@Patientid", updatedPatientId);
                cmd.Parameters.AddWithValue("@Patientname", updatedPatientName);
                cmd.Parameters.AddWithValue("@Symptoms", updatedSymptoms);
                cmd.Parameters.AddWithValue("@Diagnosis", updatedDiagnosis);
                cmd.Parameters.AddWithValue("@Medicines", updatedMedicines);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Diagnosis updated successfully!");
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

        private void DeleteData(string diagnosisId)
        {
            try
            {
                SqlCon.Open();
                string query = "DELETE FROM Diagnosistbl WHERE Diagnosisid = @Diagnosisid";
                SqlCommand cmd = new SqlCommand(query, SqlCon);
                cmd.Parameters.AddWithValue("@Diagnosisid", diagnosisId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Diagnosis deleted successfully!");
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
            tbSymptoms.Clear();
            tbDiagnosis.Clear();
            tbMedicines.Clear();
            Patientid.SelectedIndex = -1; // Clear the selected item in ComboBox
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Home HomeWindow = new Home();
            HomeWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void Patientid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Patientid.SelectedItem != null)
            {
                // Assuming we want to fetch patient name when the selection changes
                string selectedPatientId = ((ComboBoxItem)Patientid.SelectedItem).Content.ToString(); // Use Content for Patient ID

                // Fetch patient name from Patienttbl based on the selectedPatientId
                string selectedPatientName = GetPatientName(selectedPatientId);

                if (!string.IsNullOrEmpty(selectedPatientName))
                {
                    // Display the fetched patient name or use it as needed
                    MessageBox.Show("Selected Patient Name: " + selectedPatientName);

                    // Set the patient name in the TextBox
                    tbPatientname.Text = selectedPatientName;
                }
                else
                {
                    MessageBox.Show("Error: Unable to retrieve patient name for the selected ID.");
                }
            }
        }

        private string GetPatientName(string patientId)
        {
            string patientName = string.Empty;

            try
            {
                SqlCon.Open();
                string query = "SELECT Patientname FROM Patienttbl WHERE Patientid = @Patientid";
                SqlCommand cmd = new SqlCommand(query, SqlCon);
                cmd.Parameters.AddWithValue("@Patientid", patientId);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    patientName = result.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                SqlCon.Close();
            }

            return patientName;
        }
    }
}
