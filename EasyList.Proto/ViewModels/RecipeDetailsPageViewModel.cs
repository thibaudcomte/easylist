using EasyList.Proto.Core.Recipes;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Collections.Generic;

namespace EasyList.Proto.ViewModels
{
    class RecipeDetailsPageViewModel : ViewModelBase
    {
        public Recipe Recipe { get; private set; }

        public RecipeDetailsPageViewModel(RecipesFacade recipesFacade)
        {
            _RecipesFacade = recipesFacade;
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            int id = (int)e.Parameter;
            Recipe = await _RecipesFacade.GetRecipeByIdAsync(id);

            base.OnNavigatedTo(e, viewModelState);
        }

        private readonly RecipesFacade _RecipesFacade;
    }
}
