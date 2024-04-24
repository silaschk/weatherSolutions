using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;


namespace weatherSolutions.Models
{
    public class Service : IService
    {
        city IService.GetForecast(string city)
        {
            string IDOWeather = API.OpenWeatherAPI;
            var client = new RestClient($"http://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&APPID={IDOWeather}");
            ///RestRequest request = new RestRequest(Method.Get);
            RestRequest request = new RestRequest($"http://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&APPID={IDOWeather}", RestSharp.Method.Get);
            RestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                // Deserialize the string content into JToken object
                var content = JsonConvert.DeserializeObject<JToken>(response.Content);

                // Deserialize the JToken object into our WeatherResponse Class
                return content.ToObject<city>();
            }

            return null;
        }
    }
}
