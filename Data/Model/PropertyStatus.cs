using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class PropertyStatus
    {
        [Key]
        public byte Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }

    }
}
