using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.APIDefinitions
{
    public class WeatherAPIResponseByLoc
    {
        public string name { get; set; }
        public WeatherLocalNames local_names { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }

        public string country { get; set; }

        public string state { get; set; }

    }
}
