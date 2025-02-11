using System.ComponentModel.DataAnnotations;

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
