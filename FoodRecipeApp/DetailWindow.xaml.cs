using FoodRecipeApp.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace FoodRecipeApp
{
    /// <summary>
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    /// 
    

    public partial class DetailWindow : Window
    {
        private bool IsShowTutorial = false;
        private Recipe MyRecipe { get; set; }

        public delegate void MyDelegate(Recipe Target);
        public event MyDelegate SetColorFavoriteButton;

        public DetailWindow()
        {
            InitializeComponent();
           
        }

        private void TutorialButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsShowTutorial)
            {
                ((Storyboard)DetailWD.FindResource("Slide_TutorialReverse")).Begin();
            }
            else
            {
                ((Storyboard)DetailWD.FindResource("Slide_Tutorial")).Begin();

                
            }
            IsShowTutorial = !IsShowTutorial;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ((Storyboard)DetailWD.FindResource("Slide_TutorialReverse")).Begin();
            IsShowTutorial = !IsShowTutorial;
        }

        private void DetailWD_ContentRendered(object sender, EventArgs e)
        {
            Thread.Sleep(100);
            ((Storyboard)DetailWD.FindResource("LoadWindow")).Begin();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (SetColorFavoriteButton != null)
            {
                SetColorFavoriteButton(DataContext as Recipe);
            }

            ((Storyboard)DetailWD.FindResource("CloseWindow")).Completed += new EventHandler((s, o) => {
               
                DetailWD.Close();
            });
        }

        private void ButtonFavorite_Loaded(object sender, RoutedEventArgs e)
        {
            SetChangeColorFavoriteButton(sender as Button);

        }

        private void ButtonFavorite_Click(object sender, RoutedEventArgs e)
        {
            MyRecipe.IsFavorite = !MyRecipe.IsFavorite;

            SetChangeColorFavoriteButton(sender as Button);


        }

        private void SetChangeColorFavoriteButton(Button button)
        {
            string colorButton = (MyRecipe.IsFavorite) ? "#FFFF0000" : "#FFFFFFFF";
            string colorContentButton = (MyRecipe.IsFavorite) ? "#FFFFFFFF" : "#FF000000";

            button.Background = (Brush)(new BrushConverter()).ConvertFromString(colorButton);
            (button.Content as PackIcon).Foreground = (Brush)(new BrushConverter()).ConvertFromString(colorContentButton);
        }

        private void DetailWD_Loaded(object sender, RoutedEventArgs e)
        {
            MyRecipe = DetailWD.DataContext as Recipe;

           lstImageRecipe.ItemsSource = MyRecipe.Images;
            lstImageRecipe.SelectedItem = MyRecipe.Images[0];

            lstComponent.ItemsSource = MyRecipe.ShoppingList;

            lstTutorial.ItemsSource = MyRecipe.Tutorials;
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            lstImageRecipe.SelectedItem = GetImageFormCondition(MyRecipe.Images.Count -1, 1, 0);
        }

        private void ButtonPre_Click(object sender, RoutedEventArgs e)
        {
            lstImageRecipe.SelectedItem = GetImageFormCondition(0, -1, MyRecipe.Images.Count-1);
        }

        private string GetImageFormCondition(int Condition,int DefaltValue, int PosFormConditon)
        {
            string result = "";

            BindingList<string> content = lstImageRecipe.ItemsSource as BindingList<string>;

            int indexOldSelected = content.IndexOf(lstImageRecipe.SelectedItem.ToString());
            int newIndex = (indexOldSelected == Condition) ? PosFormConditon : indexOldSelected + DefaltValue;
            result =  content.ElementAt(newIndex);

            return result;
        }

        private void lstImageRecipe_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var MyScroll = VisualTreeHelperEx.FindDescendantByType<ScrollViewer>(sender as ListView);

            if (e.Delta > 0)
            {
                MyScroll.LineLeft();
            }
            else
            {
                MyScroll.LineRight();
                e.Handled = true;
            }
        }

        private void YoutubeButton_Click(object sender, RoutedEventArgs e)
        {
            if ((this.DataContext as Recipe).YoutubeLink.Length == 0)
            {
                MessageBox.Show("Thực đơn chưa Video!!");
            }
            else
            {
                System.Diagnostics.Process.Start((this.DataContext as Recipe).YoutubeLink);
            }
        }
    }
}
