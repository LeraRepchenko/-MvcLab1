using Microsoft.AspNetCore.Mvc;
using MvcLab1.Models;
using MvcLab1.Repositories;

namespace MvcLab1.Controllers
{
    public class RecipesController : Controller
    {
        private readonly IRecipeRepository _repository;

        public RecipesController(IRecipeRepository repository)
        {
            _repository = repository;
        }

        // GET: /Recipes
        public IActionResult Index()
        {
            var recipes = _repository.GetAll();
            return View(recipes);
        }

        // GET: /Recipes/Details/5
        public IActionResult Details(int id)
        {
            var recipe = _repository.GetById(id);
            if (recipe == null)
                return NotFound();
            return View(recipe);
        }

        // GET: /Recipes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Recipes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                recipe.CreatedDate = DateTime.Now;
                _repository.Add(recipe);
                TempData["SuccessMessage"] = "Рецепт успешно добавлен!";
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // GET: /Recipes/Edit/5
        public IActionResult Edit(int id)
        {
            var recipe = _repository.GetById(id);
            if (recipe == null)
                return NotFound();
            return View(recipe);
        }

        // POST: /Recipes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Recipe recipe)
        {
            if (id != recipe.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update(recipe);
                    TempData["SuccessMessage"] = "Рецепт успешно обновлен!";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(recipe);
        }

        // GET: /Recipes/Delete/5
        public IActionResult Delete(int id)
        {
            var recipe = _repository.GetById(id);
            if (recipe == null)
                return NotFound();
            return View(recipe);
        }

        // POST: /Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.Delete(id);
            TempData["SuccessMessage"] = "Рецепт удален!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Recipes/ByCuisine/Русская
        public IActionResult ByCuisine(string cuisine)
        {
            var recipes = _repository.GetByCuisine(cuisine);
            ViewBag.Cuisine = cuisine;
            return View("Index", recipes);
        }

        // GET: /Recipes/ByDifficulty/Легко
        public IActionResult ByDifficulty(string difficulty)
        {
            var recipes = _repository.GetByDifficulty(difficulty);
            ViewBag.Difficulty = difficulty;
            return View("Index", recipes);
        }
        /// Фильтрация рецептов по времени подготовки и приготовления
       
        public IActionResult ByTimeRange(int minPrepTime = 0, int maxCookTime = 120)
        {
            var recipes = _repository.GetByTimeRange(minPrepTime, maxCookTime);
            ViewBag.MinPrepTime = minPrepTime;
            ViewBag.MaxCookTime = maxCookTime;
            ViewBag.Title = $"Рецепты с подготовкой от {minPrepTime} мин и временем до {maxCookTime} мин";
            return View(recipes);
        }

        
        /// Топ самых простых  рецептов
       
        public IActionResult TopEasyRecipes(int count = 5)
        {
            var recipes = _repository.GetTopEasyRecipes(count);
            ViewBag.Title = $"Топ {count} самых простых и быстрых рецептов";
            ViewBag.Count = count;
            return View(recipes);
        }
        /// Результаты поиска
        public IActionResult Search(string searchTerm)
        {
            

            var recipes = _repository.SearchRecipes(searchTerm ?? string.Empty);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.Title = "Поиск рецептов";
            ViewBag.Count = recipes.Count();

            return View("Search", recipes);
        }


        /// Группировка рецептов по кухне

        public IActionResult GroupedByCuisine()
        {
            var grouped = _repository.GetRecipesGroupedByCuisine();
            return View(grouped);
        }

      
        /// Статистика по рецептам
        
        public IActionResult Statistics()
        {
            var allRecipes = _repository.GetAll().ToList();

            var stats = new RecipeStatisticsViewModel
            {
                TotalCount = _repository.GetTotalCount(),
                AverageTotalTime = _repository.GetAverageTotalTime(),
                AverageDifficultyScore = _repository.GetAverageDifficultyScore(),
                TotalTimeRange = _repository.GetTimeRange(),
                Cuisines = allRecipes
                    .GroupBy(r => r.Cuisine)
                    .Select(g => new CuisineStatViewModel
                    {
                        Cuisine = g.Key ?? "Без категории",
                        Count = g.Count(),
                        AverageTime = g.Average(r => r.PrepTime + r.CookTime),
                        MinPrepTime = g.Min(r => r.PrepTime),
                        MaxCookTime = g.Max(r => r.CookTime),
                        MostCommonDifficulty = g.GroupBy(r => r.Difficulty)
                            .OrderByDescending(d => d.Count())
                            .First().Key ?? "Неизвестно"
                    }).OrderBy(c => c.Cuisine)
            };

            return View(stats);
        }

       
        /// Пагинация
       
        public IActionResult Paginated(int page = 1, int pageSize = 5)
        {
            var recipes = _repository.GetRecipesWithPagination(page, pageSize);
            var totalPages = _repository.GetTotalPages(pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPreviousPage = page > 1;
            ViewBag.HasNextPage = page < totalPages;

            return View(recipes);
        }
    }
}