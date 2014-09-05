using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;

namespace NetGain.GoogleMap
{

    public static class GoogleAddressAPIClient
    {
        public static Location GetLocationByAddress(Address addr)
        {
            var url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + addr.StreetAddress + ",+" + addr.City + ",+" + addr.State + ",+" + addr.Country + ",+" + addr.Zipcode + "&sensor=false";
            // Synchronous Consumption
            var syncClient = new WebClient();
            var content = syncClient.DownloadString(url);

            // Create the Json serializer and parse the response
            var addressData = new GoogleAddressAPIResult();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GoogleAddressAPIResult));
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(content)))
            {
                addressData = (GoogleAddressAPIResult)serializer.ReadObject(ms);
            }

            return addressData.results[0].geometry.location;
        }
    }

}
