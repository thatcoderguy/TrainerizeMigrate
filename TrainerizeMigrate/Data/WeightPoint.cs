using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.Data
{
    public class WeightPoint
    {
        [Key]
        public int id { get; set; }
        public string date { get; set; }
        public double value { get; set; }
        public bool imported { get; set; } = false;
    }
}
