using System.ComponentModel.DataAnnotations;

namespace VisitorMVC.Models.DTOs
{
    public class RejectRequestDto
    {
        public int RequestId { get; set; }

        [Required]
        public string Remarks { get; set; }
    }
}