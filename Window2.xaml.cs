using Messe_Client.Models;
using Newtonsoft.Json;
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

namespace Messe_Client
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public bool initialized = false;

        public Window2()
        {
            InitializeComponent();
        }

        public Window2(Company[] data)
        {
            InitializeComponent();
            dgData.ItemsSource = data;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dgData.ItemsSource);
            view.Filter = FilterCompanies; // Define the filter method
            initialized = true;
        }

        public Window2(Customer[] data)
        {
            InitializeComponent();
            dgData.ItemsSource = data;
        }
        public Window2(ProductGroup[] data)
        {
            InitializeComponent();
            dgData.ItemsSource = data;
        }



        private void searchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (initialized)
            {
                CollectionViewSource.GetDefaultView(dgData.ItemsSource).Refresh();
            }
        }

        private bool FilterCompanies(object item)
        {
            if (string.IsNullOrEmpty(tbSearchField.Text))
                return true; // If no filter text, show all companies

            var company = item as Company;
            string filterText = tbSearchField.Text.ToLower();

            // Modify this to filter on any field of your Company class
            return company.city.ToLower().Contains(filterText) ||
                   company.companyName.ToLower().Contains(filterText) ||
                   company.street.ToLower().Contains(filterText) ||
                   company.postalCode.ToLower().Contains(filterText) ||
                   company.email.ToLower().Contains(filterText); // Example of filtering by name or location
        }

        private bool FilterCustomers(object item)
        {
            if (string.IsNullOrEmpty(tbSearchField.Text))
                return true; // If no filter text, show all companies

            var customer = item as Customer;
            string filterText = tbSearchField.Text.ToLower();

            // Modify this to filter on any field of your Company class
            return customer.FirstName.ToLower().Contains(filterText) ||
                   customer.LastName.ToLower().Contains(filterText) ||
                   customer.Street.ToLower().Contains(filterText) ||
                   customer.PostalCode.ToLower().Contains(filterText) ||
                   customer.City.ToLower().Contains(filterText); // Example of filtering by name or location
        }

        private bool FilterProductGroups(object item)
        {
            if (string.IsNullOrEmpty(tbSearchField.Text))
                return true; // If no filter text, show all companies

            var productGroup = item as ProductGroup;
            string filterText = tbSearchField.Text.ToLower();

            // Modify this to filter on any field of your Company class
            return productGroup.groupName.ToLower().Contains(filterText); // Example of filtering by name or location
        }
    }
}
