using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FoodRecipeApp.Model
{
    public class RecipeDAO
    {
        public static string ImagePathDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Image\Recipe";

        public static List<Recipe> GetAllForLst()
        { 
            List<Recipe> lstRecipe = new List<Recipe>();

            var lstXMLNodes = MyProjectXML.ins.db.SelectNodes("root/Recipe");
            
            foreach (XmlElement item in lstXMLNodes)
            {
                Recipe recipe = new Recipe();
                recipe.ID = Convert.ToInt32(item.GetAttribute("Value"));
                //GET name
                recipe.Name = item.ChildNodes[0].InnerText;

                //GET desc
                recipe.Descrition = item.ChildNodes[1].InnerText;

                //GET image
                foreach (XmlElement elementXML in item.ChildNodes[2].ChildNodes)
                {
                    recipe.Images.Add(ImagePathDirectory+elementXML.InnerText);
                }

                //get Youtube Link
                recipe.YoutubeLink = item.ChildNodes[3].InnerText;


                //get shopping list
                foreach (XmlElement elementXML in item.ChildNodes[4].ChildNodes)
                {
                    recipe.ShoppingList.Add(new ComponentRecipe()
                    {
                        Name = elementXML.GetAttribute("Name"),
                        Count = elementXML.GetAttribute("Value")
                    });
                }

                //get tutorials
                foreach (XmlElement elementXML in item.ChildNodes[5].ChildNodes)
                {
                    CookingStep cookingStep = new CookingStep()
                    {
                        StepNumber = Convert.ToInt32(elementXML.GetAttribute("Value")),
                        StepDerc = elementXML.ChildNodes[1].InnerText
                    };

                    foreach (XmlElement ImageSourceXML in elementXML.ChildNodes[0])
                    {
                        cookingStep.StepImages.Add(ImagePathDirectory + ImageSourceXML.InnerText);
                    }

                    recipe.Tutorials.Add(cookingStep);
                }

                recipe.rate = Convert.ToInt32((item.ChildNodes[6] as XmlElement).GetAttribute("Value"));
                recipe.IsFavorite = Convert.ToBoolean((item.ChildNodes[7] as XmlElement).GetAttribute("Value"));
                lstRecipe.Add(recipe);
            }

            return lstRecipe;
        }

        public static Recipe GetAllInfoByID(int id)
        {
            Recipe recipe = new Recipe();
            var MyXMLNode = MyProjectXML.ins.db.SelectSingleNode($"root/Recipe [@Value='{id}']");
            recipe.ID = id;

            //GET Name
            recipe.Name = MyXMLNode.ChildNodes[0].InnerText;

            //GET desc
            recipe.Descrition = MyXMLNode.ChildNodes[1].InnerText;

            //GET image
            foreach (XmlElement elementXML in MyXMLNode.ChildNodes[2].ChildNodes)
            {
                recipe.Images.Add(ImagePathDirectory + elementXML.InnerText);
            }

            //get Youtube Link
            recipe.YoutubeLink = MyXMLNode.ChildNodes[3].InnerText;


            //get shopping list
            foreach (XmlElement elementXML in MyXMLNode.ChildNodes[4].ChildNodes)
            {
                recipe.ShoppingList.Add(new ComponentRecipe()
                {
                    Name = elementXML.GetAttribute("Name"),
                    Count = elementXML.GetAttribute("Value")
                });
            }

            //get tutorials
            foreach (XmlElement elementXML in MyXMLNode.ChildNodes[5].ChildNodes)
            {
                CookingStep cookingStep = new CookingStep()
                {
                    StepNumber = Convert.ToInt32(elementXML.GetAttribute("Value")),
                    StepDerc = elementXML.ChildNodes[1].InnerText
                };

                foreach (XmlElement ImageSourceXML in elementXML.ChildNodes[0])
                {
                    cookingStep.StepImages.Add(ImagePathDirectory + ImageSourceXML.InnerText);
                }

                recipe.Tutorials.Add(cookingStep);
            }

            recipe.rate = Convert.ToInt32((MyXMLNode.ChildNodes[6] as XmlElement).GetAttribute("Value"));
            recipe.IsFavorite = Convert.ToBoolean((MyXMLNode.ChildNodes[7] as XmlElement).GetAttribute("Value"));

            return recipe;
        }
    }
}
