using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Recipes
{
    public interface IRecipesProvider
    {
        Task<IEnumerable<Recipe>> GetAllRecipesAsync();
        Task<Recipe> GetRecipeByIdAsync(int id);
        Task<IEnumerable<Recipe>> FindRecipesAsync(string search);
    }
}
