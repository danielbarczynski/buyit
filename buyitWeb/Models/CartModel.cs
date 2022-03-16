using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
    }
}
