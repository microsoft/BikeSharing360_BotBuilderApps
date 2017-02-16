using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerServiceApis.Models
{
    public class Rootobject
    {
        public string authenticationResultCode { get; set; }
        public string brandLogoUri { get; set; }
        public string copyright { get; set; }
        public Resourceset[] resourceSets { get; set; }
        public int statusCode { get; set; }
        public string statusDescription { get; set; }
        public string traceId { get; set; }
    }

    public class Resourceset
    {
        public int estimatedTotal { get; set; }
        public Resource[] resources { get; set; }
    }

    public class Resource
    {
        public string __type { get; set; }
        public float[] bbox { get; set; }
        public string id { get; set; }
        public string distanceUnit { get; set; }
        public string durationUnit { get; set; }
        public Routeleg[] routeLegs { get; set; }
        public string trafficCongestion { get; set; }
        public string trafficDataUsed { get; set; }
        public float travelDistance { get; set; }
        public int travelDuration { get; set; }
        public int travelDurationTraffic { get; set; }
    }

    public class Routeleg
    {
        public Actualend actualEnd { get; set; }
        public Actualstart actualStart { get; set; }
        public object[] alternateVias { get; set; }
        public int cost { get; set; }
        public string description { get; set; }
        public Itineraryitem[] itineraryItems { get; set; }
        public string routeRegion { get; set; }
        public Routesubleg[] routeSubLegs { get; set; }
        public float travelDistance { get; set; }
        public int travelDuration { get; set; }
    }

    public class Actualend
    {
        public string type { get; set; }
        public float[] coordinates { get; set; }
    }

    public class Actualstart
    {
        public string type { get; set; }
        public float[] coordinates { get; set; }
    }

    public class Itineraryitem
    {
        public string compassDirection { get; set; }
        public Detail[] details { get; set; }
        public string exit { get; set; }
        public string iconType { get; set; }
        public Instruction instruction { get; set; }
        public Maneuverpoint maneuverPoint { get; set; }
        public string sideOfStreet { get; set; }
        public string tollZone { get; set; }
        public string towardsRoadName { get; set; }
        public string transitTerminus { get; set; }
        public float travelDistance { get; set; }
        public int travelDuration { get; set; }
        public string travelMode { get; set; }
        public Warning[] warnings { get; set; }
        public Hint[] hints { get; set; }
    }

    public class Instruction
    {
        public object formattedText { get; set; }
        public string maneuverType { get; set; }
        public string text { get; set; }
    }

    public class Maneuverpoint
    {
        public string type { get; set; }
        public float[] coordinates { get; set; }
    }

    public class Detail
    {
        public int compassDegrees { get; set; }
        public int[] endPathIndices { get; set; }
        public string maneuverType { get; set; }
        public string mode { get; set; }
        public string[] names { get; set; }
        public string roadType { get; set; }
        public int[] startPathIndices { get; set; }
        public string[] locationCodes { get; set; }
    }

    public class Warning
    {
        public string origin { get; set; }
        public string severity { get; set; }
        public string text { get; set; }
        public string to { get; set; }
        public string warningType { get; set; }
    }

    public class Hint
    {
        public string hintType { get; set; }
        public string text { get; set; }
    }

    public class Routesubleg
    {
        public Endwaypoint endWaypoint { get; set; }
        public Startwaypoint startWaypoint { get; set; }
        public float travelDistance { get; set; }
        public int travelDuration { get; set; }
    }

    public class Endwaypoint
    {
        public string type { get; set; }
        public float[] coordinates { get; set; }
        public string description { get; set; }
        public bool isVia { get; set; }
        public string locationIdentifier { get; set; }
        public int routePathIndex { get; set; }
    }

    public class Startwaypoint
    {
        public string type { get; set; }
        public float[] coordinates { get; set; }
        public string description { get; set; }
        public bool isVia { get; set; }
        public string locationIdentifier { get; set; }
        public int routePathIndex { get; set; }
    }
}