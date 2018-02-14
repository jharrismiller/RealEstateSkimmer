using System.ComponentModel.DataAnnotations;

namespace Web.Models.Default
{
    public class RealtorSearchViewModel
    {
        [Required]
        public int Miles { get; set; } = 25;
        [StringLength(5, MinimumLength = 5), Required, Display(Name = "Zip Code")]
        public string ZipCode { get; set; } = "22206";

        [Required, Display(Name="Pages to Capture")]
        public int PagesToCapture { get; set; } = 20;
    }
}
