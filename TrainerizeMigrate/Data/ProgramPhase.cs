using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.Data
{
    public class ProgramPhase
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string instruction { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int? duration { get; set; }
        public string durationType { get; set; }
        public string planType { get; set; }
        public string modified { get; set; }
        public int? new_id { get; set; }
    }
}
