using System;
using System.Collections.Generic;
using System.Linq;
using MvcLab1.Models; //Чтобы видел Product
namespace MvcLab1.Repositories // Должно быть Repositories, не Models!
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products;
        private int _nextId = 1;
        public InMemoryProductRepository()
        {
            _products = new List<Product>();
            SeedData();
        }
        private void SeedData()
        {
            // Добавляем тестовые данные
            Add(new Product
            {
                Name = "Ноутбук ASUS",
                Price = 75000,
                Category = "Электроника",
                Description = "Игровой ноутбук",
                CreatedDate = DateTime.Now,
                InStock = true
            });
            Add(new Product
            {
                Name = "Смартфон Samsung",
                Price = 45000,
                Category = "Электроника",
                Description = "Galaxy S23",
                CreatedDate = DateTime.Now,
                InStock = true
            });
        }
        public IEnumerable<Product> GetAll() => _products;
        public Product? GetById(int id) =>
        _products.FirstOrDefault(p => p.Id == id);
        public void Add(Product product)
        {
            product.Id = _nextId++;
            _products.Add(product);
        }
        public void Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Price = product.Price;
                existing.Category = product.Category;
                existing.Description = product.Description;
                existing.InStock = product.InStock;
                // Не обновляем CreatedDate
            }
        }
        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
                _products.Remove(product);
        }
        public IEnumerable<Product> GetByCategory(string category) =>
        _products.Where(p => p.Category.Equals(category,
       StringComparison.OrdinalIgnoreCase));
        public IEnumerable<Product> GetInStock() =>
        _products.Where(p => p.InStock);
    }
}