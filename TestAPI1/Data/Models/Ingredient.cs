using System.ComponentModel.DataAnnotations;

namespace TestAPI1.Data.Models
{
    public class Ingredient
    {
        [Key]
        [Required]
        public long IngredientID { get; set; }
        public long? RecipeID { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
    }
}
