using Microsoft.EntityFrameworkCore;
using MvcLab1.Data;
using MvcLab1.Models;

namespace MvcLab1.Repositories
{
    public class EfRecipeRepository : IRecipeRepository
    {
        private readonly AppDbContext _context;

        public EfRecipeRepository(AppDbContext context)
        {
            _context = context;
        }

        // Синхронные методы
        public IEnumerable<Recipe> GetAll()
        {
            return _context.Recipes.ToList();
        }

        public Recipe? GetById(int id)
        {
            return _context.Recipes.Find(id);
        }

        public void Add(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            _context.SaveChanges();
        }

        public void Update(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var recipe = GetById(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                _context.SaveChanges();
            }
        }

        // Асинхронные методы
        public async Task<IEnumerable<Recipe>> GetAllAsync()
        {
            return await _context.Recipes.ToListAsync();
        }

        public async Task<Recipe?> GetByIdAsync(int id)
        {
            return await _context.Recipes.FindAsync(id);
        }

        public async Task AddAsync(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var recipe = await GetByIdAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
        }

        // Дополнительные методы
        public IEnumerable<Recipe> GetByCuisine(string cuisine)
        {
            return _context.Recipes
                .Where(r => r.Cuisine == cuisine)
                .ToList();
        }

        public IEnumerable<Recipe> GetByDifficulty(string difficulty)
        {
            return _context.Recipes
                .Where(r => r.Difficulty == difficulty)
                .ToList();
        }

        public IEnumerable<Recipe> SearchByTitle(string searchTerm)
        {
            return _context.Recipes
                .Where(r => r.Title.Contains(searchTerm))
                .ToList();
        }
    }
}