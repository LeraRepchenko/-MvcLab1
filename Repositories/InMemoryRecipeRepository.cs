using System;
using System.Collections.Generic;
using System.Linq;
using MvcLab1.Models;

namespace MvcLab1.Repositories
{
    public class InMemoryRecipeRepository : IRecipeRepository
    {
        private readonly List<Recipe> _recipes;
        private int _nextId = 1;

        public InMemoryRecipeRepository()
        {
            _recipes = new List<Recipe>();
            SeedData();
        }

        private void SeedData()
        {
            // Добавляем тестовые данные (минимум 3 записи)
            Add(new Recipe
            {
                Title = "Борщ",
                Cuisine = "Русская",
                PrepTime = 30,
                CookTime = 90,
                Difficulty = "Средне",
                Ingredients = "Свекла, капуста, картофель, морковь, лук, мясо, томатная паста",
                Instructions = "1. Сварить бульон. 2. Обжарить овощи. 3. Добавить в бульон. 4. Варить до готовности.",
                CreatedDate = DateTime.Now
            });

            Add(new Recipe
            {
                Title = "Паста Карбонара",
                Cuisine = "Итальянская",
                PrepTime = 15,
                CookTime = 20,
                Difficulty = "Легко",
                Ingredients = "Спагетти, яйца, бекон, сыр пармезан, чеснок, соль, перец",
                Instructions = "1. Сварить пасту. 2. Обжарить бекон. 3. Смешать яйца с сыром. 4. Соединить все ингредиенты.",
                CreatedDate = DateTime.Now
            });

            Add(new Recipe
            {
                Title = "Суши",
                Cuisine = "Японская",
                PrepTime = 60,
                CookTime = 30,
                Difficulty = "Сложно",
                Ingredients = "Рис для суши, нори, рыба лосось, огурец, авокадо, соевый соус",
                Instructions = "1. Сварить рис. 2. Подготовить начинку. 3. Скрутить роллы. 4. Нарезать и подавать.",
                CreatedDate = DateTime.Now
            });
        }

        public IEnumerable<Recipe> GetAll() => _recipes;

        public Recipe? GetById(int id) =>
            _recipes.FirstOrDefault(r => r.Id == id);

        public void Add(Recipe recipe)
        {
            recipe.Id = _nextId++;
            _recipes.Add(recipe);
        }

        public void Update(Recipe recipe)
        {
            var existing = GetById(recipe.Id);
            if (existing != null)
            {
                existing.Title = recipe.Title;
                existing.Cuisine = recipe.Cuisine;
                existing.PrepTime = recipe.PrepTime;
                existing.CookTime = recipe.CookTime;
                existing.Difficulty = recipe.Difficulty;
                existing.Ingredients = recipe.Ingredients;
                existing.Instructions = recipe.Instructions;
                // Не обновляем CreatedDate
            }
        }

        public void Delete(int id)
        {
            var recipe = GetById(id);
            if (recipe != null)
                _recipes.Remove(recipe);
        }

        public IEnumerable<Recipe> GetByCuisine(string cuisine) =>
            _recipes.Where(r => r.Cuisine.Equals(cuisine, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<Recipe> GetByDifficulty(string difficulty) =>
            _recipes.Where(r => r.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase));
    }
}
