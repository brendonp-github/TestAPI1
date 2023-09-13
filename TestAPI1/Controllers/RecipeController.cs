using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;
using TestAPI1.Data;
using TestAPI1.Data.Models;

namespace TestAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecipeController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public RecipeController(ApplicationDBContext context) => _context = context;

        // GET: api/Recipe
        [HttpGet]
        public async Task<Recipe[]> GetRecipe()
        {
            var recipes = await _context.Recipe.ToArrayAsync();
            var ingredients = await _context.Ingredient.Where(ingredient => ingredient.RecipeID != null).ToArrayAsync();
            foreach (var recipe in recipes)
            {
                recipe.Ingredients = ingredients.Where(Ingredient => Ingredient.RecipeID == recipe.RecipeID).ToArray();
            }
            return recipes;
        }

        // GET: api/Recipe
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(long id)
        {
            var item = await _context.Recipe.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            item.Ingredients = await _context.Ingredient.Where(ingredient => ingredient.RecipeID == id).ToArrayAsync();
            return item;
        }

        [HttpDelete("{id}")]
        public async Task DeleteRecipe(long id)
        {
            var item = await _context.Recipe.FindAsync(id);
            if (item != null)
            {
                _context.Recipe.Remove(item);
                var ingredients = await _context.Ingredient.Where(ingredient => ingredient.RecipeID == id).ToArrayAsync();
                foreach (var ingredient in ingredients)
                {
                    _context.Ingredient.Remove(ingredient);
                }
                await _context.SaveChangesAsync();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<long>> PutRecipe(long id, Recipe item)
        {
            //if (id != item.RecipeID)
            //{
            //    return BadRequest();
            //}
            item.RecipeID = id;
            var existingItem = await _context.Recipe.FindAsync(id);
            existingItem.Name = item.Name;
            existingItem.Description = item.Description;
            existingItem.ImagePath = item.ImagePath;
            _context.Entry(existingItem).State = EntityState.Modified;
            var ingredients = await _context.Ingredient.Where(ingredient => ingredient.RecipeID == id).ToArrayAsync();
            foreach (var ingredient in ingredients)
            {
                _context.Ingredient.Remove(ingredient);
            }
            foreach (var ingredient in item.Ingredients)
            {
                ingredient.RecipeID = id;
                _context.Ingredient.Add(ingredient);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return existingItem.RecipeID;
        }

        [HttpPost]
        public async Task<ActionResult<long>> PostRecipe(Recipe item)
        {
            _context.Recipe.Add(item);
            await _context.SaveChangesAsync();
            foreach (var ingredient in item.Ingredients)
            {
                ingredient.RecipeID = item.RecipeID;
                _context.Ingredient.Add(ingredient);
            }
            await _context.SaveChangesAsync();
            return item.RecipeID;
        }
    }
}
