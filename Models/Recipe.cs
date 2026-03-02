using System.ComponentModel.DataAnnotations;

namespace MvcLab1.Models
{
    public class Recipe
    {
        public Recipe()
        {
            Title = string.Empty;
            Cuisine = string.Empty;
            Difficulty = string.Empty;
            Ingredients = string.Empty;
            Instructions = string.Empty;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Название рецепта обязательно")]
        [StringLength(200, MinimumLength = 3,
            ErrorMessage = "Название должно быть от 3 до 200 символов")]
        [Display(Name = "Название рецепта")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Кухня обязательна")]
        [StringLength(50)]
        [Display(Name = "Кухня")]
        public string Cuisine { get; set; }

        [Required(ErrorMessage = "Время подготовки обязательно")]
        [Range(1, 240, ErrorMessage = "Время подготовки должно быть от 1 до 240 минут")]
        [Display(Name = "Время подготовки (мин)")]
        public int PrepTime { get; set; }

        [Required(ErrorMessage = "Время приготовления обязательно")]
        [Range(1, 480, ErrorMessage = "Время приготовления должно быть от 1 до 480 минут")]
        [Display(Name = "Время приготовления (мин)")]
        public int CookTime { get; set; }

        [Required(ErrorMessage = "Сложность обязательна")]
        [Display(Name = "Сложность")]
        public string Difficulty { get; set; }

        [Required(ErrorMessage = "Ингредиенты обязательны")]
        [StringLength(1000, MinimumLength = 10,
            ErrorMessage = "Ингредиенты должны быть от 10 до 1000 символов")]
        [Display(Name = "Ингредиенты")]
        [DataType(DataType.MultilineText)]
        public string Ingredients { get; set; }

        [Required(ErrorMessage = "Инструкция обязательна")]
        [StringLength(5000, MinimumLength = 20,
            ErrorMessage = "Инструкция должна быть от 20 до 5000 символов")]
        [Display(Name = "Инструкция")]
        [DataType(DataType.MultilineText)]
        public string Instructions { get; set; }

        [Display(Name = "Дата добавления")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        // Метод для получения общего времени приготовления
        [Display(Name = "Общее время")]
        public int GetTotalTime()
        {
            return PrepTime + CookTime;
        }
    }
}
