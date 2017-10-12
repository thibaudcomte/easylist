using EasyList.Proto.Core.Recipes;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Generic;

namespace EasyList.Proto.ViewModels
{
    public class RecipesPageViewModel : ViewModelBase
    {
        public ObservableCollection<Recipe> Recipes { get; }

        private Recipe _SelectedRecipe;
        public Recipe SelectedRecipe
        {
            get { return _SelectedRecipe; }
            set { SetProperty(ref _SelectedRecipe, value); }
        }

        public ICommand GoToRecipeDetailsCommand { get; }
        public ICommand AddToSelectionCommand { get; }
        public ICommand AddToFavoritesCommand { get; }

        private readonly RecipesFacade _RecipesFacade;

        public RecipesPageViewModel(RecipesFacade recipesFacade, INavigationService navigationService)
        {
            _RecipesFacade = recipesFacade;

            Recipes = new ObservableCollection<Recipe>();

            GoToRecipeDetailsCommand = new DelegateCommand(() =>
            {
                navigationService.Navigate(PageTokens.RecipeDetails.ToString(), SelectedRecipe.Id);
            }, () => { return SelectedRecipe != null; }).ObservesProperty(() => SelectedRecipe);

            AddToSelectionCommand = new DelegateCommand(() =>
            {
                recipesFacade.SelectedRecipesContainer.Add(SelectedRecipe);
            }, () => { return SelectedRecipe != null; }).ObservesProperty(() => SelectedRecipe);

            AddToFavoritesCommand = new DelegateCommand(() =>
            {
                recipesFacade.FavoriteRecipesContainer.Add(SelectedRecipe);
            }, () => { return SelectedRecipe != null; }).ObservesProperty(() => SelectedRecipe);
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            Recipes.Clear();

            foreach (var recipe in await _RecipesFacade.GetAllRecipesAsync())
            {
                Recipes.Add(recipe);
            }
        }
    }
}
