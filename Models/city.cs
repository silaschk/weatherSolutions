using System.ComponentModel.DataAnnotations;

namespace weatherSolutions.Models
{
    public class city
    {
        [Display(Name = "ID:")]
        public int Id { get; set; }

        [Display(Name = "City:")]
        public string CityName { get; set; }

        [Display(Name = "Temperature:")]
        public float Temperature { get; set; }

        [Display(Name = "Temperature Feels Like:")]
        public float Temp_FeelsLike { get; set; }

        [Display(Name = "Min Temperature")]
        public float MinTemp { get; set; }

        [Display(Name = "Max Temperature:")]
        public float MaxTemp { get; set; }

        [Display(Name = "Humidity")]
        public int Humidity { get; set; }

        [Display(Name = "Description:")]
        public string Description { get; set; }

        [Display(Name = "Latitude:")]
        public float Latitude { get; set; }

        [Display(Name = "Longitude:")]
        public float Longitude { get; set; }

        //public city City { get; set; }
    }

    public class Coord
    {
        public float Lon { get; set; }
        public float Lat { get; set; }
    }

    public class Main
    {
        public float Temp { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public float Temp_Min { get; set; }
        public float Temp_Max { get; set; }
        public float Feels_Like { get; set; }
    }

    public class Sys
    {
        public int Type { get; set; }
        public int Id { get; set; }
        public float Message { get; set; }
        public string Country { get; set; }
        public int Sunrise { get; set; }
        public int Sunshine { get; set; }

    }

    public class Weather
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

    }

    public class ind
    {
        public string date { get; set; }
            public string day { get; set; }
        public string description { get; set; }
        public int deg { get; set;}
    }



}
