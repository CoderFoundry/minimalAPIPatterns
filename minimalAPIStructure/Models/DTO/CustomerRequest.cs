using System.ComponentModel.DataAnnotations;

namespace minimalAPIStructure.Models.DTO
{
    /// <summary>
    /// The payload clients send when creating or updating a customer.
    /// </summary>
    public class CustomerRequest
    {
        /// <summary>
        /// The customer’s first name (2–50 chars).
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// The customer’s last name (2–50 chars).
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        /// <summary>
        /// A valid e-mail address.
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "{0} must be a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Street address (10–100 chars).
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string Address { get; set; } = null!;

        /// <summary>
        /// Optional second address line.
        /// </summary>
        public string? Address2 { get; set; }

        /// <summary>
        /// City (10–100 chars).
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string City { get; set; } = null!;

        /// <summary>
        /// Two-letter state abbreviation.
        /// </summary>
        [Required]
        public string State { get; set; } = null!;

        /// <summary>
        /// U.S. ZIP code (e.g. 12345 or 12345-6789).
        /// </summary>
        [Display(Name = "ZIP Code")]
        [RegularExpression(@"^\d{5}(?:-\d{4})?$", ErrorMessage = "{0} must be a valid US ZIP code (e.g. 12345 or 12345-6789).")]
        public string ZipCode { get; set; } = null!;
    }

}