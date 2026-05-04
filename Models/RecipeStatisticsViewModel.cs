using System.Collections.Generic;

namespace MvcLab1.Models
{
   
    /// ViewModel для страницы статистики рецептов
 
    public class RecipeStatisticsViewModel
    {
        public int TotalCount { get; set; }
        public double AverageTotalTime { get; set; }
        public decimal AverageDifficultyScore { get; set; }
        public (int MinTotalTime, int MaxTotalTime) TotalTimeRange { get; set; }
        public IEnumerable<CuisineStatViewModel> Cuisines { get; set; }
    }

   
    /// ViewModel для статистики по кухне
   
    public class CuisineStatViewModel
    {
        public string Cuisine { get; set; } = string.Empty;
        public int Count { get; set; }
        public double AverageTime { get; set; }
        public int MinPrepTime { get; set; }
        public int MaxCookTime { get; set; }
        public string MostCommonDifficulty { get; set; } = string.Empty;
    }
}