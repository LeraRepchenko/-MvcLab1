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




        /// 1. Фильтрация по времени (от минимального времени подготовки до максимального времени приготовления)

        public IEnumerable<Recipe> GetByTimeRange(int minPrepTime, int maxCookTime)
        {
            return _context.Recipes
                .Where(r => r.PrepTime >= minPrepTime && r.CookTime <= maxCookTime)
                .OrderBy(r => r.PrepTime + r.CookTime) // Сортировка по общему времени
                .ToList();
        }


        /// 2. Топ самых простых  рецептов

        public IEnumerable<Recipe> GetTopEasyRecipes(int count)
        {
            return _context.Recipes
                .Where(r => r.Difficulty == "Легкая")
                .OrderBy(r => r.PrepTime + r.CookTime) // Сначала самые быстрые
                .Take(count)
                .ToList();
        }


        /// 3. Поиск рецептов по названию, кухне или ингредиентам

        public IEnumerable<Recipe> SearchRecipes(string searchTerm)
        {
            return _context.Recipes
                .Where(r => r.Title.Contains(searchTerm) ||
                            r.Cuisine.Contains(searchTerm) ||
                            r.Ingredients.Contains(searchTerm))
                .OrderBy(r => r.Title)
                .ToList();
        }


        /// 4. Среднее общее время приготовления (PrepTime + CookTime)

        public double GetAverageTotalTime()
        {
            return _context.Recipes
                .Select(r => r.PrepTime + r.CookTime)
                .Average();
        }


        /// 5. Общее количество рецептов

        public int GetTotalCount()
        {
            return _context.Recipes.Count();
        }


        /// 6. Проверка наличия рецептов в указанной кухне

        public bool AnyInCuisine(string cuisine)
        {
            return _context.Recipes.Any(r => r.Cuisine == cuisine);
        }


        /// 7. Группировка рецептов по кухне

        public IEnumerable<IGrouping<string, Recipe>> GetRecipesGroupedByCuisine()
        {
            // Сначала получаем данные из БД, затем группируем в памяти
            var recipes = _context.Recipes.ToList();
            return recipes
                .GroupBy(r => r.Cuisine)
                .OrderBy(g => g.Key)
                .ToList();
        }


        /// 8. Пагинация (постраничное отображение)

        public IEnumerable<Recipe> GetRecipesWithPagination(int page, int pageSize)
        {
            return _context.Recipes
                .OrderBy(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        /// 9. Общее количество страниц

        public int GetTotalPages(int pageSize)
        {
            int totalCount = _context.Recipes.Count();
            return (int)Math.Ceiling(totalCount / (double)pageSize);
        }


        /// 10. Средняя сложность (Легкая=1, Средняя=2, Сложная=3)

        public decimal GetAverageDifficultyScore()
        {
            var recipes = _context.Recipes.ToList();
            if (!recipes.Any()) return 0;

            var scores = recipes.Select(r => r.Difficulty == "Легкая" ? 1 :
                                              r.Difficulty == "Средняя" ? 2 : 3);
            return (decimal)scores.Average();
        }


        /// 11. Диапазон времени (минимальное и максимальное общее время)

        public (int MinTotalTime, int MaxTotalTime) GetTimeRange()
        {
            var times = _context.Recipes
                .Select(r => r.PrepTime + r.CookTime)
                .ToList();

            return (
                MinTotalTime: times.Any() ? times.Min() : 0,
                MaxTotalTime: times.Any() ? times.Max() : 0
            );
        }

        // Асинхронные версии новых методов

        public async Task<IEnumerable<Recipe>> GetByTimeRangeAsync(int minPrepTime, int maxCookTime)
        {
            return await _context.Recipes
                .Where(r => r.PrepTime >= minPrepTime && r.CookTime <= maxCookTime)
                .OrderBy(r => r.PrepTime + r.CookTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Recipe>> GetTopEasyRecipesAsync(int count)
        {
            return await _context.Recipes
                .Where(r => r.Difficulty == "Легкая")
                .OrderBy(r => r.PrepTime + r.CookTime)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Recipe>> SearchRecipesAsync(string searchTerm)
        {
            return await _context.Recipes
                .Where(r => r.Title.Contains(searchTerm) ||
                            r.Cuisine.Contains(searchTerm) ||
                            r.Ingredients.Contains(searchTerm))
                .OrderBy(r => r.Title)
                .ToListAsync();
        }

        public async Task<double> GetAverageTotalTimeAsync()
        {
            return await _context.Recipes
                .Select(r => r.PrepTime + r.CookTime)
                .AverageAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Recipes.CountAsync();
        }

        public async Task<decimal> GetAverageDifficultyScoreAsync()
        {
            var recipes = await _context.Recipes.ToListAsync();
            if (!recipes.Any()) return 0;

            var scores = recipes.Select(r => r.Difficulty == "Легкая" ? 1 :
                                              r.Difficulty == "Средняя" ? 2 : 3);
            return (decimal)scores.Average();
        }

        public async Task<IEnumerable<IGrouping<string, Recipe>>> GetRecipesGroupedByCuisineAsync()
        {
            
            var recipes = await _context.Recipes.ToListAsync();
            return recipes
                .GroupBy(r => r.Cuisine)
                .OrderBy(g => g.Key)
                .ToList();
        }
    }
}