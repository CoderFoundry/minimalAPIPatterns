using System.ComponentModel.DataAnnotations;

namespace minimalAPIStructure.Endpoints.Orders.UpdateOrder
{
    public class UpdateOrderRequest
    {

        // Customer must be specified
        [Required(ErrorMessage = "Customer Id is required.")]
        [Display(Name = "Customer Id")]
        public required int CustomerId { get; set; }

        // Product must be specified
        [Required(ErrorMessage = "Product Id is required.")]
        [Display(Name = "Product Id")]
        public int ProductId { get; set; }

        // Quantity must be at least 1
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Delivery date is required.")]
        [Display(Name = "Delivery Date")]
        [BusinessDay]
        public DateTime DeliveryDate { get; set; }


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

    }
}
