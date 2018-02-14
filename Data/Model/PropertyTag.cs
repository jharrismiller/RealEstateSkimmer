using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class PropertyTag
    {
        [Key]
        public byte Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
    }
}
