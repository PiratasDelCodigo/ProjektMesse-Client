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
            this.ResizeMode = ResizeMode.NoResize;
        }

        public void clearInputs()
        {
            pwb_password.Password = "";
            tb_name.Text = "";
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if(tb_name.Text == name_admin)//per API mit DB abgleichen
            {   //Name richtig, PW wird geprüft...
                if (pwb_password.Password == password_admin) //per API mit DB abgleichen
                {
                    //PW und Name richtig!
                    MessageBox.Show("Login erfolgreich!");
                    Handler.login_window_activity_status = false;
                    Handler.signed_in = true;
                    Handler.username = tb_name.Text;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Passwort falsch!");
                    //Name richtig, PW nicht!
                }
            }
            else
            {
                //Name falsch, PW wird geprüft...
                if(pwb_password.Password == password_admin)//per API mit DB abgleichen
                {
                    MessageBox.Show("Name falsch!");
                    //Name falsch, PW richtig!
                }
                else
                {
                    MessageBox.Show("Name und Passwort falsch!");
                    //Name und PW falsch!
                }

            }
        }

        private void login_window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Handler.login_window_activity_status = false;
            Handler.signed_in = false;
            Handler.username = "";
        }
    }
}
