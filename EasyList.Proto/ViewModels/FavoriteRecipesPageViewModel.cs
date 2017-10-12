using EasyList.Proto.Core.Recipes;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasyList.Proto.ViewModels
{
    class FavoriteRecipesPageViewModel : ViewModelBase
    {
        public ObservableCollection<Recipe> Recipes { get; }

        private Recipe _SelectedRecipe;
        public Recipe SelectedRecipe
        {
            get { return _SelectedRecipe; }
            set { SetProperty(ref _SelectedRecipe, value); }
        }

        public ICommand AddToSelectedRecipesCommand { get; }
        public ICommand RemoveRecipeCommand { get; }
        public ICommand GoToRecipeDetailsCommand { get; }

        public FavoriteRecipesPageViewModel(INavigationService navigationService, RecipesFacade recipesFacade)
        {
            Recipes = recipesFacade.FavoriteRecipesContainer;

            GoToRecipeDetailsCommand = new DelegateCommand(() =>
            {
                navigationService.Navigate(PageTokens.RecipeDetails.ToString(), SelectedRecipe.Id);
            }, () => { return SelectedRecipe != null; }).ObservesProperty(() => SelectedRecipe);

            AddToSelectedRecipesCommand = new DelegateCommand(() =>
            {
                recipesFacade.SelectedRecipesContainer.Add(SelectedRecipe);
            }, () => { return SelectedRecipe != null; }).ObservesProperty(() => SelectedRecipe);

            RemoveRecipeCommand = new DelegateCommand(() =>
            {
                recipesFacade.FavoriteRecipesContainer.Remove(SelectedRecipe);
            }, () => { return SelectedRecipe != null; }).ObservesProperty(() => SelectedRecipe);
        }
    }
}
