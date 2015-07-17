using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planning
{
    [Serializable()]
    public class DatesHolydays
    {
        public List<DateTime> holydays = new List<DateTime>();
        public DateTime endStuding;
        public DateTime startStuding;
    }
}
