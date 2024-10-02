using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Messe_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lbCName.Visibility = Visibility.Hidden;
            lbCAddress.Visibility = Visibility.Hidden;
            tbCName.Visibility = Visibility.Hidden;
            tbCAddress.Visibility = Visibility.Hidden;
            checkHTTP();
           
        }

        private async void checkHTTP()
        {
            var httpService = new HttpService();

            // GET Request
            string getResponse = await httpService.GetAsync("https://localhost:7049/api/Company");
            Console.WriteLine("GET Response: " + getResponse);

            // POST Request
            string jsonPayload = "{\"key\": \"value\"}";
            string postResponse = await httpService.PostAsync("https://api.example.com/post", jsonPayload);
            Console.WriteLine("POST Response: " + postResponse);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            lbCName.Visibility = Visibility.Visible;
            lbCAddress.Visibility = Visibility.Visible;
            tbCName.Visibility = Visibility.Visible;
            tbCAddress.Visibility = Visibility.Visible;
        }

        private void onClickTakeImage(object sender, RoutedEventArgs e)
        {
            Window1 webcamwindow = new Window1();
            webcamwindow.Show();
        }

        public void SetImageFromBase64(string base64String)
        {
            // Step 1: Convert the base64 string to a byte array
            byte[] imageBytes = Convert.FromBase64String(base64String);

            // Step 2: Create a MemoryStream from the byte array
            using (var ms = new MemoryStream(imageBytes))
            {
                // Step 3: Create a BitmapImage and set it to the Image control
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // To make the image cross-thread accessible if needed

                // Step 4: Set the Image.Source to the BitmapImage
                userImage.Source = bitmapImage;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            lbCName.Visibility = Visibility.Hidden;
            lbCAddress.Visibility = Visibility.Hidden;
            tbCName.Visibility = Visibility.Hidden;
            tbCAddress.Visibility = Visibility.Hidden;
        }
    }
}