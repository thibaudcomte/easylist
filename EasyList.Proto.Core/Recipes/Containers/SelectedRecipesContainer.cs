using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace EasyList.Proto.Core.Recipes.Containers
{
    public class SelectedRecipesContainer : RecipesContainer, IDisposable
    {
        public IngredientsAggregator IngredientsAggregator { get; }

        public SelectedRecipesContainer()
        {
            IngredientsAggregator = new IngredientsAggregator();
            CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Recipe recipe in e.NewItems)
                    {
                        IngredientsAggregator.Add(recipe);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Recipe recipe in e.OldItems)
                    {
                        IngredientsAggregator.Remove(recipe);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    IngredientsAggregator.Clear();
                    break;
                default:
                    break;
            }
        }

        public void Dispose()
        {
            CollectionChanged -= OnCollectionChanged;
        }
    }
}
