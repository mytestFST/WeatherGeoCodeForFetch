using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Runtime.CompilerServices;
namespace Utilities.APIHelper
{
    public class APIClient
    {
        
        
        internal  HttpClient RequestClient { get; set; }

        /*--------------------------------------------------------------------------------------------------------------------------------------------
        * Function Name: InitializeClient
        * Descritpion/Purpose:  Initializes API Client Object which is used APIHelper classes
        * Input Parameters:
        *     Parameter1 - URI <String>

        * Output Parameters:
               None
        * Author: Remila Palaniappan
        * Date Created:  03/25/2024
        * Updates:
        * <Date Of Update> - <User Who Updated> - <Description of Update>
        *--------------------------------------------------------------------------------------------------------------------------------------------
        */
        //public static void InitializeClient(string uri, [CallerFilePath] string path="")
        public   APIClient(string uri, [CallerFilePath] string path = "")
        {
            RequestClient = new HttpClient();
            RequestClient.BaseAddress = new Uri(uri);
            RequestClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }



    }
}
