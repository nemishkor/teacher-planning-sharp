using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Planning
{
    [Serializable()]
    public class Disciplines
    {
        // only data of elements in form
        public List<DisciplineType> disciplineList = new List<DisciplineType>();
    }
}
