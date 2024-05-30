using System.ComponentModel.DataAnnotations;

namespace DataBaseConnectionMVC.Areas.LOC_Country.Models
{
    public class LOC_CountryModel
    {
        [Required(ErrorMessage = "This is Required")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "This is Required")]
        public string? CountryName { get; set; }

        [Required(ErrorMessage = "This is Required")]
        public string? CountryCode { get; set; }

        [Required(ErrorMessage = "This is Required")]
        public DateTime Created { get; set; }

        [Required(ErrorMessage = "This is Required")]
        public DateTime Modified { get; set; }
    }
    public class LOC_CountryDropDownModel
    {
        public int CountryId { get; set; }

        [Required]
        public string? CountryName { get; set; }
    }
}