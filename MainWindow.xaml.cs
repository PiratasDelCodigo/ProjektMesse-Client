using Messe_Client.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Messe_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private loginwindow login_window;
        private bool login_window_activity_status = false;
        public MainWindow()
        {
            InitializeComponent();
            lbCName.Visibility = Visibility.Hidden;
            lbCAddress.Visibility = Visibility.Hidden;
            tbCName.Visibility = Visibility.Hidden;
            tbCAddress.Visibility = Visibility.Hidden;
            checkHTTP();
            admin_TabItem.Visibility = Visibility.Hidden;
            Handler.login_window_activity_status = false;
        }
        private async void checkHTTP()
        {
            var httpService = new HttpService();

            // GET Request
            try
            {
                string getCompanyResponse = await httpService.GetAsync("https://localhost:7049/api/Company");
                string getProductGroupResponse = await httpService.GetAsync("https://localhost:7049/api/ProductGroup");

                Company[] companies = JsonConvert.DeserializeObject<Company[]>(getCompanyResponse);
                ProductGroup[] productGroups = JsonConvert.DeserializeObject<ProductGroup[]>(getProductGroupResponse);

                companyComboBox.ItemsSource = companies;
                companyComboBox.DisplayMemberPath = "companyName";
                companyComboBox.SelectedValuePath = "id";

                favoriteComboBox.ItemsSource = productGroups;
                favoriteComboBox.DisplayMemberPath = "groupName";
                favoriteComboBox.SelectedValuePath = "id";
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Error with GET request: " + ex.Message);
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Error deserializing GET response: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred in GET request: " + ex.Message);
            }
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

        public void logout()
        {
            //API integration für logout!
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem is TabItem selectedTab)
            {
                if (selectedTab.Name == "admin_TabItem")
                {
                    
                }
                else if(selectedTab.Name == "user_TabItem")
                {
                    //Wenn User Tab offen ist
                    
                }
            }
        }

        private async void btCompanies_Click(object sender, RoutedEventArgs e)
        {
            var httpService = new HttpService();

            // GET Request with error handling
            try
            {
                string getResponse = await httpService.GetAsync("https://localhost:7049/api/Company");
                Console.WriteLine("GET Response: " + getResponse);

                // Attempt to deserialize the response
                Company[] companies = JsonConvert.DeserializeObject<Company[]>(getResponse);

                // If successful, initialize and show the data window
                Window2 datawindow = new Window2(companies);
                datawindow.Show();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Error with GET request: " + ex.Message);
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Error deserializing GET response: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }

        private async void btProductGroup_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private async void btCustomer_Click(object sender, RoutedEventArgs e)
        {
            var httpService = new HttpService();

            // GET Request with error handling
            try
            {
                string getResponse = await httpService.GetAsync("https://localhost:7049/api/Customer");
                Console.WriteLine("GET Response: " + getResponse);

                // Attempt to deserialize the response
                Customer[] customers = JsonConvert.DeserializeObject<Customer[]>(getResponse);

                // If successful, initialize and show the data window
                Window2 datawindow = new Window2(customers);
                datawindow.Show();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Error with GET request: " + ex.Message);
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Error deserializing GET response: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }

        }

        public void Admin_Tab_Controll()
        {
            if (Handler.admin_tab_allowed == true)
            {
                //enable admin tab
                admin_TabItem.Visibility = Visibility.Visible;
            }
            else
            {
                //disable admin tab
                admin_TabItem.Visibility = Visibility.Hidden;
                tabControl.SelectedItem = user_TabItem;
            }
        }

        private void user_grid_loginbutton_Click(object sender, RoutedEventArgs e)
        {
            if(Handler.signed_in == false)
            { 
                if (login_window == null)
                {
                    login_window = new loginwindow(this);
                    login_window.Show();
                }
                else
                {
                    login_window.Close();
                    Handler.login_window_activity_status = false;
                    login_window = null;
                    login_window = new loginwindow(this);
                    login_window.Show();
                    Handler.login_window_activity_status = true;
                }
                if (login_window.IsActive == true)
                {
                    if (login_window.IsVisible == false)
                    {
                        //Wenn Loginwindow hidden
                        if (Handler.signed_in == false)
                        {
                            //Noch nicht eingeloggt, Loginwindow hidden
                            login_window.Show();
                            login_window.Focus();
                            Handler.login_window_activity_status = true;
                        }
                        else
                        {
                            //Eingeloggt, Loginwindow hidden --> do nothing
                            Trace.WriteLine($"Already signed in as {Handler.username}");
                            
                        }
                    }
                    else if (login_window.IsVisible == true)
                    {
                        //Loginwindow shown -- do nothing (expand if necessary)
                    }
                }
                else
                {
                    login_window.Close();
                    Handler.login_window_activity_status = false;
                    login_window = null;
                    login_window = new loginwindow(this);
                    login_window.Show();
                    Handler.login_window_activity_status = true;
                }
            }
            else
            {
                //logged in, do nothing
            }
        }

        private void SignOut_Button_Click(object sender, RoutedEventArgs e)
        {
            login_window.clearInputs();
            login_window.Close();
            Handler.login_window_activity_status = false;
            logout();
            Handler.signed_in = false;
            Handler.admin_tab_allowed = false;
            Admin_Tab_Controll();
            MessageBox.Show("Logout erfolgreich!");
        }
    }
}