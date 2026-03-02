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
    }
}