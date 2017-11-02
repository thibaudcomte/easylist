using System;
using System.Collections;
using System.Collections.Generic;

namespace EasyList.Proto.Core.Recipes
{
    public class IngredientItemCategory
    {
        public int Id { get; }
        public string Name { get; }

        public IngredientItemCategory(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class IngredientItemCategoryEqualityComparer : IEqualityComparer<IngredientItemCategory>
    {
        public bool Equals(IngredientItemCategory x, IngredientItemCategory y)
            => x.Id == y.Id && x.Name == y.Name;

        public int GetHashCode(IngredientItemCategory cat) => cat.Id;
    }

    public class IngredientItem
    {
        public int Id { get; }
        public string Name { get; }
        public IngredientItemCategory Category { get; }

        public IngredientItem(int id, string name, IngredientItemCategory category)
        {
            Id = id;
            Name = name;
            Category = category;
        }
    }

    public enum EIngredientUnitType
    {
        None,
        TeaSpoon,
        TableSpoon,
        MilliLiter,
        Grams,
        Cup
    }

    public class Ingredient
    {
        public IngredientItem Item { get; }
        public int Id => Item.Id;
        public string Name => Item.Name;
        public float Quantity { get; }
        public EIngredientUnitType Unit { get; }

        public Ingredient(IngredientItem item, float quantity, EIngredientUnitType unit)
        {
            Item = item;
            Quantity = quantity;
            Unit = unit;
        }

        public Ingredient IncreaseQuantity(float quantity) => new Ingredient(Item, Quantity + quantity, Unit);
        public Ingredient DecreaseQuantity(float quantity) => new Ingredient(Item, Math.Max(Quantity - quantity, 0), Unit);
    }
}
