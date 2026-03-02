using Microsoft.AspNetCore.Mvc;

namespace MvcLab1.Controllers
{
   
    [Route("restaurant")]
    [Route("cafe")]
    public class RestaurantController : Controller
    {
        // Данные о блюдах 
        private static List<Dish> _dishes = new List<Dish>
        {
            new Dish { Id = 1, Name = "Паста Карбонара", Description = "Спагетти с беконом в сливочном соусе", Price = 450, Category = "Паста", IsSpicy = false, IsVegetarian = false },
            new Dish { Id = 2, Name = "Пицца Маргарита", Description = "Томатный соус, моцарелла, базилик", Price = 550, Category = "Пицца", IsSpicy = false, IsVegetarian = true },
            new Dish { Id = 3, Name = "Цезарь с курицей", Description = "Салат с курицей, пармезаном и соусом Цезарь", Price = 380, Category = "Салаты", IsSpicy = false, IsVegetarian = false },
            new Dish { Id = 4, Name = "Том Ям", Description = "Острый суп с креветками на кокосовом молоке", Price = 490, Category = "Супы", IsSpicy = true, IsVegetarian = false },
            new Dish { Id = 5, Name = "Ризотто с грибами", Description = "Итальянское ризотто с белыми грибами", Price = 420, Category = "Паста", IsSpicy = false, IsVegetarian = true },
            new Dish { Id = 6, Name = "Тирамису", Description = "Классический итальянский десерт", Price = 320, Category = "Десерты", IsSpicy = false, IsVegetarian = true },
            new Dish { Id = 7, Name = "Борщ", Description = "Украинский борщ со сметаной", Price = 280, Category = "Супы", IsSpicy = false, IsVegetarian = false },
            new Dish { Id = 8, Name = "Греческий салат", Description = "Салат с фетой, огурцами и оливками", Price = 350, Category = "Салаты", IsSpicy = false, IsVegetarian = true }
        };

        /// <summary>
        ///  Menu - показывает меню ресторана
        /// </summary>
        [HttpGet]
        [Route("")]
        [Route("menu")]
        public IActionResult Menu()
        {
            // Получаем уникальные категории блюд 
            var categories = _dishes.Select(d => d.Category).Distinct().OrderBy(c => c).ToList();
            ViewBag.Categories = categories;

            // Группируем блюда по категориям
            var dishesByCategory = new Dictionary<string, List<Dish>>();
            foreach (var category in categories)
            {
                dishesByCategory[category] = _dishes.Where(d => d.Category == category).ToList();
            }

            ViewBag.DishesByCategory = dishesByCategory;
            ViewBag.TotalDishes = _dishes.Count;
            ViewBag.RestaurantName = "Уютный уголок";
            ViewBag.RestaurantRating = 4.8;

            ViewData["PageTitle"] = "Меню ресторана";
            ViewData["CurrentYear"] = DateTime.Now.Year;

            return View();
        }

        /// <summary>
        ///  Dish - принимает параметр dishId, показывает описание блюда
        /// </summary>
        [HttpGet]
        [Route("dish/{id:int}")]
        [Route("dish/details/{id:int}")]
        public IActionResult Dish(int id)
        {
            // Проверка существования блюда
            var dish = _dishes.FirstOrDefault(d => d.Id == id);

            if (dish == null)
            {
                ViewBag.ErrorMessage = $"Блюдо с ID {id} не найдено!";
                ViewBag.SuggestedIds = new[] { 1, 2, 3, 4, 5 };
                ViewData["PageTitle"] = "Блюдо не найдено";
                return View("NotFound");
            }

            // Передача данных о блюде
            ViewBag.Dish = dish;
            ViewBag.DishId = dish.Id;
            ViewBag.DishName = dish.Name;
            ViewBag.DishDescription = dish.Description;
            ViewBag.DishPrice = dish.Price;
            ViewBag.DishCategory = dish.Category;
            ViewBag.DishRating = (dish.Id % 5) + 1;

            // Добавляем категории и блюда по категориям для навигации
            var categories = _dishes.Select(d => d.Category).Distinct().OrderBy(c => c).ToList();
            ViewBag.Categories = categories;

            var dishesByCategory = new Dictionary<string, List<Dish>>();
            foreach (var category in categories)
            {
                dishesByCategory[category] = _dishes.Where(d => d.Category == category).ToList();
            }
            ViewBag.DishesByCategory = dishesByCategory;

            // Динамические вычисления
            ViewBag.HasDiscount = dish.Id % 3 == 0;
            ViewBag.DiscountPrice = ViewBag.HasDiscount ? (int)(dish.Price * 0.9) : dish.Price;

            // Навигация
            ViewBag.PrevId = dish.Id > 1 ? dish.Id - 1 : (int?)null;
            ViewBag.NextId = dish.Id < _dishes.Count ? dish.Id + 1 : (int?)null;

            ViewData["PageTitle"] = $"{dish.Name} - детали блюда";
            ViewData["CurrentDate"] = DateTime.Now.ToString("dd.MM.yyyy");

            return View();
        }

        /// <summary>
        /// Действие 3: TableBooking - форма бронирования столика
        /// </summary>
        [HttpGet]
        [Route("booking")]
        [Route("reserve")]
        public IActionResult TableBooking()
        {
            ViewBag.AvailableTables = 12;
            ViewBag.MaxGuests = 8;
            ViewBag.MinGuests = 1;

            ViewData["PageTitle"] = "Бронирование столика";
            ViewData["WorkingHours"] = "12:00 - 23:00";
            ViewData["Phone"] = "+7 (999) 123-45-67";

            // Генерация доступных временных слотов
            var timeSlots = new List<string>();
            for (int hour = 12; hour <= 22; hour++)
            {
                timeSlots.Add($"{hour}:00");
                if (hour < 22) timeSlots.Add($"{hour}:30");
            }
            ViewBag.TimeSlots = timeSlots;

            return View();
        }

        /// <summary>
        ///  TableBooking - 
        /// </summary>
        [HttpPost]
        [Route("booking")]
        [Route("reserve")]
        public IActionResult TableBooking(string name, string phone, int guests, string date, string time, string comments)
        {
            // Проверка входных данных
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || guests < 1)
            {
                ViewBag.ErrorMessage = "Пожалуйста, заполните все обязательные поля!";
                return RedirectToAction("TableBooking");
            }

            // Сохраняем данные бронирования во ViewBag для отображения
            ViewBag.Name = name;
            ViewBag.Phone = phone;
            ViewBag.Guests = guests;
            ViewBag.Date = date;
            ViewBag.Time = time;
            ViewBag.Comments = comments;
            ViewBag.BookingNumber = new Random().Next(1000, 9999);

            ViewData["PageTitle"] = "Бронирование подтверждено";

            return View("BookingConfirmation");
        }
    }

    /// <summary>
    /// Модель блюда
    /// </summary>
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public bool IsSpicy { get; set; }
        public bool IsVegetarian { get; set; }
    }
}
