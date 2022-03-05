using System.ComponentModel.DataAnnotations;

namespace buyitWeb.Models
{
    public class CoverTypeModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
