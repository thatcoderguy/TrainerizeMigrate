using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.Data
{
    public class BodyWeight
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string unit { get; set; }
        public string? goal { get; set; }
        public virtual List<WeightPoint> points { get; set; }
    }
}
