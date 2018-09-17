using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MOSAD1.Models
{
    [DataContract]
    class City
    {
        [DataMember]
        public string aqi { get; set; }
        public string co { get; set; }
        public string no2 { get; set; }
        public string o3 { get; set; }
        public string pm10 { get; set; }
        public string pm25 { get; set; }
        public string qlty { get; set; }
        public string so2 { get; set; }
    }
    [DataContract]
    class Aqi
    {
        [DataMember]
        public City city { get; set; }
    }
    [DataContract]
    class Update
    {
        [DataMember]
        public string loc { get; set; }
        [DataMember]
        public string utc { get; set; }
    }
    [DataContract]
    class Basic
    {
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string cnty { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string lat { get; set; }
        [DataMember]
        public string lon { get; set; }
        [DataMember]
        public Update update { get; set; }
    }
    [DataContract]
    class Astro
    {
        [DataMember]
        public string mr { get; set; }
        [DataMember]
        public string ms { get; set; }
        [DataMember]
        public string sr { get; set; }
        [DataMember]
        public string ss { get; set; }
    }
    [DataContract]
    class Cond
    {
        [DataMember]
        public string code_d { get; set; }
        [DataMember]
        public string code_n { get; set; }
        [DataMember]
        public string txt_d { get; set; }
        [DataMember]
        public string txt_n { get; set; }
    }
    [DataContract]
    class _Cond
    {
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string txt { get; set; }
    }
    [DataContract]
    class Tmp
    {
        [DataMember]
        public string max { get; set; }
        [DataMember]
        public string min { get; set; }
    }
    [DataContract]
    class Wind
    {
        [DataMember]
        public string deg { get; set; }
        [DataMember]
        public string dir { get; set; }
        [DataMember]
        public string sc { get; set; }
        [DataMember]
        public string spd { get; set; }
    }
    [DataContract]
    class Daily_forecast
    {
        [DataMember]
        public Astro astro { get; set; }
        [DataMember]
        public Cond cond { get; set; }
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string hum { get; set; }
        [DataMember]
        public string pcpn { get; set; }
        [DataMember]
        public string pop { get; set; }
        [DataMember]
        public string pres { get; set; }
        [DataMember]
        public Tmp tmp { get; set; }
        [DataMember]
        public string uv { get; set; }
        [DataMember]
        public string vis { get; set; }
        [DataMember]
        public Wind wind { get; set; }
    }

    [DataContract]
    class Hourly_forecast
    {
        [DataMember]
        public Cond cond { get; set; }
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string hum { get; set; }
        [DataMember]
        public string pop { get; set; }
        [DataMember]
        public string pres { get; set; }
        [DataMember]
        public string tmp { get; set; }
        [DataMember]
        public Wind wind { get; set; }
    }

    [DataContract]
    class Now
    {
        [DataMember]
        public _Cond cond { get; set; }
        [DataMember]
        public string fl { get; set; }
        [DataMember]
        public string hum { get; set; }
        [DataMember]
        public string pcpn { get; set; }
        [DataMember]
        public string pres { get; set; }
        [DataMember]
        public string tmp { get; set; }
        [DataMember]
        public string vis { get; set; }
        [DataMember]
        public Wind wind { get; set; }
    }
    [DataContract]
    class Air
    {
        [DataMember]
        public string brf { get; set; }
        [DataMember]
        public string txt { get; set; }
    }
    [DataContract]
    class Comf
    {
        [DataMember]
        public string brf { get; set; }
        [DataMember]
        public string txt { get; set; }
    }
    [DataContract]
    class Cw
    {
        [DataMember]
        public string brf { get; set; }
        [DataMember]
        public string txt { get; set; }
    }
    [DataContract]
    class Drsg
    {
        [DataMember]
        public string brf { get; set; }
        [DataMember]
        public string txt { get; set; }
    }
    [DataContract]
    class Flu
    {
        [DataMember]
        public string brf { get; set; }
        [DataMember]
        public string txt { get; set; }
    }
    [DataContract]
    class Sport
    {
        [DataMember]
        public string brf { get; set; }
        [DataMember]
        public string txt { get; set; }
    }
    [DataContract]
    class Trav
    {
        [DataMember]
        public string brf { get; set; }
        [DataMember]
        public string txt { get; set; }
    }
    [DataContract]
    class Uv
    {
        [DataMember]
        public string brf { get; set; }
        [DataMember]
        public string txt { get; set; }
    }
    [DataContract]
    class Suggestion
    {
        [DataMember]
        public Air air { get; set; }
        [DataMember]
        public Comf comf { get; set; }
        [DataMember]
        public Cw cw { get; set; }
        [DataMember]
        public Drsg drsg { get; set; }
        [DataMember]
        public Flu flu { get; set; }
        [DataMember]
        public Sport sport { get; set; }
        [DataMember]
        public Trav trav { get; set; }
        [DataMember]
        public Uv uv { get; set; }
    }
    [DataContract]
    class HeWeather5
    {
        [DataMember]
        public Aqi aqi { get; set; }
        [DataMember]
        public Basic basic { get; set; }
        [DataMember]
        public List<Daily_forecast> daily_forecast { get; set; }
        [DataMember]
        public List<Hourly_forecast> hourly_forecast { get; set; }
        [DataMember]
        public Now now { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public Suggestion suggestion { get; set; }
    }
    [DataContract]
    class RootObject
    {
        [DataMember]
        public List<HeWeather5> HeWeather5 { get; set; }
    }
}
