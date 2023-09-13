using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TestAPI1.Data;
using TestAPI1.Data.Models;

namespace TestAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public IngredientController(ApplicationDBContext context) => _context = context;

        // GET: api/Ingredient
        [HttpGet]
        public async Task<Ingredient[]> GetIngredient() => await _context.Ingredient.Where(ingredient => ingredient.RecipeID == null).ToArrayAsync();

        // GET: api/Ingredient
        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredient>> GetIngredient(long id)
        {
            var item = await _context.Ingredient.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpDelete("{id}")]
        public async Task DeleteIngredient(long id)
        {
            var item = await _context.Ingredient.FindAsync(id);
            if (item != null)
            {
                _context.Ingredient.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<long>> PutIngredient(long id, Ingredient item)
        {
            //if (id != item.RecipeID)
            //{
            //    return BadRequest();
            //}
            item.IngredientID = id;
            var existingItem = await _context.Ingredient.FindAsync(id);
            existingItem.Name = item.Name + "1";
            existingItem.Amount = item.Amount;
            _context.Entry(existingItem).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return existingItem.IngredientID;
        }

        [HttpPost]
        public async Task<ActionResult<long>> PostIngredient(Ingredient item)
        {
            item.RecipeID = null;
            item.IngredientID = 0;
            _context.Ingredient.Add(item);
            await _context.SaveChangesAsync();
            return item.IngredientID;
        }
        //[HttpPost("{id}")]
        //public async Task<ActionResult<long>> PostIngredient(long id, Ingredient item)
        //{
        //    item.RecipeID = id;
        //    _context.Ingredient.Add(item);
        //    await _context.SaveChangesAsync();
        //    return item.IngredientID;
        //}
    }
}
