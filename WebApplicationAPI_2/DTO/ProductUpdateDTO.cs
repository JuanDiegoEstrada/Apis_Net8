using System.ComponentModel.DataAnnotations;

namespace WebApplicationAPI_2.DTO
{
    public class ProductUpdateDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public DateTime DateAlta { get; set; }
        [Required]
        public string sku { get; set; }
    }
}
