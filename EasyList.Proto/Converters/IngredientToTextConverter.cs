using EasyList.Proto.Core.Recipes;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace EasyList.Proto.Converters
{
    class IngredientToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Ingredient ingredient = value as Ingredient;

            switch (ingredient?.Unit)
            {
                case EIngredientUnitType.None:
                    if (ingredient.Quantity > 0)
                    {
                        return $"{ingredient.Quantity} {ingredient.Name}";
                    }
                    else
                    {
                        return ingredient.Name;
                    }

                case EIngredientUnitType.TeaSpoon:
                    if (ingredient.Quantity >= 2)
                    {
                        return $"{ingredient.Quantity} cuillères à café de {ingredient.Name}";
                    }
                    else
                    {
                        return $"{ingredient.Quantity} cuillère à café de {ingredient.Name}";
                    }

                case EIngredientUnitType.TableSpoon:
                    if (ingredient.Quantity >= 2)
                    {
                        return $"{ingredient.Quantity} cuillères à soupe de {ingredient.Name}";
                    }
                    else
                    {
                        return $"{ingredient.Quantity} cuillère à soupe de {ingredient.Name}";
                    }

                case EIngredientUnitType.MilliLiter:
                    if (ingredient.Quantity > 1000)
                    {
                        return $"{ingredient.Quantity / 1000} L de {ingredient.Name}";
                    }
                    else
                    {
                        return $"{ingredient.Quantity} mL de {ingredient.Name}";
                    }

                case EIngredientUnitType.Grams:
                    if (ingredient.Quantity > 1000)
                    {
                        return $"{ingredient.Quantity / 1000} kg de {ingredient.Name}";
                    }
                    else
                    {
                        return $"{ingredient.Quantity} g de {ingredient.Name}";
                    }

                case EIngredientUnitType.Cup:
                    if (ingredient.Quantity >= 2)
                    {
                        return $"{ingredient.Quantity} tasses de {ingredient.Name}";
                    }
                    else
                    {
                        return $"{ingredient.Quantity} tasse de {ingredient.Name}";
                    }
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
