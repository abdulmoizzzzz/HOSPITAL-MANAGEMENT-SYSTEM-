using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace AbdulMoiz_F20605021_pbl
{
    public partial class LOGIN : Window
    {
        private readonly SqlConnection SqlCon = new SqlConnection("Data Source=DESKTOP-VHAVSBO\\SQLEXPRESS;Initial Catalog=HMS_ABDULMOIZ;Integrated Security=True");

        public LOGIN()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Check if both username and password are provided
            if (string.IsNullOrWhiteSpace(tbusername.Text) || string.IsNullOrWhiteSpace(tbpassword.Password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            // Check if the checkbox is checked
            if (!agreeCheckbox.IsChecked.Value)
            {
                MessageBox.Show("Please agree to the terms before logging in.");
                return;
            }

            // Validate the entered username and password
            if (ValidateUser(tbusername.Text, tbpassword.Password))
            {
                // If valid, open the Home window
                Home HomeWindow = new Home();
                HomeWindow.Show();
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private bool ValidateUser(string username, string password)
        {
            try
            {
                SqlCon.Open();

                // Check if the entered credentials exist in the Patienttbl
                string patientQuery = "SELECT * FROM Patienttbl WHERE Patientname = @Username";
                SqlCommand patientCmd = new SqlCommand(patientQuery, SqlCon);
                patientCmd.Parameters.AddWithValue("@Username", username);

                SqlDataReader patientReader = patientCmd.ExecuteReader();
                if (patientReader.HasRows)
                {
                    // User is a patient, check the password
                    patientReader.Read();
                    string storedEncryptedPassword = patientReader["PatientPassword"].ToString();
                    patientReader.Close();

                    // Decrypt the stored password and check if it matches the entered password
                    string decryptedPassword = DecryptPassword(storedEncryptedPassword);
                    return password == decryptedPassword;
                }
                patientReader.Close();

                // Check if the entered credentials exist in the Doctortbl
                string doctorQuery = "SELECT * FROM Doctortbl WHERE DoctorName = @Username";
                SqlCommand doctorCmd = new SqlCommand(doctorQuery, SqlCon);
                doctorCmd.Parameters.AddWithValue("@Username", username);

                SqlDataReader doctorReader = doctorCmd.ExecuteReader();
                if (doctorReader.HasRows)
                {
                    // User is a doctor, check the password
                    doctorReader.Read();
                    string storedEncryptedPassword = doctorReader["DoctorPassword"].ToString();
                    doctorReader.Close();

                    // Decrypt the stored password and check if it matches the entered password
                    string decryptedPassword = DecryptPassword(storedEncryptedPassword);
                    return password == decryptedPassword;
                }
                doctorReader.Close();

                return false; // If not found in both tables or passwords don't match, invalid user
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
            finally
            {
                SqlCon.Close();
            }
        }

        private string DecryptPassword(string encryptedPassword)
        {
            // Implement your decryption logic here
            // For simplicity, you can use a basic decryption algorithm
            byte[] data = Convert.FromBase64String(encryptedPassword);
            return Encoding.UTF8.GetString(data);
        }

        private void agreeCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            // Handle checkbox state change if needed
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            tbpassword.Clear();
            tbusername.Clear();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Home HomeWindow = new Home();
            HomeWindow.Show();
            this.Visibility = Visibility.Hidden;
        }
    }
}
