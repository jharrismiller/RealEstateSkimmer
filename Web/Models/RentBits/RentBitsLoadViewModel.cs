using System.ComponentModel.DataAnnotations;

namespace Web.Models.RentBits
{
    public class RentBitsLoadViewModel
    {
        [StringLength(5)]
        public string ZipCode { get; set; }
        public string Location { get; set; }
    }
}
