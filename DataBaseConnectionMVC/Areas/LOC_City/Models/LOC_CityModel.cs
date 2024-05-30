using System.ComponentModel.DataAnnotations;

namespace DataBaseConnectionMVC.Areas.LOC_City.Models
{
    public class LOC_CityModel
    {
        [Required]
        public int CityId { get; set; }

        [Required]
        public int StateId { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public string? StateName { get; set; }
        [Required]
        public string? CityName { get; set; }
        [Required]
        public string? CityCode { get; set; }
        [Required]
        public DateTime? Created { get; set; }
        [Required]
        public DateTime? Modified { get; set; }
    }
    public class LOC_CityDropDownModel
    {
        [Required]
        public int CityId { get; set; }

        [Required]
        public string? CityName { get; set; }
    }
}
