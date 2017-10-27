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

    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Quantity { get; set; }
        public EIngredientUnitType Unit { get; set; }

        public Ingredient Copy()
        {
            return new Ingredient
            {
                Id = Id,
                Name = Name,
                Quantity = Quantity,
                Unit = Unit
            };
        }
    }
}
