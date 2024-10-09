using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messe_Client
{
    internal class Handler
    {
        public bool login_window_activity_status { get; set; }

        public Handler()
        {
            login_window_activity_status = false;
        }
    }
}
