using System.ComponentModel.DataAnnotations;

namespace DataBaseConnectionMVC.Areas.MST_Branch.Models
{
    public class MST_BranchModel
    {
        [Required]
        public int BranchId { get; set; }
        [Required]
        public string? BranchName { get; set; }
        [Required]
        public string? BranchCode { get; set; }
        [Required]
        public DateTime? Created { get; set; }
        [Required]
        public DateTime? Modified { get; set; }
    }
    public class MST_BranchDropDownModel
    {
        [Required]
        public int BranchId { get; set; }
        [Required]
        public string? BranchName { get; set; }
    }
}
