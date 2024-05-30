using System.ComponentModel.DataAnnotations;

namespace DataBaseConnectionMVC.Areas.LOC_State.Models
{
    public class LOC_StateModel
    {
        [Required]
        public int StateId { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public string? StateName { get; set; }

        [Required]
        public string? StateCode { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Modified { get; set; }
    }
    public class LOC_StateDropDownModel
    {
        [Required]
        public int StateId { get; set; }

        [Required]
        public string? StateName { get; set; }
    }
}
