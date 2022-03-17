using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace buyitWeb.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }     
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }
        [Required]
        public int BookModelId { get; set; }
        [ValidateNever]
        public BookModel BookModel { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
}
