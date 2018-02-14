using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model
{
    public class Property
    {
        [Key]
        public int Id { get; set; }
        [StringLength(550)]
        public string RealtorUrl { get; set; }
        public int? Beds { get; set; }
        public int? Baths { get; set; }
        [StringLength(500)]
        public string ListedAddress { get; set; }

        [StringLength(25)]
        public string RealtorListingId { get; set; }
        [StringLength(25)]
        public string RealtorPropertyId { get; set; }

        [StringLength(150)]
        public string Address { get; set; }
        [StringLength(50)]
        public string Address2 { get; set; }
        [StringLength(100)]
        public string City { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(15)]
        public string Zip { get; set; }
        public byte? PropertyTypeId { get; set; }
        public byte? PropertyStatusId { get; set; }

        public int? LotSize { get; set; }
        public int? AskingPrice { get; set; }

        [ForeignKey("PropertyStatusId")]
        public virtual PropertyStatus PropertyStatus { get; set; }
        [ForeignKey("PropertyTypeId")]
        public virtual PropertyType PropertyType { get; set; }
    }
}
