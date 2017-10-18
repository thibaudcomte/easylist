using System.Diagnostics;

namespace EasyList.Proto.Core.Recipes
{
    public enum EIngredientUnitType
    {
        None,
        TeaSpoon,
        TableSpoon,
        MilliLiter,
        Grams,
        Cup
    }

    [DebuggerDisplay("{Name} ({Id}): {Quantity} {QuantityUnit}")]
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Quantity { get; set; }
        public EIngredientUnitType QuantityUnit { get; set; }

        public Ingredient Copy()
        {
            return new Ingredient
            {
                Id = Id,
                Name = Name,
                Quantity = Quantity,
                QuantityUnit = QuantityUnit
            };
        }
    }
}
