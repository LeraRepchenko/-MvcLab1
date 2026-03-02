using Microsoft.AspNetCore.Mvc;
using MvcLab1.Models;
using MvcLab1.Repositories;
namespace MvcLab1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _repository;
        // Внедрение зависимости через конструктор
        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }
        // GET: /Products
        public IActionResult Index()
        {
            var products = _repository.GetAll();
            return View(products);
        }
        // GET: /Products/Details/5
        public IActionResult Details(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }
        // GET: /Products/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: /Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedDate = DateTime.Now;
                _repository.Add(product);
                TempData["SuccessMessage"] = "Товар успешно добавлен!";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        // GET: /Products/Edit/5
        public IActionResult Edit(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }
        // POST: /Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update(product);
                    TempData["SuccessMessage"] = "Товар успешно обновлен!";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(product);
        }
        // GET: /Products/Delete/5
        public IActionResult Delete(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }
        // POST: /Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.Delete(id);
            TempData["SuccessMessage"] = "Товар удален!";
            return RedirectToAction(nameof(Index));
        }
        // GET: /Products/Category/Электроника
        public IActionResult Category(string category)
        {
            var products = _repository.GetByCategory(category);
            ViewBag.Category = category;
            return View(products);
        }
        // GET: /Products/InStock
        public IActionResult InStock()
        {
            var products = _repository.GetInStock();
            return View("Index", products);
        }
    }
}