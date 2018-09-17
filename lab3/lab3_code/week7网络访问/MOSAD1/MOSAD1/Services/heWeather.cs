using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using MOSAD1.Models;
namespace MOSAD1.Services
{
    class WeatherService
    {
        public async static Task<RootObject> GetWeather(string cityName)
        {
            var http = new HttpClient();
            if (cityName == "")
            {
                cityName = "guangzhou";
            }
            var response = await http.GetAsync("https://free-api.heweather.com/v5/weather?city=" + cityName + "&key=0244c885f15d47da91eec9c7985c073a");
            var result = await response.Content.ReadAsStringAsync();
            if(result.Contains("unknown city"))
            {
                throw  new Exception("No Such City");
            }
            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            Debug.Write(result);
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(ms);
            return data;
        }
    }

}
