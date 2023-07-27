using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PummaApplication.Models
{
    public class CitiesAndState
    {
        public string mscz_state_city_statename { get; set; }
        public string mscz_state_city_city { get; set; }
        public string zipcd { get; set; }
        public List<CitiesAndState> CitiesAndStateList { get; internal set; }
        public CityStateResult result { get; set; }
        public List<CitiesAndState> CityStateResult { get; internal set; }
    }
    public class CityStateResult
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
}