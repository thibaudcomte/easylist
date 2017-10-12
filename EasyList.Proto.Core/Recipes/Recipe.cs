using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EasyList.Proto.Core.Recipes
{
    [DebuggerDisplay("{Title}")]
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
        public List<Ingredient> Ingredients { get; } = new List<Ingredient>();

        /// <summary>
        /// Instructions.
        /// </summary>
        public List<string> Instructions { get; } = new List<string>();
    }
}
