using System.Collections.Generic;
using MvcLab1.Models;

namespace MvcLab1.Repositories
{
    public interface IRecipeRepository
    {
        IEnumerable<Recipe> GetAll();
        Recipe? GetById(int id);
        void Add(Recipe recipe);
        void Update(Recipe recipe);
        void Delete(int id);
        IEnumerable<Recipe> GetByCuisine(string cuisine);
        IEnumerable<Recipe> GetByDifficulty(string difficulty);

        /// Фильтрация по времени приготовления
        IEnumerable<Recipe> GetByTimeRange(int minPrepTime, int maxCookTime);

        /// Топ самых простых рецептов
        IEnumerable<Recipe> GetTopEasyRecipes(int count);

        /// Поиск по названию или ингредиентам
        IEnumerable<Recipe> SearchRecipes(string searchTerm);

        ///Среднее время приготовления
        double GetAverageTotalTime();

        ///Проверка наличия рецептов в кухне
        bool AnyInCuisine(string cuisine);

        ///Группировка по кухне
        IEnumerable<IGrouping<string, Recipe>> GetRecipesGroupedByCuisine();

        ///Пагинация<
        IEnumerable<Recipe> GetRecipesWithPagination(int page, int pageSize);

        ///Общее количество страниц
        int GetTotalPages(int pageSize);

        ///Статистика: общее количество рецептов
        int GetTotalCount();

        /// Статистика: средняя сложность (как число)
        decimal GetAverageDifficultyScore();

        ///Диапазон времени приготовления
        (int MinTotalTime, int MaxTotalTime) GetTimeRange();

        // Асинхронные версии
        Task<IEnumerable<Recipe>> GetAllAsync();
        Task<Recipe?> GetByIdAsync(int id);
        Task<IEnumerable<Recipe>> GetByTimeRangeAsync(int minPrepTime, int maxCookTime);
        Task<IEnumerable<Recipe>> GetTopEasyRecipesAsync(int count);
        Task<IEnumerable<Recipe>> SearchRecipesAsync(string searchTerm);
        Task<double> GetAverageTotalTimeAsync();
        Task<int> GetTotalCountAsync();
        Task<decimal> GetAverageDifficultyScoreAsync();
        Task<IEnumerable<IGrouping<string, Recipe>>> GetRecipesGroupedByCuisineAsync();
    }
}


