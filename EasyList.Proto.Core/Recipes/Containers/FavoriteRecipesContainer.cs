using EasyList.Proto.Core.Containers;
using EasyList.Proto.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Recipes.Containers
{
    public class FavoriteRecipesContainer : RecipesContainer
    {
        private readonly RecipesContainerRetriever _FavoriteRecipesContainerRetriever;

        public FavoriteRecipesContainer(IStorageReaderWriter storageReaderWriter, IRecipesProvider recipesProvider)
        {
            _FavoriteRecipesContainerRetriever = new RecipesContainerRetriever(this, storageReaderWriter, recipesProvider);
        }

        public Task LoadAsync()
        {
            return _FavoriteRecipesContainerRetriever.LoadAsync();
        }

        public Task SaveAsync()
        {
            return _FavoriteRecipesContainerRetriever.SaveAsync();
        }
    }
}
