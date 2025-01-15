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
using Messe_Client.Handler;

namespace Messe_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private loginwindow login_window;
        private bool login_window_activity_status = false;
        private string pendingImageBase64 = "BAASD";
        public MainWindow()
        {
            InitializeComponent();
            lbCName.Visibility = Visibility.Hidden;
            lbCAddress.Visibility = Visibility.Hidden;
            lbCCity.Visibility = Visibility.Hidden;
            lbCPostalCode.Visibility = Visibility.Hidden;
            lbCEMail.Visibility = Visibility.Hidden;
            tbCName.Visibility = Visibility.Hidden;
            tbCAddress.Visibility = Visibility.Hidden;
            tbCCity.Visibility = Visibility.Hidden;
            tbCPostalCode.Visibility = Visibility.Hidden;
            tbCEMail.Visibility = Visibility.Hidden;
            prefetch();
            admin_TabItem.Visibility = Visibility.Visible;
            Handler.LoginHandler.login_window_activity_status = false;
        }

        private async Task refresh()
        {
            var httpService = new HttpService();
            string getCompanyResponse = await httpService.GetAsync("https://localhost:7049/api/Company");
            string getProductGroupResponse = await httpService.GetAsync("https://localhost:7049/api/ProductGroup");
            string getCustomerResponse = await httpService.GetAsync("https://localhost:7049/api/Customer");


            if (getCompanyResponse != null)
            {
                JsonHandler.setCompaniesToData(getCompanyResponse);
            }
            else
            {
                Console.WriteLine("Error with GET request: Company");
            }

            if (getProductGroupResponse != null)
            {
                JsonHandler.setProductGroupToData(getProductGroupResponse);
            }
            else
            {
                Console.WriteLine("Error with GET request: Productgroups");
            }

            if (getCustomerResponse != null)
            {
                JsonHandler.setCustomersToData(getCustomerResponse);
            }
            else
            {
                Console.WriteLine("Error with GET request: Customers ");
            }

            (int pendingCount, int successCount) = await JsonHandler.sendPendingData();
            lbPending.Content = "Pending Data: " + pendingCount;
            if(successCount > 0)
            {
                MessageBox.Show($"{successCount} Pending data sent successfully!");
            }
            var timeStamp = JsonHandler.getTimeStamp();
            if (timeStamp != null)
            {
                setTimeStamp((DateTime) timeStamp);
            }
            

        }
        private async void prefetch()
        {
            await refresh();

            // GET Request
            try
            {
                (Company[] companies, var t) = JsonHandler.getCompanyFromJSON();

                companyComboBox.ItemsSource = companies;
                companyComboBox.DisplayMemberPath = "companyName";
                companyComboBox.SelectedValuePath = "id";

                (ProductGroup[] productGroups, var timeStamp) = JsonHandler.getProductGroupsFromJSON();
                if (timeStamp != null)
                {
                    setTimeStamp((DateTime)timeStamp);
                }
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
            lbCCity.Visibility = Visibility.Visible;
            lbCPostalCode.Visibility = Visibility.Visible;
            lbCEMail.Visibility = Visibility.Visible;
            tbCName.Visibility = Visibility.Visible;
            tbCAddress.Visibility = Visibility.Visible;
            tbCCity.Visibility = Visibility.Visible;
            tbCPostalCode.Visibility = Visibility.Visible;
            tbCEMail.Visibility = Visibility.Visible;
        }

        private void onClickTakeImage(object sender, RoutedEventArgs e)
        {
            Window1 webcamwindow = new Window1();
            webcamwindow.Show();
        }

        public void SetImageFromBase64(string base64String)
        {
            // Step 1: Convert the base64 string to a byte array
            pendingImageBase64 = base64String;
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
            lbCCity.Visibility = Visibility.Hidden;
            lbCPostalCode.Visibility = Visibility.Hidden;
            lbCEMail.Visibility = Visibility.Hidden;
            tbCName.Visibility = Visibility.Hidden;
            tbCAddress.Visibility = Visibility.Hidden;
            tbCCity.Visibility = Visibility.Hidden;
            tbCPostalCode.Visibility = Visibility.Hidden;
            tbCEMail.Visibility = Visibility.Hidden;
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

        private void setTimeStamp(DateTime time)
        {
            lbTimestamp.Content = "Last Update: " + time.ToString("dd.MM.yyyy HH:mm:ss");
        }

        private void btCompanies_Click(object sender, RoutedEventArgs e)
        {
            // If successful, initialize and show the data window
            (Company[] companies, DateTime? timeStamp) = JsonHandler.getCompanyFromJSON();
            if (timeStamp != null)
            {
                setTimeStamp((DateTime)timeStamp);
            }
            Window2 datawindow = new Window2(companies != null && companies.Length > 0 ? companies : []);
            datawindow.Show();
        }

        private void btProductGroup_Click(object sender, RoutedEventArgs e)
        {
            (ProductGroup[] productGroups, DateTime? timeStamp) = JsonHandler.getProductGroupsFromJSON();
            if (timeStamp != null)
            {
                setTimeStamp((DateTime)timeStamp);
            }
            Window2 datawindow = new Window2(productGroups != null && productGroups.Length > 0 ? productGroups : []);
            datawindow.Show();
        }

        private void btCustomer_Click(object sender, RoutedEventArgs e)
        {
            (Customer[] customers, DateTime? timeStamp) = JsonHandler.getCustomersFromJSON();
            if (timeStamp != null)
            {
                setTimeStamp((DateTime)timeStamp);
            }
            Window2 datawindow = new Window2(customers != null && customers.Length > 0 ? customers : []);
            datawindow.Show();
        }

        public void Admin_Tab_Controll()
        {
            if (LoginHandler.admin_tab_allowed == true)
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
            if(LoginHandler.signed_in == false)
            { 
                if (login_window == null)
                {
                    login_window = new loginwindow(this);
                    login_window.Show();
                }
                else
                {
                    login_window.Close();
                    LoginHandler.login_window_activity_status = false;
                    login_window = null;
                    login_window = new loginwindow(this);
                    login_window.Show();
                    LoginHandler.login_window_activity_status = true;
                }
                if (login_window.IsActive == true)
                {
                    if (login_window.IsVisible == false)
                    {
                        //Wenn Loginwindow hidden
                        if (LoginHandler.signed_in == false)
                        {
                            //Noch nicht eingeloggt, Loginwindow hidden
                            login_window.Show();
                            login_window.Focus();
                            LoginHandler.login_window_activity_status = true;
                        }
                        else
                        {
                            //Eingeloggt, Loginwindow hidden --> do nothing
                            Trace.WriteLine($"Already signed in as {Handler.LoginHandler.username}");
                            
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
                    LoginHandler.login_window_activity_status = false;
                    login_window = null;
                    login_window = new loginwindow(this);
                    login_window.Show();
                    LoginHandler.login_window_activity_status = true;
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
            LoginHandler.login_window_activity_status = false;
            logout();
            LoginHandler.signed_in = false;
            LoginHandler.admin_tab_allowed = false;
            Admin_Tab_Controll();
            MessageBox.Show("Logout erfolgreich!");
        }

        private async void btSubmit_Click(object sender, RoutedEventArgs e)
        {
            Company? temp = null;
            if (checkIfEmpty())
            {
                MessageBox.Show("Bitte füllen Sie alle Felder aus.");
                return;
            }
            if (cbCreateCompany.IsChecked == false)
            {
                if (companyComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Bitte eine Company auswählen.");
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(tbCName.Text) || string.IsNullOrEmpty(tbCAddress.Text))
                {
                    MessageBox.Show("Bitte füllen Sie alle Felder der neuen Company aus.");
                    return;
                }
                temp = (Company)companyComboBox.SelectedItem;
                
            }
            postNewCustomer(temp);

           

            

        }
        private async void postNewCustomer(Company? customerCompany)
        {
            ProductGroup customerFavorite = (ProductGroup)favoriteComboBox.SelectedItem;

           //@Maxi check if comany schon da, sonst erstellen

            var httpService = new HttpService();
            if(customerCompany != null)
            {
                var dataToPost = new Customer()
                {
                    FirstName = tbFirstName.Text,
                    LastName = tbLastName.Text,
                    Street = tbStreet.Text,
                    PostalCode = tbPostalCode.Text,
                    City = tbCity.Text,
                    Image = pendingImageBase64,
                    FavoriteId = customerFavorite.Id,
                    CompanyId = customerCompany.id
                };

                try
                {
                    string jsonData = JsonConvert.SerializeObject(dataToPost);
                    var response = await httpService.PostAsync<Customer>("https://localhost:7049/api/Customer", jsonData);

                    if (response.HttpResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Data submitted successfully!");
                        clearCustomerDataFromMainWindow();
                    }
                    else
                    {
                        int pendingCount = JsonHandler.setPendingCustomers(dataToPost);
                        lbPending.Content = "Pending Data: " + pendingCount;
                        MessageBox.Show("Error submitting data: " + response.HttpResponse.ReasonPhrase);
                        clearCustomerDataFromMainWindow();
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show("Error with POST request: " + ex.Message);
                }
                catch (JsonException ex)
                {
                    MessageBox.Show("Error serializing data: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message);
                }
            }
            else
            {
                // Handling for company creation
                postNewCompany();
            }
        }

        private async void postNewCompany()
        {
            var httpService = new HttpService();
            var companyToPost = new Company()
            {
                companyName = tbCName.Text,
                street = tbCAddress.Text,
                city = tbCCity.Text,
                postalCode = tbCPostalCode.Text,
                email = tbCEMail.Text

            };

            try
            {
                string jsonData = JsonConvert.SerializeObject(companyToPost);
                var response = await httpService.PostAsync<Company>("https://localhost:7049/api/Company", jsonData);
                if (response.HttpResponse.IsSuccessStatusCode)
                {
                    var companyID = response.Data.id;
                    MessageBox.Show("Data submitted successfully!");
                    postNewCustomer(response.Data);
                }
                else
                {
                    ProductGroup customerFavorite = (ProductGroup)favoriteComboBox.SelectedItem;


                    var customerToPost = new Customer()
                    {
                        FirstName = tbFirstName.Text,
                        LastName = tbLastName.Text,
                        Street = tbStreet.Text,
                        PostalCode = tbPostalCode.Text,
                        City = tbCity.Text,
                        Image = pendingImageBase64,
                        FavoriteId = customerFavorite.Id,
                        PendingCompany = companyToPost
                    };

                    int pendingCount = JsonHandler.setPendingCustomers(customerToPost);
                    lbPending.Content = "Pending Data: " + pendingCount;
                    MessageBox.Show("Error submitting data: " + response.HttpResponse.ReasonPhrase);
                    //clearCutomerDataFromMainWindow();
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show("Error with POST request: " + ex.Message);
            }
            catch (JsonException ex)
            {
                MessageBox.Show("Error serializing data: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message);
            }
        }

        private bool checkIfEmpty()
        {
            if (string.IsNullOrEmpty(tbFirstName.Text) ||
                string.IsNullOrEmpty(tbLastName.Text) ||
                string.IsNullOrEmpty(tbStreet.Text) ||
                string.IsNullOrEmpty(tbPostalCode.Text) ||
                string.IsNullOrEmpty(tbCity.Text) ||
                string.IsNullOrWhiteSpace(tbFirstName.Text) ||
                string.IsNullOrWhiteSpace(tbLastName.Text) ||
                string.IsNullOrWhiteSpace(tbStreet.Text) ||
                string.IsNullOrWhiteSpace(tbPostalCode.Text) ||
                string.IsNullOrWhiteSpace(tbCity.Text) ||
                favoriteComboBox.SelectedItem == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void clearCustomerDataFromMainWindow()
        {
            tbFirstName.Text = "";
            tbLastName.Text = "";
            tbStreet.Text = "";
            tbPostalCode.Text = "";
            tbCity.Text = "";
            favoriteComboBox.SelectedItem = null;
            companyComboBox.SelectedItem = null;
            pendingImageBase64 = "BAASD";
            userImage.Source = null;
        }

        private async void btRefresh_Click(object sender, RoutedEventArgs e)
        {
            await refresh();
        }
    }
}