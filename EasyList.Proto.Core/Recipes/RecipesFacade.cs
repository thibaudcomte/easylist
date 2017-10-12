using EasyList.Proto.Core.Recipes.Containers;
using EasyList.Proto.Core.Storage.LocalStorage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Recipes
{
    public class RecipesFacade
    {
        private readonly IRecipesProvider _RecipesProvider;

        public SelectedRecipesContainer SelectedRecipesContainer { get; }
        public FavoriteRecipesContainer FavoriteRecipesContainer { get; }

        public RecipesFacade(IRecipesProvider recipesProvider)
        {
            _RecipesProvider = recipesProvider;

            SelectedRecipesContainer = new SelectedRecipesContainer();
            FavoriteRecipesContainer = new FavoriteRecipesContainer(new LocalStorageReaderWriter("favoriteRecipes"), recipesProvider);
        }

        public Task InitializeAsync()
        {
            return FavoriteRecipesContainer.LoadAsync();
        }

        public Task UnInitializeAsync()
        {
            return FavoriteRecipesContainer.SaveAsync();
        }

        public Task<IEnumerable<Recipe>> GetAllRecipesAsync()
        {
            return _RecipesProvider.GetAllRecipesAsync();
        }

        public Task<IEnumerable<Recipe>> FindRecipesAsync(string search)
        {
            return _RecipesProvider.FindRecipesAsync(search);
        }

        public Task<Recipe> GetRecipeByIdAsync(int id)
        {
            return _RecipesProvider.GetRecipeByIdAsync(id);
        }
    }
}
