using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.APIHelper;

namespace geocodeUtility
{
    class GeoCodes
    {
        static async Task Main(string[] args)
        {
            List<string> results = new List<string>();
            try
            {
                List<string> locations = new List<string>();
                List<string> zipcodes = new List<string>();
                
                
                

                if (args.Length == 0 )
                {
                    throw new Exception("Please provide arguments");
                }
                string apikey = "f897a99d971b5eef57be6fafa0d83239";
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].Contains(","))
                    {
                        locations.Add(args[i]);
                    }
                    else if (!args[i].Contains(","))
                    {
                        zipcodes.Add(args[i]);
                    }
                }

                foreach (var item in locations)
                {
                    string locationName;
                    if (item.Contains(",US"))
                    {
                        locationName = item;
                    }
                    else
                    {
                        locationName = item + ",US";
                    }


                    var WeatherResponse = await WeatherAPICall.GetWeatherByLocationName(locationName, apikey, "5", "http://api.openweathermap.org/geo/1.0/");

                    if (WeatherResponse == null)
                    {
                        results.Add("No data returned" );
                    }
                    else
                    {

                        
                        results.Add( WeatherResponse[0].name + ", " + WeatherResponse[0].state + ", " + WeatherResponse[0].country + ", Latitude : " + WeatherResponse[0].lat + ", Longitude :" + WeatherResponse[0].lon);
                    }
                }


                foreach (var item in zipcodes)
                {
                    string zipcode=item;
                    


                    var WeatherResponse = await WeatherAPICall.GetWeatherByZipCode(zipcode, apikey, "http://api.openweathermap.org/geo/1.0/");

                    if (WeatherResponse == null)
                    {
                        results.Add("No data returned" );
                    }
                    else
                    {


                        results.Add(WeatherResponse.zip + ", " + WeatherResponse.name +  ", " + WeatherResponse.country + ", Latitude : " + WeatherResponse.lat + ", Longitude :" + WeatherResponse.lon);
                    }
                }


                results.Sort();

                foreach (var item in results)
                {
                    Console.WriteLine(item);
                }


                
            }
            catch (Exception e)
            {
                results.Add(e.Message);
                Console.WriteLine(e.Message);
            }
        }
    }

}

