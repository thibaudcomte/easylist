﻿using EasyList.Proto.ViewModels;
using EasyList.Proto.Views;
using Prism.Events;
using Prism.Windows.AppModel;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EasyList.Proto
{

    internal class NavigationEntry
    {
        public string Name { get; set; }
        public string Glyph { get; set; }
        public PageTokens Token { get; set; }

        public NavigationEntry(string name, string glyph, PageTokens token)
        {
            Name = name;
            Glyph = glyph;
            Token = token;
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppShell : Page
    {
        private readonly INavigationService navigationService;
        private readonly IEventAggregator eventAggregator;
        private readonly ISessionStateService sessionStateService;
        private List<NavigationEntry> navigationEntries;

        public AppShell(INavigationService navigationService, IEventAggregator eventAggregator,
            ISessionStateService sessionStateService)
        {
            InitializeComponent();

            this.navigationService = navigationService;
            this.eventAggregator = eventAggregator;
            this.sessionStateService = sessionStateService;

            UpdateNavigationViewMenuItems();

            eventAggregator.GetEvent<NavigationStateChangedEvent>().Subscribe(OnNavigationStateChanged);
        }

        public void SetContentFrame(Frame frame)
        {
            frame.Transitions = new TransitionCollection
            {
                new NavigationThemeTransition()
            };
            border.Child = frame;
        }

        private void UpdateNavigationViewMenuItems()
        {
            navigationEntries = new List<NavigationEntry>
            {
                new NavigationEntry("Toutes les recettes", "\ued56", PageTokens.Recipes),
                new NavigationEntry("Votre sélection", "\uf0e3", PageTokens.SelectedRecipes),
                new NavigationEntry("Liste des courses", "\ue7bf", PageTokens.ShoppingList),
                new NavigationEntry("Vos recettes favorites", "\ue113", PageTokens.FavoriteRecipes),
                new NavigationEntry("Vos drives", "\uec47", PageTokens.Stores)
            };

            hamburgerMenuControl.ItemsSource = navigationEntries;
        }

        private void OnMenuItemClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = e.ClickedItem as NavigationEntry;
            navigationService.Navigate(menuItem.Token.ToString(), null);
        }

        private void OnNavigationStateChanged(NavigationStateChangedEventArgs args)
        {
            PageTokens currentPageToken;

            if (Enum.TryParse(args.Sender.Content.GetType().Name.Replace("Page", string.Empty), out currentPageToken))
            {
                var entry = navigationEntries.FirstOrDefault(e => e.Token == currentPageToken);
                hamburgerMenuControl.SelectedItem = entry;
                hamburgerMenuControl.SelectedIndex = navigationEntries.IndexOf(entry);
            }
        }
    }
}
