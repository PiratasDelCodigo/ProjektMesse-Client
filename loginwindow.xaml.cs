using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Messe_Client.Handler;

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
        private MainWindow mw;

        public loginwindow(MainWindow mw)
        {
            InitializeComponent();
            Handler.LoginHandler.login_window_activity_status = false;
            this.ResizeMode = ResizeMode.NoResize;
            this.mw = mw;
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
                    Handler.LoginHandler.login_window_activity_status = false;
                    Handler.LoginHandler.signed_in = true;
                    Handler.LoginHandler.username = tb_name.Text;
                    this.Hide();
                    Handler.LoginHandler.admin_tab_allowed = true;
                    mw.Admin_Tab_Controll();
                }
                else
                {
                    MessageBox.Show("Passwort falsch!");
                    Handler.LoginHandler.admin_tab_allowed = false;
                    mw.Admin_Tab_Controll();
                    //Name richtig, PW nicht!
                }
            }
            else
            {
                //Name falsch, PW wird geprüft...
                if(pwb_password.Password == password_admin)//per API mit DB abgleichen
                {
                    MessageBox.Show("Name falsch!");
                    Handler.LoginHandler.admin_tab_allowed = false;
                    mw.Admin_Tab_Controll();
                    //Name falsch, PW richtig!
                }
                else
                {
                    MessageBox.Show("Name und Passwort falsch!");
                    Handler.LoginHandler.admin_tab_allowed = false;
                    mw.Admin_Tab_Controll();
                    //Name und PW falsch!
                }

            }
        }

        private void login_window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Handler.LoginHandler.login_window_activity_status = false;
            Handler.LoginHandler.signed_in = false;
            Handler.LoginHandler.username = "";
        }
    }
}
