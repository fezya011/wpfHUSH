using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfHUSH.Model
{
    public class Report
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Reason Reason { get; set; }
        public int ReasonId { get; set; }
    }
}
