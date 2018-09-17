using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MOSAD1.Models;
using System.Xml;
using Windows.Data.Xml.Dom;
using System.Xml.Serialization;

namespace MOSAD1.Services
{
    class ExpressQueryService
    {
        public async static Task<root> GetExpressInfo(string company,string number)
        {
            var http = new HttpClient();
            if (number == "")
            {
                number = "guangzhou";
            }
            Debug.WriteLine(company);
            switch(company)
            {
                case "申通":company = "sto";break;
                case "圆通":company = "yt";break;
                case "顺丰":company = "sf";break;

            }
            var response = await http.GetAsync("http://v.juhe.cn/exp/index?key=491954bbcac885c7db971aa175ffe8ee&com="+company+"&no=" + number+ "&dtype=xml");
            var result = await response.Content.ReadAsStringAsync();
            
            Debug.Write(result);
            XmlSerializer se = new XmlSerializer(typeof(root));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (root)se.Deserialize(ms);
            if (data.resultcode!= "200")
            {
                throw new Exception("无信息");
            }
            return data;
        }
    }
}
