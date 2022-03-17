using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace buyitWeb.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        [ValidateNever]
        public int BookModelId { get; set; }
        public BookModel BookModel { get; set; }
        public int Count { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        [NotMapped]
        public double Price { get; set; }
    }
}
