using EasyList.Proto.Core.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Recipes
{
    public class IngredientsContainer : ObservableContainer<Ingredient>
    {
    }

    public class IngredientsAggregator
    {
        public IngredientsContainer IngredientsContainer { get; } = new IngredientsContainer();

        public void Add(Recipe recipe)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                Add(ingredient);
            }
        }

        public void Add(Ingredient ingredient)
        {
            Ingredient @in = IngredientsContainer.SingleOrDefault(ing => ing.Id == ingredient.Id);

            if (@in == null)
            {
                IngredientsContainer.Add(ingredient.Copy());
            }
            else
            {
                @in.Quantity += ingredient.Quantity;
            }
        }

        public void Remove(Recipe recipe)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                Remove(ingredient);
            }
        }

        public void Remove(Ingredient ingredient)
        {
            Ingredient @in = IngredientsContainer.SingleOrDefault(ing => ing.Id == ingredient.Id);

            if (@in != null)
            {
                @in.Quantity -= ingredient.Quantity;

                if (@in.Quantity <= 0.0f)
                {
                    IngredientsContainer.Remove(@in);
                }
            }
        }

        public void Clear()
        {
            IngredientsContainer.Clear();
        }
    }
}
