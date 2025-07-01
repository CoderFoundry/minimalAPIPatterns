using minimalAPIStructure.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MinimalAPIStructure.Models;

public class Order : IValidatableObject
{

    // Order ID is auto-generated
    public int Id { get; init; }

    // Customer must be specified
    [Required (ErrorMessage = "Customer is required.")]    
    public required int CustomerId { get; set; }

    // Product must be specified
    [Required(ErrorMessage = "Product is required.")]
    public int ProductId { get; set; }


    // Price must be positive
    [Required(ErrorMessage = "Unit price is required.")]   
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
    public decimal UnitPrice { get; set; }

    // Quantity must be at least 1
    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    public DateTime OrderDate { get; init; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Delivery date is required.")]
    [BusinessDay(ErrorMessage = "Delivery date must be a business day (Monday–Friday).")]    
    public DateTime DeliveryDate { get; set; }

    // Read-only computed property
    // Computed read-only property
    [NotMapped]
    public decimal TotalPrice => UnitPrice * Quantity;

    

    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BusinessDayAttribute : ValidationAttribute
    {
        public BusinessDayAttribute()
            => ErrorMessage = "The {0} must fall on a business day (Monday–Friday).";

        public override bool IsValid(object? value)
        {
            // Let [Required] handle nulls if you need them.
            if (value is DateTime dt)
            {
                return dt.DayOfWeek is not DayOfWeek.Saturday
                    && dt.DayOfWeek is not DayOfWeek.Sunday;
            }
            return true;
        }
    }

    // Cross-property validation
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Example rule: total must not exceed $10,000
        if (TotalPrice > 10_000m)
        {
            yield return new ValidationResult(
                "Order total cannot exceed $10,000.",
                new[] { nameof(UnitPrice), nameof(Quantity) }
            );
        }
    }
    
    [NotMapped]    
    public string ProductName => Product.Name;
    
    [NotMapped]
    public string CustomerName
       => Customer is null
          ? string.Empty
          : $"{Customer.FirstName} {Customer.LastName}";
}