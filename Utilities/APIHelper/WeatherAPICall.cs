using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Utilities.APIDefinitions;
using NLog;
//using APILayer.LogHelper;
using Utilities.APIHelper;

namespace Utilities.APIHelper
{
    public static class WeatherAPICall
    {
        

       static  Logger log = LogManager.GetCurrentClassLogger();
        static APIClient _WeatherAPIClient;
        
        public static async Task <WeatherAPIResponseByLoc[]> GetWeatherByLocationName(string q, string apiKey, string limit,  string uri)
        {
            

             _WeatherAPIClient = new APIClient(uri);

            //Set up Active Batch POST Request Body
            //var WeatherAPIParameters = new WeatherAPI
            //{
            //    appid=apiKey,
            //    q=q,
            //    limit=limit
            // };

            ////Convert to JSON
            //var jsonData = JsonConvert.SerializeObject(WeatherAPIParameters);
            //var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");

            //Log into Active Batch via API
            HttpResponseMessage APIResponse = await _WeatherAPIClient.RequestClient.GetAsync(_WeatherAPIClient.RequestClient.BaseAddress+ "direct?q="+q+"&limit="+limit+"&appid="+apiKey);

            //Check to see if the response was successful
            if (APIResponse.IsSuccessStatusCode)
            {
                //Get the response body
                var responseString = await APIResponse.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<WeatherAPIResponseByLoc[]>(responseString);
                
                

                return result;
            }
            else
            {
                return null;
            }
        }


        public static async Task<WeatherAPIResponseByZip> GetWeatherByZipCode(string zipCode, string apiKey, string uri)
        {


            _WeatherAPIClient = new APIClient(uri);

            //Set up Active Batch POST Request Body
           

            //Convert to JSON
            //var jsonData = JsonConvert.SerializeObject(WeatherAPIParameters);
            //var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");

            //Log into Active Batch via API
            HttpResponseMessage APIResponse = await _WeatherAPIClient.RequestClient.GetAsync(_WeatherAPIClient.RequestClient.BaseAddress+"zip?zip=" + zipCode + "&appid=" + apiKey);

            //Check to see if the response was successful
            if (APIResponse.IsSuccessStatusCode)
            {
                //Get the response body
                var responseString = await APIResponse.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<WeatherAPIResponseByZip>(responseString);



                return result;
            }
            else
            {
                return null;
            }
        }

    }
}