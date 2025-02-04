using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.Data
{
    public class TrainingProgram
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string durationType { get; set; }
        public string subscriptionType { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string accessLevel { get; set; }
        public int new_id { get; set; }
    }
}
