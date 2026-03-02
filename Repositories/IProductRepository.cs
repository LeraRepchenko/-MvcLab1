using System.Collections.Generic;
using MvcLab1.Models; // Добавить, чтобы видел Product

namespace MvcLab1.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
        IEnumerable<Product> GetByCategory(string category);
        IEnumerable<Product> GetInStock();
    }
}
