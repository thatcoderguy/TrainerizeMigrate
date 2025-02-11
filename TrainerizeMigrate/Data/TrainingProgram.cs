using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainerizeMigrate.Data
{
    public class TrainingProgram
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string durationType { get; set; }
        public string subscriptionType { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string accessLevel { get; set; }
    }
}
