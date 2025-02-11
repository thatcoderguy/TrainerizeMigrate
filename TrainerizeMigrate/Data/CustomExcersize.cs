using System.ComponentModel.DataAnnotations;

namespace TrainerizeMigrate.Data
{
    public class CustomExcersize
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string alternateName { get; set; }
        public string description { get; set; }
        public string recordType { get; set; }
        public string videoType { get; set; }
        public string videoUrl { get; set; }
        public virtual List<Tag> tags { get; set; }
        public int? new_id {  get; set; }
    }
}
