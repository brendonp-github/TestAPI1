using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAPI1.Data.Models
{
    public class Recipe
    {
        [Key]
        [Required]
        public long RecipeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public Ingredient[] Ingredients { get; set; }
    }
}
