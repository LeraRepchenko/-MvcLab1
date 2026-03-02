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
    }
}