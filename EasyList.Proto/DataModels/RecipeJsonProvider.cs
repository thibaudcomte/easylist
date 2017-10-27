using EasyList.Proto.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace EasyList.Proto.DataModels
{
    class RecipesJsonProvider : IRecipesProvider
    {
        private List<Recipe> _Recipes;

        private async Task EnsureLoadRecipesAsync()
        {
            if (_Recipes != null)
                return;

            var uri = new Uri("ms-appx:///DataModels/recipes.json");
            var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var text = await FileIO.ReadTextAsync(file);

            JsonObject jsonObject = JsonObject.Parse(text);
            JsonArray jsonRecipes = jsonObject.GetNamedArray("recipes");

            _Recipes = new List<Recipe>();

            foreach (var jsonRecipe in jsonRecipes)
            {
                var recipeObject = jsonRecipe.GetObject();
                var recipe = new Recipe
                {
                    Id = _Recipes.Count,
                    Title = recipeObject.GetNamedString("title"),
                    ImageUri = new Uri(recipeObject.GetNamedString("img")),
                    PrepTime = (int)recipeObject.GetNamedNumber("prep"),
                    CookTime = (int)recipeObject.GetNamedNumber("cook")
                };

                JsonArray jsonInstructions = recipeObject.GetNamedArray("inst");

                foreach (var jsonInstruction in jsonInstructions)
                {
                    recipe.Instructions.Add(jsonInstruction.GetString());
                }

                JsonArray jsonIngredients = recipeObject.GetNamedArray("ingr");

                foreach (var jsonIngredient in jsonIngredients)
                {
                    var ingredientObject = jsonIngredient.GetObject();
                    var ingredient = new Ingredient
                    {
                        Name = ingredientObject.GetNamedString("name"),
                        Id = _Recipes.Count*1000 + recipe.Ingredients.Count
                    };

                    if (ingredientObject.ContainsKey("qty"))
                        ingredient.Quantity = (float)ingredientObject.GetNamedNumber("qty");

                    if (ingredientObject.ContainsKey("unit"))
                        ingredient.Unit = GetUnitFromText(ingredientObject.GetNamedString("unit"));

                    recipe.Ingredients.Add(ingredient);
                }

                _Recipes.Add(recipe);
            }
        }

        private static EIngredientUnitType GetUnitFromText(string value)
        {
            switch (value.ToLower())
            {
                case "gr": return EIngredientUnitType.Grams;
                case "ml": return EIngredientUnitType.MilliLiter;
                case "tbsp": return EIngredientUnitType.TableSpoon;
                case "tsp": return EIngredientUnitType.TeaSpoon;
                case "vr": return EIngredientUnitType.Cup;
                default: return EIngredientUnitType.None;
            }
        }

        public async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
        {
            await EnsureLoadRecipesAsync();
            return _Recipes;
        }

        public async Task<Recipe> GetRecipeByIdAsync(int id)
        {
            await EnsureLoadRecipesAsync();
            return _Recipes.Find(recipe => recipe.Id == id);
        }

        public async Task<IEnumerable<Recipe>> FindRecipesAsync(string search)
        {
            await EnsureLoadRecipesAsync();
            search = search.ToLower();
            return _Recipes.FindAll(r => r.Title.ToLower().Contains(search));
        }
    }
}
