﻿using System.ComponentModel.DataAnnotations;

namespace buyitWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Order { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
