
using System.Windows;


namespace AbdulMoiz_F20605021_pbl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the LOGIN window
            LOGIN loginWindow = new LOGIN();
            // Show the LOGIN window
            loginWindow.Show();
            this.Visibility = Visibility.Hidden;


        }
       

    }
}
