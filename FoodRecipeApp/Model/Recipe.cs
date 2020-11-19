using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FoodRecipeApp.Model
{
    public class Recipe
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Descrition { get; set; }
        public BindingList<string> Images { get; set; }
        public string YoutubeLink { get; set; }
        public BindingList<ComponentRecipe> ShoppingList { get; set; }
        public BindingList<CookingStep> Tutorials { get; set; }
        public int rate { get; set; }
        public bool IsFavorite { get; set; }

        public Recipe()
        {
            ID = MyProjectXML.ins.db.SelectNodes("root/Recipe").Count + 1;

            Images = new BindingList<string>();

            ShoppingList = new BindingList<ComponentRecipe>();

            Tutorials = new BindingList<CookingStep>();

            IsFavorite = false;
        }

        public Recipe(Recipe recipe)
        {
            ID = recipe.ID;
            Name = recipe.Name ?? throw new ArgumentNullException(nameof(recipe.Name));
            Descrition = recipe.Descrition ?? throw new ArgumentNullException(nameof(recipe.Descrition));
            Images = recipe.Images ?? throw new ArgumentNullException(nameof(recipe.Images));
            YoutubeLink = recipe.YoutubeLink ?? throw new ArgumentNullException(nameof(recipe.YoutubeLink));
            ShoppingList = recipe.ShoppingList ?? throw new ArgumentNullException(nameof(recipe.ShoppingList));
            Tutorials = recipe.Tutorials ?? throw new ArgumentNullException(nameof(recipe.Tutorials));
            rate = recipe.rate;
            IsFavorite = recipe.IsFavorite;
        }

        public static int NextID()
        {
            return MyProjectXML.ins.db.SelectNodes("root/Recipe").Count + 1;
        }
    }
}
