using System.ComponentModel.DataAnnotations;

namespace minimalAPIStructure.Models.DTO
{
    public class CustomerResponse
    {
        /// <summary>
        /// The database key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The customer’s first name.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// The customer’s last name.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// A valid e-mail address.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Street address.
        /// </summary>
        public string Address { get; set; } = null!;

        /// <summary>
        /// Optional second address line.
        /// </summary>
        public string? Address2 { get; set; }

        /// <summary>
        /// City.
        /// </summary>
        public string City { get; set; } = null!;

        /// <summary>
        /// State abbreviation.
        /// </summary>
        public string State { get; set; } = null!;

        /// <summary>
        /// U.S. ZIP code.
        /// </summary>
        [Display(Name = "ZIP Code")]
        public string ZipCode { get; set; } = null!;

        /// <summary>
        /// Convenience property for the full name (FirstName + LastName).
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";
    }

}