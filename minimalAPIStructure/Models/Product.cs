using MinimalAPIStructure.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace minimalAPIStructure.Models
{
    /// <summary>
    /// Represents a product that can be ordered.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Product ID is auto-generated.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the product.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Detailed description of the product.
        /// </summary>
        [StringLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// List price of the product. Used to snapshot price into orders.
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// Navigation property: orders for this product.
        /// </summary>
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
