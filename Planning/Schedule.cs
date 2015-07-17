using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Planning
{
    [Serializable()]
    public class Schedule
    {
        [XmlElement("B")]
        public List<string> allSubjects = new List<string>();
    }
}
