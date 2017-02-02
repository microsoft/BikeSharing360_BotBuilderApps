using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using BikeSharing360_BotBuilderApp.Common;
using System.Text;

namespace BikeSharing360_BotBuilderApp.Backend
{
    public enum IncidentType
    {
        lost,
        flattire
    };

    public class CustomerService
    {
        public static async Task<Employee> GetAvailableEmployee()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Consts._BikeSharing360BackendApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("getavailableemployee").Result;
            if (response.IsSuccessStatusCode)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                var responseString = await response.Content.ReadAsStringAsync();
                var responseElement = JsonConvert.DeserializeObject<Employee>(responseString, settings);
                responseElement.id = responseElement.id.Trim();
                responseElement.name = responseElement.name.Trim();
                responseElement.serviceUrl = responseElement.serviceUrl.Trim();
                responseElement.conversationId = responseElement.conversationId.Trim();
                return responseElement;
            }
            return null;
        }
        public static async Task<string> FileCase(string userid, IncidentType incidenttype, double lat, double lon)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Consts._BikeSharing360BackendApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            string ctype = "lost";
            switch (incidenttype)
            {
                case IncidentType.flattire:
                    ctype = "flattire";
                    break;
                case IncidentType.lost:
                    ctype = "lost";
                    break;
            }
            string parameter = string.Format(Consts.FileCaseAPI, userid, ctype, lat, lon);
            HttpResponseMessage response = client.GetAsync(parameter).Result;
            if (response.IsSuccessStatusCode)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                String responseString = await response.Content.ReadAsStringAsync();
                var responseElement = JsonConvert.DeserializeObject<string>(responseString, settings);
                return responseElement;
            }
            else
            {
                return "83723";
            }
        }

        public static async Task<Employee> LoadInformation(string employeeId)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Consts._BikeSharing360BackendApiBaseAddress);

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("getinformation?id=" + employeeId).Result;
            if (response.IsSuccessStatusCode)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                var responseString = await response.Content.ReadAsStringAsync();
                var responseElement = JsonConvert.DeserializeObject<Employee>(responseString, settings);
                responseElement.id = responseElement.id.Trim();
                responseElement.name = responseElement.name.Trim();
                responseElement.serviceUrl = responseElement.serviceUrl.Trim();
                responseElement.customer.id = responseElement.customer.id.Trim();
                responseElement.customer.name = responseElement.customer.name.Trim();
                responseElement.customer.serviceUrl = responseElement.customer.serviceUrl.Trim();
                return responseElement;
            }
            else
                return null;
        }

        public static async Task<bool> SaveInformation(Employee employee)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Consts._BikeSharing360BackendApiBaseAddress);

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            
            StringContent content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("saveinformation", content);
            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}