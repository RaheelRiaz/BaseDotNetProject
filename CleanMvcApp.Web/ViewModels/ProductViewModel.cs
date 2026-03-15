using System.ComponentModel.DataAnnotations;

namespace CleanMvcApp.Web.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    [DataType(DataType.Currency)]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

    public DateTime CreatedDate { get; set; }
}
