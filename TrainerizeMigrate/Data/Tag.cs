using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.Data
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
    }
}
