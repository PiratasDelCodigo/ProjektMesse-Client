using Messe_Client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messe_Client
{
    internal static class JsonHandler
    {
        public static dynamic? jsonData { get; set; }

        public static (Company[]?, DateTime) getCompanyFromJSON()
        {
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            jsonData["currentTimestamp"] = DateTime.UtcNow;
            File.WriteAllText("data.json", jsonData.ToString());
            var companies = jsonData!["companies"].ToObject<Company[]>();
            var timestamp = jsonData["currentTimestamp"].ToObject<DateTime>();
            return (companies, timestamp);
        }

        public static (Customer[]?, DateTime) getCustomersFromJSON()
        {
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            jsonData["currentTimestamp"] = DateTime.UtcNow;
            File.WriteAllText("data.json", jsonData.ToString());
            var customers = jsonData!["customers"].ToObject<Customer[]>();
            var timestamp = jsonData["currentTimestamp"].ToObject<DateTime>();
            return (customers, timestamp);
        }

        public static (ProductGroup[]?, DateTime) getProductGroupsFromJSON()
        {
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            jsonData["currentTimestamp"] = DateTime.UtcNow;
            File.WriteAllText("data.json", jsonData.ToString());
            var productGroups = jsonData!["productGroups"].ToObject<ProductGroup[]>();
            var timestamp = jsonData["currentTimestamp"].ToObject<DateTime>();
            return (productGroups, timestamp);
        }

        public static void setCompaniesToData(string companyJSON)
        {
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            dynamic companies = Newtonsoft.Json.JsonConvert.DeserializeObject(companyJSON);
            jsonData["companies"] = companies;
            jsonData["currentTimestamp"] = DateTime.UtcNow;
            File.WriteAllText("data.json", jsonData.ToString());
        }

        public static void setCustomersToData(string companyJSON)
        {
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            dynamic companies = Newtonsoft.Json.JsonConvert.DeserializeObject(companyJSON);
            jsonData["customers"] = companies;
            jsonData["currentTimestamp"] = DateTime.UtcNow;
            File.WriteAllText("data.json", jsonData.ToString());
        }

        public static void setProductGroupToData(string companyJSON)
        {
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            dynamic companies = Newtonsoft.Json.JsonConvert.DeserializeObject(companyJSON);
            jsonData["productGroups"] = companies;
            jsonData["currentTimestamp"] = DateTime.UtcNow;
            File.WriteAllText("data.json", jsonData.ToString());
        }

    }

}
