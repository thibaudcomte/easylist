using EasyList.Proto.Core.Recipes;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasyList.Proto.ViewModels
{
    class SelectedRecipesPageViewModel : ViewModelBase
    {
        public ObservableCollection<Recipe> Recipes { get; }

        private Recipe _SelectedRecipe;
        public Recipe SelectedRecipe
        {
            get { return _SelectedRecipe; }
            set { SetProperty(ref _SelectedRecipe, value); }
        }

        public ICommand GoToRecipeDetailsCommand { get; }
        public ICommand DeleteRecipeCommand { get; }

        public SelectedRecipesPageViewModel(INavigationService navigationService, RecipesFacade recipesFacade)
        {
            Recipes = recipesFacade.SelectedRecipesContainer;

            GoToRecipeDetailsCommand = new DelegateCommand(() =>
            {
                navigationService.Navigate(PageTokens.RecipeDetails.ToString(), SelectedRecipe.Id);
            }, () => { return SelectedRecipe != null; }).ObservesProperty(() => SelectedRecipe);

            DeleteRecipeCommand = new DelegateCommand(() =>
            {
                recipesFacade.SelectedRecipesContainer.Remove(SelectedRecipe);
            }, () => { return SelectedRecipe != null; }).ObservesProperty(() => SelectedRecipe);
        }
    }
}
