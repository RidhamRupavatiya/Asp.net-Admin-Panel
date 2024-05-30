using System.ComponentModel.DataAnnotations;

namespace DataBaseConnectionMVC.Areas.MST_Student.Models
{
    public class MST_StudentModel
    {
        [Required(ErrorMessage = "This Field is Required")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public string? StudentName { get; set;}

        [Required(ErrorMessage = "This Field is Required")]
        [RegularExpression(pattern: "^\\+?\\d{10}$", ErrorMessage = "Please enter a valid phone number")]
        public string? MobileNo { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [RegularExpression(pattern: "^\\+?\\d{10}$", ErrorMessage = "Please enter a valid phone number")]
        public string? FatherMobile { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [RegularExpression(pattern: "^\\S+@\\S+$", ErrorMessage = "Must Have '@' Symbol")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "This Field Field is Required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public DateTime? Created { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public DateTime? Modified { get; set;}
    }
}
