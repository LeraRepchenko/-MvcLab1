using MvcLab1.Models;

namespace MvcLab1.Repositories
{
    public class InMemoryRecipeRepository : IRecipeRepository
    {
        private static List<Recipe> _recipes = new();
        private static int _nextId = 1;

        // ========== СУЩЕСТВУЮЩИЕ МЕТОДЫ ==========

        public IEnumerable<Recipe> GetAll() => _recipes;

        public Recipe? GetById(int id) => _recipes.FirstOrDefault(r => r.Id == id);

        public void Add(Recipe recipe)
        {
            recipe.Id = _nextId++;
            recipe.CreatedDate = DateTime.Now;
            _recipes.Add(recipe);
        }

        public void Update(Recipe recipe)
        {
            var existing = GetById(recipe.Id);
            if (existing != null)
            {
                var index = _recipes.IndexOf(existing);
                _recipes[index] = recipe;
            }
        }

        public void Delete(int id)
        {
            var recipe = GetById(id);
            if (recipe != null)
                _recipes.Remove(recipe);
        }

        public IEnumerable<Recipe> GetByCuisine(string cuisine)
            => _recipes.Where(r => r.Cuisine == cuisine).ToList();

        public IEnumerable<Recipe> GetByDifficulty(string difficulty)
            => _recipes.Where(r => r.Difficulty == difficulty).ToList();

        public IEnumerable<Recipe> SearchByTitle(string searchTerm)
            => _recipes.Where(r => r.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

        // ========== НОВЫЕ МЕТОДЫ ДЛЯ LINQ-ЗАПРОСОВ ==========

        public IEnumerable<Recipe> GetByTimeRange(int minPrepTime, int maxCookTime)
        {
            return _recipes
                .Where(r => r.PrepTime >= minPrepTime && r.CookTime <= maxCookTime)
                .OrderBy(r => r.PrepTime + r.CookTime)
                .ToList();
        }

        public IEnumerable<Recipe> GetTopEasyRecipes(int count)
        {
            return _recipes
                .Where(r => r.Difficulty == "Легкая")
                .OrderBy(r => r.PrepTime + r.CookTime)
                .Take(count)
                .ToList();
        }

        public IEnumerable<Recipe> SearchRecipes(string searchTerm)
        {
            return _recipes
                .Where(r => r.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            r.Cuisine.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            r.Ingredients.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .OrderBy(r => r.Title)
                .ToList();
        }

        public double GetAverageTotalTime()
        {
            if (!_recipes.Any()) return 0;
            return _recipes.Average(r => r.PrepTime + r.CookTime);
        }

        public int GetTotalCount() => _recipes.Count;

        public bool AnyInCuisine(string cuisine)
            => _recipes.Any(r => r.Cuisine == cuisine);

        public IEnumerable<IGrouping<string, Recipe>> GetRecipesGroupedByCuisine()
        {
            return _recipes
                .GroupBy(r => r.Cuisine)
                .OrderBy(g => g.Key)
                .ToList();
        }

        public IEnumerable<Recipe> GetRecipesWithPagination(int page, int pageSize)
        {
            return _recipes
                .OrderBy(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetTotalPages(int pageSize)
        {
            int totalCount = _recipes.Count;
            return totalCount == 0 ? 1 : (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public decimal GetAverageDifficultyScore()
        {
            if (!_recipes.Any()) return 0;

            var scores = _recipes.Select(r => r.Difficulty == "Легкая" ? 1 :
                                              r.Difficulty == "Средняя" ? 2 : 3);
            return (decimal)scores.Average();
        }

        public (int MinTotalTime, int MaxTotalTime) GetTimeRange()
        {
            if (!_recipes.Any()) return (0, 0);

            var times = _recipes.Select(r => r.PrepTime + r.CookTime);
            return (times.Min(), times.Max());
        }

        // ========== АСИНХРОННЫЕ МЕТОДЫ ==========

        public async Task<IEnumerable<Recipe>> GetAllAsync()
            => await Task.Run(() => _recipes);

        public async Task<Recipe?> GetByIdAsync(int id)
            => await Task.Run(() => GetById(id));

        public async Task AddAsync(Recipe recipe)
        {
            recipe.Id = _nextId++;
            recipe.CreatedDate = DateTime.Now;
            await Task.Run(() => _recipes.Add(recipe));
        }

        public async Task UpdateAsync(Recipe recipe)
            => await Task.Run(() => Update(recipe));

        public async Task DeleteAsync(int id)
            => await Task.Run(() => Delete(id));

        public async Task<IEnumerable<Recipe>> GetByTimeRangeAsync(int minPrepTime, int maxCookTime)
            => await Task.Run(() => GetByTimeRange(minPrepTime, maxCookTime));

        public async Task<IEnumerable<Recipe>> GetTopEasyRecipesAsync(int count)
            => await Task.Run(() => GetTopEasyRecipes(count));

        public async Task<IEnumerable<Recipe>> SearchRecipesAsync(string searchTerm)
            => await Task.Run(() => SearchRecipes(searchTerm));

        public async Task<double> GetAverageTotalTimeAsync()
            => await Task.Run(() => GetAverageTotalTime());

        public async Task<int> GetTotalCountAsync()
            => await Task.Run(() => GetTotalCount());

        public async Task<decimal> GetAverageDifficultyScoreAsync()
            => await Task.Run(() => GetAverageDifficultyScore());

        public async Task<IEnumerable<IGrouping<string, Recipe>>> GetRecipesGroupedByCuisineAsync()
            => await Task.Run(() => GetRecipesGroupedByCuisine());
    }
}