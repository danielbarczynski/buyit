using System.ComponentModel.DataAnnotations;

namespace buyitWeb.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }
        [Required]
        [Range(1, 10000)]
        public double ListPrice { get; set; }
        [Required]
        [Range(1, 10000)]
        public double Price30 { get; set; }
        [Required]
        [Range(1, 10000)]
        public double Price70 { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int CoverTypeId { get; set; }
        public CategoryModel Category { get; set; }
        public CoverTypeModel CoverType { get; set; }
    }
}
