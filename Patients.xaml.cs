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
    public partial class Patients : Window
    {
        private readonly SqlConnection SqlCon = new SqlConnection("Data Source=DESKTOP-VHAVSBO\\SQLEXPRESS;Initial Catalog=HMS_ABDULMOIZ;Integrated Security=True");
        private readonly SimpleEncryptionHelper encryptionHelper = new SimpleEncryptionHelper();

        public Patients()
        {
            InitializeComponent();
            populate();
        }

        void populate()
        {
            SqlCon.Open();
            string query = "select * from Patienttbl";
            SqlDataAdapter da = new SqlDataAdapter(query, SqlCon);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(da);
            var ds = new DataSet();
            da.Fill(ds);
            patientdatagrid.ItemsSource = ds.Tables[0].DefaultView; // Use DefaultView for WPF DataGrid
            SqlCon.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string patientName = tbPatientname.Text.Trim();
            string patientAddress = tbPatientaddress.Text.Trim();
            string patientPhone = tbPatientphone.Text.Trim();
            string patientAge = tbPatientage.Text.Trim();
            string patientPassword = tbPatientPassword.Password;

            if (string.IsNullOrEmpty(patientName) || string.IsNullOrEmpty(patientAge) || string.IsNullOrEmpty(patientPhone))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            string encryptedPassword = encryptionHelper.Encrypt(patientPassword);
            InsertData(patientName, patientAddress, patientPhone, patientAge, encryptedPassword);

            populate();
            ClearInputs();
        }

        private void InsertData(string patientName, string patientAddress, string patientPhone, string patientAge, string patientPassword)
        {
            try
            {
                SqlCon.Open();
                string query = "INSERT INTO Patienttbl (Patientname, Patientaddress, Patientphone, Patientage, PatientPassword) VALUES (@Patientname, @Patientaddress, @Patientphone, @Patientage, @PatientPassword)";
                SqlCommand cmd = new SqlCommand(query, SqlCon);
                cmd.Parameters.AddWithValue("@Patientname", patientName);
                cmd.Parameters.AddWithValue("@Patientaddress", patientAddress);
                cmd.Parameters.AddWithValue("@Patientphone", patientPhone);
                cmd.Parameters.AddWithValue("@Patientage", patientAge);
                cmd.Parameters.AddWithValue("@PatientPassword", patientPassword);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Patient added successfully!");
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
            tbPatientname.Clear();
            tbPatientaddress.Clear();
            tbPatientphone.Clear();
            tbPatientage.Clear();
            tbPatientPassword.Clear();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)patientdatagrid.SelectedItem;

            if (selectedRow != null)
            {
                string patientId = selectedRow["Patientid"].ToString();
                string updatedName = tbPatientname.Text.Trim();
                string updatedAddress = tbPatientaddress.Text.Trim();
                string updatedPhone = tbPatientphone.Text.Trim();
                string updatedAge = tbPatientage.Text.Trim();
                string updatedPassword = tbPatientPassword.Password;

                if (string.IsNullOrEmpty(updatedName) || string.IsNullOrEmpty(updatedAge) || string.IsNullOrEmpty(updatedPhone))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                string updatedEncryptedPassword = encryptionHelper.Encrypt(updatedPassword);
                UpdateData(patientId, updatedName, updatedAddress, updatedPhone, updatedAge, updatedEncryptedPassword);

                populate();
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }

        private void UpdateData(string updatedPatientId, string updatedName, string updatedAddress, string updatedPhone, string updatedAge, string updatedPassword)
        {
            try
            {
                SqlCon.Open();
                string query = "UPDATE Patienttbl SET Patientname = @Patientname, Patientaddress = @Patientaddress, Patientphone = @Patientphone, Patientage = @Patientage, PatientPassword = @PatientPassword WHERE Patientid = @Patientid";
                SqlCommand cmd = new SqlCommand(query, SqlCon);
                cmd.Parameters.AddWithValue("@Patientid", updatedPatientId);
                cmd.Parameters.AddWithValue("@Patientname", updatedName);
                cmd.Parameters.AddWithValue("@Patientaddress", updatedAddress);
                cmd.Parameters.AddWithValue("@Patientphone", updatedPhone);
                cmd.Parameters.AddWithValue("@Patientage", updatedAge);
                cmd.Parameters.AddWithValue("@PatientPassword", updatedPassword);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Patient updated successfully!");
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
            DataRowView selectedRow = (DataRowView)patientdatagrid.SelectedItem;

            if (selectedRow != null)
            {
                string patientId = selectedRow["Patientid"].ToString();
                DeleteData(patientId);
                populate();
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void DeleteData(string patientId)
        {
            try
            {
                SqlCon.Open();
                string query = "DELETE FROM Patienttbl WHERE Patientid = @Patientid";
                SqlCommand cmd = new SqlCommand(query, SqlCon);
                cmd.Parameters.AddWithValue("@Patientid", patientId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Patient deleted successfully!");
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

        private bool IsValidInput(string input)
        {
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
