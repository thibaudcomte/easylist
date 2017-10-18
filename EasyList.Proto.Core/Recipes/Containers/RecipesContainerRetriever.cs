using EasyList.Proto.Core.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Recipes.Containers
{
    public class RecipesContainerRetriever
    {
        public RecipesContainerRetriever(RecipesContainer recipesContainer, IStorageReaderWriter storageReaderWriter, IRecipesProvider recipesProvider)
        {
            _recipesContainer = recipesContainer;
            _storageReaderWriter = storageReaderWriter;
            _recipesProvider = recipesProvider;
        }

        public async Task LoadAsync()
        {
            _recipesContainer.Clear();

            var ids = await _storageReaderWriter.ReadAsync<int[]>();
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    _recipesContainer.Add(await _recipesProvider.GetRecipeByIdAsync(id));
                }
            }
        }

        public async Task SaveAsync()
        {
            var ids = _recipesContainer.Select(recipe => recipe.Id).ToArray();
            await _storageReaderWriter.WriteAsync(ids);
        }

        private RecipesContainer _recipesContainer;
        private IStorageReaderWriter _storageReaderWriter;
        private IRecipesProvider _recipesProvider;
    }
}
