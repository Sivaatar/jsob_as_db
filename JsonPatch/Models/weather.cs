using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JsonPatch.Models
{
    public class weather
    {
        [Key]
        public virtual string City { get; set; }

        public virtual string Temprature { get; set; }

        public virtual string Humidity { get; set; }

        public virtual string Poss_of_rain { get; set; }

        public virtual string Wind_level { get; set; }

        public weather(string city, string temp, string hum, string rain,string wind)
        {
            City = city;
            Temprature = temp;
            Humidity = hum;
            Poss_of_rain = rain;
            Wind_level = wind;
        }

    }
}
