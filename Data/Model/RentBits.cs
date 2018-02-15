using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model
{
    public class RentBits
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(10)]
        public string Zip { get; set; }
        [StringLength(70)]
        public string Location { get; set; }
        [StringLength(10)]
        public string Type { get; set; }
        public int OneBedRoom { get; set; }
        public int TwoBedRoom { get; set; }
        public int ThreeOrMoreBedRoom { get; set; }
        public string Url { get; set; }
    }
}
