using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BikeSharing360_BotBuilderApp.Backend
{
    public class Bike
    {
        public string bikeid;

        public static async Task<List<Bike>> LookupBikesWithUser(string userid)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Consts._BikeSharing360BackendApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            string parameter = string.Format(Consts.LookupBikesWithUserAPI, userid);
            HttpResponseMessage response = client.GetAsync(parameter).Result;
            if (response.IsSuccessStatusCode)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                String responseString = await response.Content.ReadAsStringAsync();
                var responseElement = JsonConvert.DeserializeObject<List<Bike>>(responseString, settings);
                return responseElement;
            }
            return null;
        }

        public static async Task<Common.GeoLocation> LocateBike(string bikeid, DateTime time)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Consts._BikeSharing360BackendApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            string parameter = string.Format(Consts.LocateBikebyTimeAPI, bikeid, time.ToString());
            HttpResponseMessage response = client.GetAsync(parameter).Result;
            if (response.IsSuccessStatusCode)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                String responseString = await response.Content.ReadAsStringAsync();
                var responseElement = JsonConvert.DeserializeObject<Common.GeoLocation>(responseString, settings);
                return responseElement;
            }
            return null;
        }

        public static async Task<Common.GeoLocation> LocateBike(string bikeid)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Consts._BikeSharing360BackendApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            string parameter = string.Format(Consts.LocateBikeAPI, bikeid);
            HttpResponseMessage response = client.GetAsync(parameter).Result;
            if (response.IsSuccessStatusCode)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                String responseString = await response.Content.ReadAsStringAsync();
                var responseElement = JsonConvert.DeserializeObject<Common.GeoLocation>(responseString, settings);
                return responseElement;
            }
            return null;
        }
    }
}