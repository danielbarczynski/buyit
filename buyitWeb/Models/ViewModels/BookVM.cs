using Microsoft.AspNetCore.Mvc.Rendering;

namespace buyitWeb.Models.ViewModels
{
    public class BookVM
    {
        public BookModel BookModel { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Covers { get; set; }
    }
}
