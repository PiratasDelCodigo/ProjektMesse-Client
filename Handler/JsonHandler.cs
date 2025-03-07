﻿using Messe_Client.Models;
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

        public static (Company[]?, DateTime?) getCompanyFromJSON()
        {
            if (!File.Exists("data.json"))
            {
                File.WriteAllText("data.json", "{\"companies\":[],\"customers\":[],\"productGroups\":[],\"pendingCustomers\":[],\"currentTimestamp\":null}");
            }
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            File.WriteAllText("data.json", jsonData.ToString());
            var companies = jsonData!["companies"].ToObject<Company[]>();
            DateTime? timestamp = null;
            try
            {
                timestamp = jsonData["currentTimestamp"].ToObject<DateTime>();
            }
            catch (Exception ex)
            {
            }
            return (companies, timestamp);
        }

        public static (Customer[]?, DateTime?) getCustomersFromJSON()
        {
            if (!File.Exists("data.json"))
            {
                File.WriteAllText("data.json", "{\"companies\":[],\"customers\":[],\"productGroups\":[],\"pendingCustomers\":[],\"currentTimestamp\":null}");
            }
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            File.WriteAllText("data.json", jsonData.ToString());
            var customers = jsonData!["customers"].ToObject<Customer[]>();
            DateTime? timestamp = null;
            try
            {
                timestamp = jsonData["currentTimestamp"].ToObject<DateTime>();
            }
            catch (Exception ex)
            {
            }
            return (customers, timestamp);
        }

        public static (ProductGroup[]?, DateTime?) getProductGroupsFromJSON()
        {
            if (!File.Exists("data.json"))
            {
                File.WriteAllText("data.json", "{\"companies\":[],\"customers\":[],\"productGroups\":[],\"pendingCustomers\":[],\"currentTimestamp\":null}");
            }
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            File.WriteAllText("data.json", jsonData.ToString());
            var productGroups = jsonData!["productGroups"].ToObject<ProductGroup[]>();
            DateTime? timestamp = null;
            try
            {
                timestamp = jsonData["currentTimestamp"].ToObject<DateTime>();
            }
            catch (Exception ex)
            {
            }
            return (productGroups, timestamp);
        }

        public static void setCompaniesToData(string companyJSON)
        {
            if (!File.Exists("data.json"))
            {
                File.WriteAllText("data.json", "{\"companies\":[],\"customers\":[],\"productGroups\":[],\"pendingCustomers\":[],\"currentTimestamp\":null}");
            }
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            dynamic companies = Newtonsoft.Json.JsonConvert.DeserializeObject(companyJSON);
            if(jsonData["companies"] == null)
            {
                jsonData["companies"] = new Newtonsoft.Json.Linq.JArray();
            }
            jsonData["companies"] = companies;
            jsonData["currentTimestamp"] = DateTime.UtcNow;
            File.WriteAllText("data.json", jsonData.ToString());
        }

        public static void setCustomersToData(string customersJSON)
        {
            if (!File.Exists("data.json"))
            {
                File.WriteAllText("data.json", "{\"companies\":[],\"customers\":[],\"productGroups\":[],\"pendingCustomers\":[],\"currentTimestamp\":null}");
            }
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            dynamic customers = Newtonsoft.Json.JsonConvert.DeserializeObject(customersJSON);
            if (jsonData["customers"] == null)
            {
                jsonData["customers"] = new Newtonsoft.Json.Linq.JArray();
            }
            jsonData["customers"] = customers;
            jsonData["currentTimestamp"] = DateTime.UtcNow;
            File.WriteAllText("data.json", jsonData.ToString());
        }

        public static void setProductGroupToData(string productGroupsJSON)
        {
            if (!File.Exists("data.json"))
            {
                File.WriteAllText("data.json", "{\"companies\":[],\"customers\":[],\"productGroups\":[],\"pendingCustomers\":[],\"currentTimestamp\":null}");
            }
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            dynamic productGroups = Newtonsoft.Json.JsonConvert.DeserializeObject(productGroupsJSON);
            if (jsonData["productGroups"] == null)
            {
                jsonData["productGroups"] = new Newtonsoft.Json.Linq.JArray();
            }
            jsonData["productGroups"] = productGroups;
            jsonData["currentTimestamp"] = DateTime.UtcNow;
            File.WriteAllText("data.json", jsonData.ToString());
        }

        public static DateTime? getTimeStamp()
        {
            if (!File.Exists("data.json"))
            {
                File.WriteAllText("data.json", "{\"companies\":[],\"customers\":[],\"productGroups\":[],\"pendingCustomers\":[],\"currentTimestamp\":null}");
                return null;
            }
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            if (jsonData != null && jsonData["currentTimestamp"] != null)
            {
                return jsonData["currentTimestamp"].ToObject<DateTime>();
            }
            else
            {
                return null;
            }
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
            if (!File.Exists("data.json"))
            {
                File.WriteAllText("data.json", "{\"companies\":[],\"customers\":[],\"productGroups\":[],\"pendingCustomers\":[],\"currentTimestamp\":null}");
                return new Customer[0];
            }
            string json = File.ReadAllText("data.json");
            jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            if (jsonData != null && jsonData["pendingCustomers"] != null)
            {
                return jsonData["pendingCustomers"].ToObject<Customer[]>();
            }
            return new Customer[0];
        }   

        public static async Task<(int,int)> sendPendingData()
        {
            Customer[]? pendingCustomers = getPendingCustomers();
            var httpService = new HttpService();
            List<Customer> successCustomers = new List<Customer>();


            foreach (var customer in pendingCustomers != null ? pendingCustomers : [])
            {
                if (customer.PendingCompany != null)
                {
                    // Handling when there needs a company to be created
                    string companyJson = JsonConvert.SerializeObject(customer.PendingCompany);
                    var companyResponse = await httpService.PostAsync<Company>("https://localhost:7049/api/Company", companyJson);
                    if (companyResponse.HttpResponse.IsSuccessStatusCode)
                    {
                        var companyID = companyResponse.Data.id;
                        var customerToPost = new Customer()
                        {
                            FirstName = customer.FirstName,
                            LastName = customer.LastName,
                            City = customer.City,
                            FavoriteId = customer.FavoriteId,
                            PostalCode = customer.PostalCode,
                            Street = customer.Street,
                            Image = customer.Image,
                            CompanyId = companyID
   
                        };
                        string customerJson = JsonConvert.SerializeObject(customerToPost);
                        var customerResponse = await httpService.PostAsync<Customer>("https://localhost:7049/api/Customer", customerJson);
                        if (customerResponse.HttpResponse.IsSuccessStatusCode)
                        {
                            successCustomers.Add(customer);
                        }
                    }
                }
                else
                {
                    // Handling when there is no pending company
                    string json = JsonConvert.SerializeObject(customer);
                    var response = await httpService.PostAsync<Customer>("https://localhost:7049/api/Customer", json);
                    if (response.HttpResponse.IsSuccessStatusCode)
                    {
                        successCustomers.Add(customer);
                    }
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
