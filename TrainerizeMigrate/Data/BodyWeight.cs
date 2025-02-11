using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrainerizeMigrate.Data
{
    public class BodyWeight
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string? unit { get; set; }
        public string? goal { get; set; }
        public virtual List<WeightPoint>? points { get; set; }
    }
}
