using Messe_Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messe_Client.Handler
{
    internal static class JsonHandler
    {
        public static dynamic? jsonData { get; set; }

        public static (Company[]?, DateTime) getCompanyFromJSON()
        {
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            File.WriteAllText("data.json", jsonData.ToString());
            var companies = jsonData!["companies"].ToObject<Company[]>();
            var timestamp = jsonData["currentTimestamp"].ToObject<DateTime>();
            return (companies, timestamp);
        }

        public static (Customer[]?, DateTime) getCustomersFromJSON()
        {
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            File.WriteAllText("data.json", jsonData.ToString());
            var customers = jsonData!["customers"].ToObject<Customer[]>();
            var timestamp = jsonData["currentTimestamp"].ToObject<DateTime>();
            return (customers, timestamp);
        }

        public static (ProductGroup[]?, DateTime) getProductGroupsFromJSON()
        {
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
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

        public static DateTime getTimeStamp()
        {
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            return jsonData["currentTimestamp"];
        }

        public static int setPendingCustomers(Customer customer)
        {
            string json = File.ReadAllText("data.json");
            int pendingItems = 0;
            JToken jsonCustomer = JToken.FromObject(customer);
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            if (jsonData["pendingCustomers"] != null)
            {
                jsonData["pendingCustomers"].Add(jsonCustomer);
                pendingItems += jsonData["pendingCustomers"].Count;
            }
            else
            {
                jsonData["pendingCustomers"] = new Newtonsoft.Json.Linq.JArray();
                jsonData["pendingCustomers"].Add(jsonCustomer);
            }
            File.WriteAllText("data.json", jsonData.ToString());
            return pendingItems;
        }

        public static Customer[]? getPendingCustomers()
        {
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            if (jsonData["pendingCustomers"] != null)
            {
                return jsonData["pendingCustomers"].ToObject<Customer[]>();
            }
            return null;
        }   

        public static async Task<(int,int)> sendPendingData()
        {
            Customer[]? pendingCustomers = getPendingCustomers();
            var httpService = new HttpService();
            List<Customer> successCustomers = new List<Customer>();


            foreach (var customer in pendingCustomers)
            {
                string json = JsonConvert.SerializeObject(customer);
                HttpResponseMessage response = await httpService.PostAsync("https://localhost:7049/api/Customer", json);
                if (response.IsSuccessStatusCode)
                {
                    successCustomers.Add(customer);
                }
            }   
           

            if (successCustomers.Count > 0)
            {
                string json = File.ReadAllText("data.json");
                jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                // Remove all successful customers from pendingCustomers
                jsonData["pendingCustomers"] = new JArray(
                    ((JArray)jsonData["pendingCustomers"])
                        .Where(c => !successCustomers.Any(sc =>
                            JsonConvert.DeserializeObject<Customer>(c.ToString()).FirstName == sc.FirstName && JsonConvert.DeserializeObject<Customer>(c.ToString()).LastName == sc.LastName && JsonConvert.DeserializeObject<Customer>(c.ToString()).Image == sc.Image))
                );

                File.WriteAllText("data.json", jsonData.ToString());

            }
            return (pendingCustomers.Length - successCustomers.Count,successCustomers.Count);
        }

    }

}
