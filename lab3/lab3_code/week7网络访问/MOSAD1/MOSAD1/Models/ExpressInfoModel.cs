using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Data.Xml.Dom;

namespace MOSAD1.Models
{
    [XmlRoot("root")]
    public class root
    {
        [XmlElement("resultcode")]
        public string resultcode;
        [XmlElement("reason")]
        public string reason;
        [XmlElement("result")]
        public ExpressInfoModel result;
    }
    public class ExpressInfoModel
    {
        [XmlElement("company")]
        public string company;
        [XmlElement("no")]
        public string number;
        [XmlArray("list"), XmlArrayItem("item")]
        public List<ExpressItem> infos;
    }
    [XmlRoot("item")]
    public class ExpressItem
    {
        [XmlElement("datetime")]
        public string datetime;
        [XmlElement("remark")]
        public string remark;
        [XmlElement("zone")]
        public string zone;
    }
}
