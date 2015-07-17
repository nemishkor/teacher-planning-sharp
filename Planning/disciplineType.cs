using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Planning
{
    [Serializable()]
    public class DisciplineType
    {
        public string discipline;
        public string group;
        [XmlArray("Collection"), XmlArrayItem("Item")]
        public List<Employment> employments = new List<Employment>();
        public decimal allHours
        {
            get
            {
                decimal summa = 0;
                for (int i = 0; i < employments.Count; i++)
                    summa += employments[i].hoursNeedToTeach;
                return summa;
            }
        }
    }
    public class Employment
    {
        [XmlElement("Name")]
        public string type;
        [XmlAttribute("Value")] 
        public decimal hoursNeedToTeach;
        [XmlIgnore]
        public decimal[] hoursForWeek = new decimal[6];
    }
}
