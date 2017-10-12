using EasyList.Proto.Core.Recipes;
using EasyList.Proto.Core.Retailers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Shopping
{
    public class ShoppingFacade : IDisposable
    {
        public ShoppingList ShoppingList { get; }

        private readonly RecipesFacade _RecipesFacade;

        public ShoppingFacade(RecipesFacade recipesFacade)
        {
            _RecipesFacade = recipesFacade;

            ShoppingList = new ShoppingList();

            UpdateShoppingList();

            _RecipesFacade.SelectedRecipesContainer.IngredientsAggregator.IngredientsContainer.CollectionChanged += 
                OnSelectedRecipesIngredientsAggregatorCollectionChanged;
        }

        private void UpdateShoppingList()
        {
            ShoppingList.Clear();

            foreach (var ingredient in _RecipesFacade.SelectedRecipesContainer.IngredientsAggregator.IngredientsContainer)
            {
                ShoppingList.Add(new ShoppingListItem(ingredient));
            }
        }

        private void OnSelectedRecipesIngredientsAggregatorCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateShoppingList();
        }

        public void Dispose()
        {
            _RecipesFacade.SelectedRecipesContainer.IngredientsAggregator.IngredientsContainer.CollectionChanged -=
                OnSelectedRecipesIngredientsAggregatorCollectionChanged;
        }
    }
}
