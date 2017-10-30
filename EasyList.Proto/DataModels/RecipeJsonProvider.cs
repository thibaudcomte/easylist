using EasyList.Proto.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace EasyList.Proto.DataModels
{
    class RecipesJsonProvider : IRecipesProvider
    {
        private IEnumerable<IngredientItemCategory> _IngredientCategories;
        private IEnumerable<IngredientItem> _IngredientItems;
        private List<Recipe> _Recipes;

        private async Task ReadIngredientItemsDataBaseAsync()
        {
            var uri = new Uri("ms-appx:///DataModels/ingredient-items.json");
            var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var text = await FileIO.ReadTextAsync(file);

            JsonObject jsonObject = JsonObject.Parse(text);

            _IngredientCategories = from cat in jsonObject.GetNamedArray("ingredient_item_categories")
                             let jsonCat = cat.GetObject()
                             select new IngredientItemCategory((int)jsonCat.GetNamedNumber("id"),
                             jsonCat.GetNamedString("name"));

            _IngredientItems = from cat in jsonObject.GetNamedArray("ingredient_items")
                        let jsonItem = cat.GetObject()
                        select new IngredientItem((int)jsonItem.GetNamedNumber("id"),
                        jsonItem.GetNamedString("name"),
                        _IngredientCategories.Single(c => c.Id == (int)jsonItem.GetNamedNumber("cat")));
        }

        private async Task ReadRecipesDataBaseAsync()
        {
            if (_Recipes != null)
                return;

            await ReadIngredientItemsDataBaseAsync();

            var uri = new Uri("ms-appx:///DataModels/recipes.json");
            var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var text = await FileIO.ReadTextAsync(file);

            JsonObject jsonObject = JsonObject.Parse(text);

            var recipes = from rec in jsonObject.GetNamedArray("recipes")
                          let jsonRec = rec.GetObject()
                          let id = (int)jsonRec.GetNamedNumber("id")
                          let title = jsonRec.GetNamedString("title")
                          let image = jsonRec.GetNamedString("img")
                          let prep = (int)jsonRec.GetNamedNumber("prep")
                          let cook = (int)jsonRec.GetNamedNumber("cook")
                          select new Recipe(id, title, image, prep, cook,
                            (from ingr in jsonRec.GetNamedArray("ingr")
                             let jsonIngr = ingr.GetObject()
                             let itemId = (int)jsonIngr.GetNamedNumber("id")
                             let item = _IngredientItems.Single(i => i.Id == itemId)
                             let qty = jsonIngr.ContainsKey("qty") ? (float)jsonIngr.GetNamedNumber("qty") : 0
                             let unit = jsonIngr.ContainsKey("unit") ? GetUnitFromText(jsonIngr.GetNamedString("unit")) : EIngredientUnitType.None
                             select new Ingredient(item, qty, unit)),
                            jsonRec.GetNamedArray("inst").Select(e => e.GetString()));

            _Recipes = recipes.ToList();
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
            await ReadRecipesDataBaseAsync();
            return _Recipes;
        }

        public async Task<Recipe> GetRecipeByIdAsync(int id)
        {
            await ReadRecipesDataBaseAsync();
            return _Recipes.Find(recipe => recipe.Id == id);
        }

        public async Task<IEnumerable<Recipe>> FindRecipesAsync(string search)
        {
            await ReadRecipesDataBaseAsync();
            search = search.ToLower();
            return _Recipes.FindAll(r => r.Title.ToLower().Contains(search));
        }
    }
}
