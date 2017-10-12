using EasyList.Proto.Core.Recipes;
using Prism.Events;
using Prism.Mvvm;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace EasyList.Proto.ViewModels
{
    class MenuViewItem : BindableBase
    {
        private string _Title;
        public string Title
        {
            get { return _Title; }
            internal set { SetProperty(ref _Title, value); }
        }
        public string FontIcon { get; }
        public PageTokens Token { get; }

        public MenuViewItem(string title, string icon, PageTokens token)
        {
            Title = title;
            FontIcon = icon;
            Token = token;
        }
    }

    class MenuViewModel : ViewModelBase
    {
        public ObservableCollection<MenuViewItem> MenuItems { get; }
        public MenuViewItem SettingsMenuItem { get; }
        public MenuViewItem SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set
            {
                if (SetProperty(ref _selectedMenuItem, value))
                {
                    _navigationService?.Navigate(value.Token.ToString(), null);
                }
            }
        }

        public MenuViewModel(INavigationService navigationService, IEventAggregator eventAggregator,
            ISessionStateService sessionStateService, RecipesFacade recipesFacade)
        {
            _navigationService = navigationService;
            _sessionStateService = sessionStateService;
            _recipesFacade = recipesFacade;

            eventAggregator.GetEvent<NavigationStateChangedEvent>().Subscribe(OnNavigationStateChanged);

            MenuItems = new ObservableCollection<MenuViewItem>(new[]
            {
                new MenuViewItem("Recettes", "\ued56", PageTokens.Recipes),
                new MenuViewItem("Sélection", "\uf0e3", PageTokens.SelectedRecipes),
                new MenuViewItem("Liste des courses", "\ue7bf", PageTokens.ShoppingList),
                new MenuViewItem("Favories", "\ue113", PageTokens.FavoriteRecipes),
                new MenuViewItem("Drives", "\uec47", PageTokens.Stores)
            });

            SettingsMenuItem = new MenuViewItem("Paramètres", "\ue713", PageTokens.Settings);

            UpdateSelectedRecipesMenuItemTitle();
            UpdateFavoritesMenuItemTitle();

            _recipesFacade.SelectedRecipesContainer.CollectionChanged += OnSelectedRecipesContainerChanged;
            _recipesFacade.FavoriteRecipesContainer.CollectionChanged += OnFavoriteRecipesContainerChanged;
        }

        private void OnSelectedRecipesContainerChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateSelectedRecipesMenuItemTitle();
        }

        private void OnFavoriteRecipesContainerChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateFavoritesMenuItemTitle();
        }

        private void UpdateSelectedRecipesMenuItemTitle()
        {
            if (_recipesFacade.SelectedRecipesContainer.Any())
            {
                MenuItems[1].Title = $"Sélection {_recipesFacade.SelectedRecipesContainer.Count}";
            }
            else
            {
                MenuItems[1].Title = "Sélection";
            }
        }

        private void UpdateFavoritesMenuItemTitle()
        {
            if (_recipesFacade.FavoriteRecipesContainer.Any())
            {
                MenuItems[3].Title = $"Favories {_recipesFacade.FavoriteRecipesContainer.Count}";
            }
            else
            {
                MenuItems[3].Title = "Favories";
            }
        }


        private void OnNavigationStateChanged(NavigationStateChangedEventArgs args)
        {
            PageTokens currentPageToken;

            if (Enum.TryParse(args.Sender.Content.GetType().Name.Replace("Page", string.Empty), out currentPageToken))
            {
                _sessionStateService.SessionState[_CurrentPageTokenKey] = currentPageToken.ToString();
                var currentMenuItem = MenuItems.FirstOrDefault(item => item.Token == currentPageToken);
                if (currentMenuItem != null)
                {
                    SelectedMenuItem = currentMenuItem;
                }
            }
        }

        //private void OnUserRecipesCollectionChanged(object sender, System.EventArgs e)
        //{
        //    MenuItems[1].Title = $"Sélection {_recipesFacade.GetUserRecipes().Count()}";
        //}

        private const string _CurrentPageTokenKey = "CurrentPageToken";
        private INavigationService _navigationService;
        private ISessionStateService _sessionStateService;
        private MenuViewItem _selectedMenuItem;
        private RecipesFacade _recipesFacade;
    }
}
