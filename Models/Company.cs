using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Messe_Client.Models
{
    public class Company
    {
        public int id { get; set; }
        public string companyName { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string email { get; set; }


    }


}
