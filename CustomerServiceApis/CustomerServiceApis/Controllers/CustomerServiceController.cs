using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using CustomerServiceApis.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Xml;

namespace CustomerServiceApis.Controllers
{
    public class CustomerServiceController : ApiController
    {
        private const string _bingmapappid = "_YourBingMapId_";
        private const string _bingmapstaticmapwithrouteurl = "http://dev.virtualearth.net/REST/v1/Imagery/Map/Road/{4}%2C{5}/15/Routes/Walking?waypoint.1={2},{3};64;{6}&waypoint.2={0},{1};64;{7}&mapSize=400,300&format=png&key=" + _bingmapappid;
        private const string _bingmapstaticmapwith1pointsurl = "http://dev.virtualearth.net/REST/V1/Imagery/Map/Road/{0}%2C{1}/13?mapSize=400,300&format=png&pushpin={0},{1};64;{2}&key=" + _bingmapappid;
        private const string _bingmappointtoaddrurl = "http://dev.virtualearth.net/REST/v1/Locations/{0},{1}?o=xml&key=" + _bingmapappid;

        [Route("getavailableemployee")]
        [HttpGet]
        public Employee GetAvailableEmployee()
        {
            //get available employee from database
            return null;
        }

        [Route("getaddress")]
        [HttpGet]
        public string GetAddress(double latitude, double longitude)
        {
            string requesturl = string.Format(_bingmappointtoaddrurl, latitude.ToString(), longitude.ToString());
            XmlDocument response = GetXmlResponse(requesturl);
            if (response == null)
            {
                return "";
            }
            var addresses = response.GetElementsByTagName("FormattedAddress");
            string ret = "";
            if (addresses.Count > 0)
            {
                ret = addresses[0].InnerText.ToString();
            }
            return ret;
        }

        [Route("getinformation")]
        [HttpGet]
        public Employee GetInformation(string id)
        {
            //load employee information from database
            return null;
        }

        [Route("register")]
        [HttpPost]
        public void Register([FromBody]Employee value)
        {
            //register employee into database
        }

        [Route("saveinformation")]
        [HttpPost]
        public void SaveInformation([FromBody]Employee value)
        {
            //save employee information into database
        }

        [Route("filecase")]
        [HttpGet]
        public string FileCase(string userid, string incidenttype, double lat, double lon)
        {
            //add record to database
            return "12345";
        }

        [Route("locatebike")]
        [HttpGet]
        public GeoLocation LocateBike(string bikeid, string datetime = "")
        {
            GeoLocation ret = new GeoLocation();
            if (datetime == "")
            {
                ret.latitude = 40.722567290067673;
                ret.longitude = -73.9976117759943;
                ret.name = "Chipotle Mexican Grill";
            }
            else
            {
                ret.latitude = 40.720725953578949;
                ret.longitude = -74.005964174866676;
                ret.name = "Spring Studios";
            }
            return ret;
        }

        [Route("getmapwithroute")]
        [HttpGet]
        public string GetMapWithRoute(double latitude1, 
            double longitude1, string name1, double latitude2,
            double longitude2, string name2)
        {
            double midlat = (latitude1 + latitude2) / 2;
            double midlon = (longitude1 + longitude2) / 2;
            return string.Format(_bingmapstaticmapwithrouteurl,
                       latitude1.ToString(), longitude1.ToString(),
                       latitude2.ToString(), longitude2.ToString(),
                       midlat.ToString(), midlon.ToString(), name1.ToLower(),
                       name2.ToLower());
        }

        [Route("getmapwith1pin")]
        [HttpGet]
        public string GetMapWith1Pin(double latitude,
            double longitude, string name)
        {
            return string.Format(_bingmapstaticmapwith1pointsurl,
                       latitude.ToString(), longitude.ToString(), name);
        }

        [Route("geteta")]
        [HttpGet]
        public async Task<int> GetETA(double latitude1, double longitude1, 
            double latitude2, double longitude2)
        {
            return 15;
        }

        private static XmlDocument GetXmlResponse(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return (xmlDoc);

            }
            catch (Exception e)
            {
                var roo = e.Message;
                return null;
            }
        }
    }
}
