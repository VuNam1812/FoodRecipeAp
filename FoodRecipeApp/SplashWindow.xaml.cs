using FoodRecipeApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace FoodRecipeApp
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        public bool IsShow { get; set; }
     

        private string MyFolderPath = AppDomain.CurrentDomain.BaseDirectory + @"Image\Recipe";

        public SplashWindow()
        {
            InitializeComponent();
            
           
            IsShow = Convert.ToBoolean(MyProjectXML.ins.db.SelectSingleNode("root/ShowSplashScreen").Attributes["Value"].Value);
            
            LoadImageFromXML();
        }

        private void LoadImageFromXML()
        {
            int Length = MyProjectXML.ins.db.SelectNodes("root/Recipe").Count;
            int ImageRandom = MyRandom.instance.Next(Length);


            var MyRecipe = MyProjectXML.ins.db.SelectNodes("root/Recipe").Item(ImageRandom);
            var MyImagePath = MyRecipe.ChildNodes[2].ChildNodes[0].InnerText;

            recipeNameText.Text = MyRecipe.ChildNodes[0].InnerText;


            ImageSourceConverter converter = new ImageSourceConverter();
            recipeImage.Source = (ImageSource)converter.ConvertFromString($"{MyFolderPath}{MyImagePath}");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyProjectXML.ins.Save();

            SlideMainWindow();
        }

        private void SplashWD_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsShow)
            {
                SlideMainWindow();
            }
        }

        private void SlideMainWindow()
        {
            MainWindow mainWD = new MainWindow();

            SplashWD.Close();
            mainWD.Show();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            MyProjectXML.ins.db.SelectSingleNode("root/ShowSplashScreen").Attributes["Value"].Value = (!SplashCheckBox.IsChecked).ToString();
        }
    }
}
