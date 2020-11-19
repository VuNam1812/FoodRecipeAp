using FoodRecipeApp.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace FoodRecipeApp
{
    /// <summary>
    /// Interaction logic for AddRecipe.xaml
    /// </summary>
    public partial class AddRecipe : Window
    {
        private string ImagePathDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Image\Recipe";
        private Recipe MyRecipe { get; set; }
        private List<string> ImagePaths = new List<string>();

        public delegate void MyDelegate(int ID);
        public event MyDelegate AddRecipeIntoLst;

        public AddRecipe()
        {
            InitializeComponent();

            CookingStep.currentnumber = 0;

            MyRecipe = new Recipe();
            
            lstComponent.ItemsSource = MyRecipe.ShoppingList;

            lstTutorial.ItemsSource = MyRecipe.Tutorials;


        }

        private void AddComponentButton_Click(object sender, RoutedEventArgs e)
        {
            bool IsFullInfo = (ComponentName.Text.Length != 0 && ComponentCount.Text.Length != 0) ? true : false;

            if (IsFullInfo)
            {
                MyRecipe.ShoppingList.Add(new ComponentRecipe()
                {
                    Name = ComponentName.Text,
                    Count = ComponentCount.Text
                });
            }
            ComponentName.Text = ComponentCount.Text = "";


        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((Storyboard)AddWD.FindResource("CloseWindow")).Completed += new EventHandler((s,o) => {
                AddWD.Close();
            });
        }

        private void AddRecipeImageButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog() { InitialDirectory = ImagePathDirectory };
            screen.Multiselect = true;
            try
            {
                if (screen.ShowDialog() == true)
                {
                    MyRecipe.Images.Clear();
                    CountRecipeImage.Content = "+" + screen.FileNames.ToList().Count().ToString();
                    screen.FileNames.ToList().ForEach(item =>
                    {
                        var a = item.Substring(ImagePathDirectory.Length, item.Length - ImagePathDirectory.Length);
                        MyRecipe.Images.Add(a);
                    });
                    CountRecipeImage.Visibility = (screen.FileNames.ToList().Count() != 0) ? Visibility.Visible : Visibility.Collapsed;

                }
            }
            catch (Exception error)
            {

            }
        }

        private void AddStepImageButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog() { InitialDirectory = ImagePathDirectory };
            screen.Multiselect = true;
            try
            {
                if (screen.ShowDialog() == true)
                {
                    ImagePaths.Clear();
                    CountStepImageButton.Content = "+" + screen.FileNames.ToList().Count().ToString();
                    screen.FileNames.ToList().ForEach(item =>
                    {
                        var a = item.Substring(ImagePathDirectory.Length, item.Length - ImagePathDirectory.Length);
                        ImagePaths.Add(a);
                    });
                    CountStepImageButton.Visibility = (screen.FileNames.ToList().Count() != 0) ? Visibility.Visible : Visibility.Collapsed;

                }
            }
            catch (Exception error)
            {

            }
        }

        private void GetAllContentFormWindow()
        {
            MyRecipe.Name = RecipeName.Text;
            MyRecipe.Descrition = DercRecipeText.Text;
            MyRecipe.YoutubeLink = ytbLink.Text;
            MyRecipe.rate = BasicRatingBar.Value;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            GetAllContentFormWindow();

            var root = MyProjectXML.ins.db.CreateElement("Recipe");
            root.SetAttribute("Value", MyRecipe.ID.ToString());

            var Name = MyProjectXML.ins.db.CreateElement("Name");
            Name.InnerXml = MyRecipe.Name;

            var Descrition = MyProjectXML.ins.db.CreateElement("Descrition");
            Descrition.InnerText = MyRecipe.Descrition;

            var Images = MyProjectXML.ins.db.CreateElement("Images");
            MyRecipe.Images.ToList().ForEach(item =>
            {
                var image = MyProjectXML.ins.db.CreateElement("Image");
                image.InnerText = item;

                Images.AppendChild(image);

            });

            var YoutubeLink = MyProjectXML.ins.db.CreateElement("YoutubeLink");
            YoutubeLink.InnerText = MyRecipe.YoutubeLink;

            var ShoppingList = MyProjectXML.ins.db.CreateElement("ShoppingList");
            MyRecipe.ShoppingList.ToList().ForEach(item =>
            {
                var ele = MyProjectXML.ins.db.CreateElement("Element");

                ele.SetAttribute("Name", item.Name);
                ele.SetAttribute("Value", item.Count);

                ShoppingList.AppendChild(ele);
            });

            var Tutorial = MyProjectXML.ins.db.CreateElement("Tutorial");
            MyRecipe.Tutorials.ToList().ForEach(tutorial =>
            {
                var step = MyProjectXML.ins.db.CreateElement("Step");
                step.SetAttribute("Value", tutorial.StepNumber.ToString());

                var images = MyProjectXML.ins.db.CreateElement("Images");
                tutorial.StepImages.ForEach(item =>
                {
                    var image = MyProjectXML.ins.db.CreateElement("Image");
                    image.InnerText = item.Substring(ImagePathDirectory.Length, item.Length - ImagePathDirectory.Length);

                    images.AppendChild(image);
                });


                var derc = MyProjectXML.ins.db.CreateElement("Descrition");
                derc.InnerText = tutorial.StepDerc;

                step.AppendChild(images);
                step.AppendChild(derc);

                Tutorial.AppendChild(step);
            });

            var Rate = MyProjectXML.ins.db.CreateElement("Rate");
            Rate.SetAttribute("Value", MyRecipe.rate.ToString());

            var IsFavorite = MyProjectXML.ins.db.CreateElement("IsFavorite");
            IsFavorite.SetAttribute("Value", "False");

            root.AppendChild(Name);
            root.AppendChild(Descrition);
            root.AppendChild(Images);
            root.AppendChild(YoutubeLink);
            root.AppendChild(ShoppingList);
            root.AppendChild(Tutorial);
            root.AppendChild(Rate);
            root.AppendChild(IsFavorite);

            MyProjectXML.ins.db.SelectSingleNode("root").AppendChild(root);

            MyProjectXML.ins.Save();


            if (AddRecipeIntoLst != null)
            {

                AddRecipeIntoLst(MyRecipe.ID);
            }

            ResetForm();
        }

        private void ResetForm()
        {
            RecipeName.Text = DercRecipeText.Text = ytbLink.Text = "";
            CountRecipeImage.Visibility = Visibility.Collapsed;
            MyRecipe.ID = Recipe.NextID();
            MyRecipe.Images.Clear();
            MyRecipe.ShoppingList.Clear();
            MyRecipe.Tutorials.Clear();
            CookingStep.currentnumber = 0;
            BasicRatingBar.Value = 0;
            
        }

        private void AddStep_Click(object sender, RoutedEventArgs e)
        {
            bool IsFullInfo = (StepDerc.Text.Length != 0 && ImagePaths.Count() != 0) ? true : false;

            if (IsFullInfo)
            {
                var newTutorial = new CookingStep()
                {
                    
                    StepDerc = StepDerc.Text
                };
                ImagePaths.ForEach(item => {
                    newTutorial.StepImages.Add($"{ImagePathDirectory}{item}");
                });
                MyRecipe.Tutorials.Add(newTutorial);
            }

            StepDerc.Text = "";
            ImagePaths.Clear();
            CountStepImageButton.Visibility = Visibility.Collapsed;
        }

        private void AddWD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddWD.DragMove();
        }
    }

}
