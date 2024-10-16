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
    /// Interaktionslogik für loginwindow.xaml
    /// </summary>
    public partial class loginwindow : Window
    {
        //
        private string name_admin = "admin";
        private string password_admin = "admin";

        public loginwindow()
        {
            InitializeComponent();
            Handler.login_window_activity_status = false;
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if(tb_name.Text == name_admin)
            {
                if (pwb_password.Password == password_admin)
                {
                    //pw and nam richtig!
                    MessageBox.Show("Login erfolgreich!");
                    Handler.login_window_activity_status = false;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Passwort falsch!");
                    //Name richtig, passwort nicht
                }
            }
            else
            {
                //name falsch, passwort wird geprüft
                if(pwb_password.Password == password_admin)
                {
                    MessageBox.Show("Name falsch!");
                    //name falsch, passwort richtig
                }
                else
                {
                    MessageBox.Show("Name und Passwort falsch!");
                    //name und passwort falsch
                }

            }
        }
    }
}
