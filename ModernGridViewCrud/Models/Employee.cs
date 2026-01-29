using System.ComponentModel.DataAnnotations;

namespace ModernGridViewCrud.Models
{
    public class Employee
    {
        [Key]
        public string? EmployeeId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        public string? Designation { get; set; }

        [Required(ErrorMessage = "Date of Joining is required")]
        public string? DateOfJoining { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Qualification is required")]
        public string? Qualification { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string? State { get; set; }
    }
}
