using System.Windows;

namespace AbdulMoiz_F20605021_pbl   //HMS ABDULMOIZ
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
        }

        // Method to handle the click event of the button related to managing doctors
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Create and show a window for managing doctors
            Doctorsxaml DoctorsWindow = new Doctorsxaml();
            DoctorsWindow.Show();
            // Hide the current home window
            this.Visibility = Visibility.Hidden;
        }

        // Method to handle the click event of the button related to managing patients
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // Create and show a window for managing patients
            Patients PatientsWindow = new Patients();
            PatientsWindow.Show();
            // Hide the current home window
            this.Visibility = Visibility.Hidden;
        }

        // Method to handle the click event of the button related to managing diagnosis information
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // Create and show a window for managing diagnosis information
            Diagnosis DiagnosisWindow = new Diagnosis();
            DiagnosisWindow.Show();
            // Hide the current home window
            this.Visibility = Visibility.Hidden;
        }

        // Method to handle the click event of the button related to managing medicines
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // Create and show a window for managing medicines
            Medicines MedicinesWindow = new Medicines();
            MedicinesWindow.Show();
            // Hide the current home window
            this.Visibility = Visibility.Hidden;
        }

        // Method to handle the click event of the button related to returning to the main window
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            // Create and show the main window
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            // Hide the current home window
            this.Visibility = Visibility.Hidden;
        }
    }
}
