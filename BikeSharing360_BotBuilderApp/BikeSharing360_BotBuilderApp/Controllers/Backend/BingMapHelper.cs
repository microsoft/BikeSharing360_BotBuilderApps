using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BikeSharing360_BotBuilderApp.Backend
{
    public class BingMapHelper
    {

        public static async Task<string> StaticMapWithRoute(Common.GeoLocation location1, Common.GeoLocation location2)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Consts._BikeSharing360BackendApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            string parameter = string.Format(Consts.GetMapWithRouteAPI, 
                location1.latitude, location1.longitude, location1.name,
                location2.latitude, location2.longitude, location2.name);
            HttpResponseMessage response = client.GetAsync(parameter).Result;
            if (response.IsSuccessStatusCode)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                var responseString = await response.Content.ReadAsStringAsync();
                var responseElement = JsonConvert.DeserializeObject<string>(responseString, settings);
                return responseElement;
            }
            else
            {
                return "";
            }
        }

        public static async Task<string> PointToAddress(double latitude, double longitude)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Consts._BikeSharing360BackendApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            string parameter = string.Format(Consts.GetAddrAPI, latitude, longitude);
            HttpResponseMessage response = client.GetAsync(parameter).Result;
            if (response.IsSuccessStatusCode)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                var responseString = await response.Content.ReadAsStringAsync();
                var responseElement = JsonConvert.DeserializeObject<string>(responseString, settings);
                return responseElement;
            }
            else
            {
                return "";
            }
        }

        public static async Task<string> StaticMapWith1Pin(double latitude, double longitude, string name)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Consts._BikeSharing360BackendApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            string parameter = string.Format(Consts.GetMapWith1PinAPI, latitude, longitude, name);
            HttpResponseMessage response = client.GetAsync(parameter).Result;
            if (response.IsSuccessStatusCode)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                var responseString = await response.Content.ReadAsStringAsync();
                var responseElement = JsonConvert.DeserializeObject<string>(responseString, settings);
                return responseElement;
            }
            else
            {
                return "";
            }
        }

        public static async Task<int> GetETA(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Consts._BikeSharing360BackendApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            string parameter = string.Format(Consts.GetETAAPI,
                latitude1, longitude1, latitude2, longitude2);
            HttpResponseMessage response = client.GetAsync(parameter).Result;
            if (response.IsSuccessStatusCode)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                var responseString = await response.Content.ReadAsStringAsync();
                var responseElement = JsonConvert.DeserializeObject<int>(responseString, settings);
                return responseElement;
            }
            else
            {
                return -1;
            }
        }
    }
}