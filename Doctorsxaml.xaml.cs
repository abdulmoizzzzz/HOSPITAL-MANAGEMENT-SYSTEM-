using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace AbdulMoiz_F20605021_pbl
{
    // HOSPITAL MANAGEMENT SYSTEM //ABDULMOIZ_F20605021
    public partial class Doctorsxaml : Window
    {
        private readonly SqlConnection SqlCon = new SqlConnection("Data Source=DESKTOP-VHAVSBO\\SQLEXPRESS;Initial Catalog=HMS_ABDULMOIZ;Integrated Security=True");
        private readonly SimpleEncryptionHelper encryptionHelper = new SimpleEncryptionHelper();

        public Doctorsxaml()
        {
            InitializeComponent();
            populate(); // Call populate method to load initial data
        }

        void populate()
        {
            SqlCon.Open();
            string query = "select * from Doctortbl";
            SqlDataAdapter da = new SqlDataAdapter(query, SqlCon);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(da);
            var ds = new DataSet();
            da.Fill(ds);
            Doctorsdatagrid.ItemsSource = ds.Tables[0].DefaultView; // Use DefaultView for WPF DataGrid
            SqlCon.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Get values from TextBoxes or other controls
            string doctorName = tbDoctorName.Text.Trim();
            string yearsOfExperience = tbYearsofexperience.Text.Trim();
            string doctorPassword = tbDoctorpassword.Password;

            // Validate input
            if (string.IsNullOrEmpty(doctorName) || string.IsNullOrEmpty(yearsOfExperience))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            // Additional validation logic for yearsOfExperience, if needed
            if (!IsValidInput(doctorName) || !IsValidInput(yearsOfExperience))
            {
                MessageBox.Show("Please enter valid characters.");
                return;
            }

            string encryptedPassword = encryptionHelper.Encrypt(doctorPassword);
            // Insert data into the database
            InsertData(doctorName, yearsOfExperience, encryptedPassword);

            // Refresh the DataGrid
            populate();

            // Clear TextBoxes or other controls
            ClearInputs();
        }

        private void InsertData(string doctorName, string yearsOfexperience, string doctorPassword)
        {
            try
            {
                SqlCon.Open();
                string query = "INSERT INTO Doctortbl (DoctorName, Yearsofexperience, Doctorpassword) VALUES (@DoctorName, @Yearsofexperience, @Doctorpassword)";
                SqlCommand cmd = new SqlCommand(query, SqlCon);
                cmd.Parameters.AddWithValue("@DoctorName", doctorName);
                cmd.Parameters.AddWithValue("@Yearsofexperience", yearsOfexperience);
                cmd.Parameters.AddWithValue("@Doctorpassword", doctorPassword);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Doctor added successfully!");
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
            tbDoctorName.Clear();
            tbYearsofexperience.Clear();
            tbDoctorpassword.Clear();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)Doctorsdatagrid.SelectedItem;

            if (selectedRow != null)
            {
                // Extract the DoctorId from the selected row
                string DoctorId = selectedRow["Doctorid"].ToString();

                // Get values from TextBoxes or other controls
                string updatedName = tbDoctorName.Text.Trim();
                string updatedExperience = tbYearsofexperience.Text.Trim();
                string updatedPassword = tbDoctorpassword.Password;

                // Validate input
                if (string.IsNullOrEmpty(updatedName) || string.IsNullOrEmpty(updatedExperience))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                // Additional validation logic for updatedExperience, if needed
                if (!IsValidInput(updatedName) || !IsValidInput(updatedExperience))
                {
                    MessageBox.Show("Please enter valid characters.");
                    return;
                }

                string updatedEncryptedPassword = encryptionHelper.Encrypt(updatedPassword);
                // Update data into the database
                UpdateData(DoctorId, updatedName, updatedExperience, updatedEncryptedPassword);

                // Refresh the DataGrid
                populate();
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }

        private void UpdateData(string updatedDoctorId, string updatedName, string updatedExperience, string updatedPassword)
        {
            try
            {
                SqlCon.Open();
                string query = "UPDATE Doctortbl SET DoctorName = @DoctorName, Yearsofexperience = @Yearsofexperience, Doctorpassword = @Doctorpassword WHERE Doctorid = @Doctorid";
                SqlCommand cmd = new SqlCommand(query, SqlCon);
                cmd.Parameters.AddWithValue("@Doctorid", updatedDoctorId);
                cmd.Parameters.AddWithValue("@DoctorName", updatedName);
                cmd.Parameters.AddWithValue("@Yearsofexperience", updatedExperience);
                cmd.Parameters.AddWithValue("@Doctorpassword", updatedPassword);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Doctor updated successfully!");
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

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)Doctorsdatagrid.SelectedItem;

            if (selectedRow != null)
            {
                // Extract the DoctorId from the selected row
                string DoctorId = selectedRow["Doctorid"].ToString();

                // Delete data from the database
                DeleteData(DoctorId);

                // Refresh the DataGrid
                populate();
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void DeleteData(string doctorId)
        {
            try
            {
                SqlCon.Open();
                string query = "DELETE FROM Doctortbl WHERE Doctorid = @Doctorid";
                SqlCommand cmd = new SqlCommand(query, SqlCon);
                cmd.Parameters.AddWithValue("@Doctorid", doctorId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Doctor deleted successfully!");
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
            // we can customize this regular expression based on our specific requirements
            string pattern = "^[a-zA-Z0-9]*$"; // Only allows alphanumeric characters
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        private class SimpleEncryptionHelper
        {
            private readonly Random random = new Random();

            public string Encrypt(string plainText)
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(plainBytes);
            }

            public string Decrypt(string cipherText)
            {
                try
                {
                    byte[] cipherBytes = Convert.FromBase64String(cipherText);
                    return Encoding.UTF8.GetString(cipherBytes);
                }
                catch (FormatException)
                {
                    // Handle invalid base64 string
                    return string.Empty;
                }
            }
        }
    }
}
