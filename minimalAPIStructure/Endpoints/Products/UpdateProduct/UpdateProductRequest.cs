using System.ComponentModel.DataAnnotations;

namespace minimalAPIStructure.Endpoints.Products.UpdateProduct
{
    public class UpdateProductRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
