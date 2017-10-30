using System;
using System.Collections.Generic;

namespace EasyList.Proto.Core.Recipes
{
    public class Recipe
    {
        public int Id { get; set; }

        /// <summary>
        /// Name of the recipe.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Link to web image.
        /// </summary>
        public Uri ImageUri { get; set; }

        /// <summary>
        /// Preparation time in minutes.
        /// </summary>
        public int PrepTime { get; set; }

        /// <summary>
        /// Cooking time in minutes.
        /// </summary>
        public int CookTime { get; set; }

        /// <summary>
        /// List of ingredients.
        /// </summary>
        public IEnumerable<Ingredient> Ingredients => _Ingredients;

        /// <summary>
        /// The preparation directives.
        /// </summary>
        public IEnumerable<string> Instructions => _Instructions;

        private readonly List<Ingredient> _Ingredients = new List<Ingredient>();
        private readonly List<string> _Instructions = new List<string>();

        public Recipe(int id, string title, string image, int prep, int cook, 
            IEnumerable<Ingredient> ingredients, IEnumerable<string> instructions)
        {
            Id = id;
            Title = title;
            ImageUri = new Uri(image);
            PrepTime = prep;
            CookTime = cook;
            _Ingredients.AddRange(ingredients);
            _Instructions.AddRange(instructions);
        }
    }
}
