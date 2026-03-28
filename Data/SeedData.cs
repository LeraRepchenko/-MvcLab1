using MvcLab1.Models;
using Microsoft.EntityFrameworkCore;

namespace MvcLab1.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            // Если в базе уже есть данные, пропускаем
            if (await context.Recipes.AnyAsync())
            {
                return;
            }

            // Добавляем тестовые рецепты
            var recipes = new Recipe[]
            {
                new Recipe
                {
                    Title = "Борщ",
                    Cuisine = "Русская",
                    PrepTime = 30,
                    CookTime = 60,
                    Difficulty = "Средняя",
                    Ingredients = "Свекла, капуста, картофель, морковь, лук, томатная паста, мясо, сметана",
                    Instructions = "1. Сварить бульон. 2. Нарезать и обжарить овощи. 3. Добавить в бульон. 4. Варить до готовности. 5. Подавать со сметаной.",
                    CreatedDate = DateTime.Now.AddDays(-30)
                },
                new Recipe
                {
                    Title = "Паста Карбонара",
                    Cuisine = "Итальянская",
                    PrepTime = 10,
                    CookTime = 20,
                    Difficulty = "Средняя",
                    Ingredients = "Спагетти, бекон, яйца, пармезан, чеснок, соль, перец",
                    Instructions = "1. Отварить пасту. 2. Обжарить бекон. 3. Смешать яйца с сыром. 4. Соединить все ингредиенты. 5. Подавать горячим.",
                    CreatedDate = DateTime.Now.AddDays(-20)
                },
                new Recipe
                {
                    Title = "Сырники классические",
                    Cuisine = "Русская",
                    PrepTime = 15,
                    CookTime = 10,
                    Difficulty = "Легкая",
                    Ingredients = "Творог, яйца, мука, сахар, соль, ванилин",
                    Instructions = "1. Смешать все ингредиенты. 2. Сформировать сырники. 3. Обжарить на сковороде. 4. Подавать со сметаной.",
                    CreatedDate = DateTime.Now.AddDays(-10)
                },
                new Recipe
                {
                    Title = "Салат Цезарь",
                    Cuisine = "Американская",
                    PrepTime = 20,
                    CookTime = 10,
                    Difficulty = "Легкая",
                    Ingredients = "Куриное филе, листья салата, помидоры черри, сухарики, пармезан, соус Цезарь",
                    Instructions = "1. Обжарить курицу. 2. Нарезать овощи. 3. Смешать ингредиенты. 4. Добавить соус. 5. Посыпать сыром.",
                    CreatedDate = DateTime.Now.AddDays(-5)
                },
                new Recipe
                {
                    Title = "Шарлотка с яблоками",
                    Cuisine = "Французская",
                    PrepTime = 15,
                    CookTime = 40,
                    Difficulty = "Легкая",
                    Ingredients = "Яблоки, яйца, сахар, мука, корица, ванилин",
                    Instructions = "1. Взбить яйца с сахаром. 2. Добавить муку. 3. Нарезать яблоки. 4. Вылить тесто в форму. 5. Выпекать при 180°C 40 минут.",
                    CreatedDate = DateTime.Now.AddDays(-2)
                }
            };

            await context.Recipes.AddRangeAsync(recipes);
            await context.SaveChangesAsync();
        }
    }
}