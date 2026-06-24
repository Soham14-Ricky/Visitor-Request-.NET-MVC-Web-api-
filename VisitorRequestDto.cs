using System.ComponentModel.DataAnnotations;

namespace VisitorMVC.Models.DTOs
{
    public class VisitorRequestDto
    {
        public int RequestId { get; set; }

        [Required]
        public string VisitorName { get; set; }

        [Required]

        [RegularExpression(@"^[0-9]{10}$",
            ErrorMessage =
            "Mobile number must be 10 digits")]
        public string MobileNumber { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string PersonToMeet { get; set; }

        [Required]
        public string PurposeOfVisit { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }

        public string? Status { get; set; }

        public string? Remarks { get; set; }

        public int CreatedBy { get; set; }
    }
}