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
        public Window2()
        {
            InitializeComponent();
        }

        public Window2(Company[] data)
        {
            InitializeComponent();
            dgData.ItemsSource = data;
        }

        public Window2(Customer[] data)
        {
            InitializeComponent();
            dgData.ItemsSource = data;
        }



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
