using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messe_Client.Handler
{
    static class LoginHandler
    {
        public static bool login_window_activity_status { get; set; } = false;
        public static bool signed_in { get; set; } = false;
        public static string username { get; set; } = "ERROR: Not logged in yet!";

        public static bool admin_tab_allowed { get; set; } = false;
    }
}
