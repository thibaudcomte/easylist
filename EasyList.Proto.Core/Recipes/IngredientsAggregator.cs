using EasyList.Proto.Core.Misc.Containers;
using System.Linq;

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
                IngredientsContainer.Add(ingredient);
            }
            else
            {
                Ingredient @new = @in.IncreaseQuantity(ingredient.Quantity);
                IngredientsContainer.Remove(@in);
                IngredientsContainer.Add(@new);
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
                Ingredient @new = @in.DecreaseQuantity(ingredient.Quantity);
                IngredientsContainer.Remove(@in);

                if (@new.Quantity > 0)
                {
                    IngredientsContainer.Add(@new);
                }
            }
        }

        public void Clear()
        {
            IngredientsContainer.Clear();
        }
    }
}
