using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalAPIStructure.Models;

public record Customer
{
    // Customer ID is auto-generated
    public int Id { get; init; }

    [Length(2, 50, ErrorMessage = "{0} must be between {1} and {2} characters long.")]
    [Display(Name = "First Name")]
    [Required]
    public required string FirstName { get; set; }

    [Length(2, 50, ErrorMessage = "{0} must be between {1} and {2} characters long.")]
    [Display(Name = "Last Name")]
    [Required]
    public required string LastName { get; set; }

    [EmailAddress]
    [Display(Name = "Email Address")]
    public required string Email { get; set; }

    [Length(5, 100, ErrorMessage = "{0} must be between {1} and {2} characters.")]
    [Required]
    public required string Address { get; set; }

    public string? Address2 { get; set; }

    [Length(5, 100, ErrorMessage = "{0} must be between {1} and {2} characters.")]
    [Required]
    public required string City { get; set; }

    [Required]   
    public required string State { get; set; }

    [Display(Name = "ZIP Code")]
    [RegularExpression(@"^\d{5}(?:-\d{4})?$",
       ErrorMessage = "{0} must be a valid US ZIP code (e.g. 12345 or 12345-6789).")]
    public required string ZipCode { get; set; }

    //navigation collection: one Customer → many Orders
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    /// <summary>
    /// Not mapped to the DB; just a convenience for filtering & display.

    /// </summary>
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}"; 
}

