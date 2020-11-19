using FoodRecipeApp.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.CodeDom;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace FoodRecipeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int PosPageButton { get; set; }
        private int maxnumber;
        private Button SeletedButton { get; set; }
        private int currentNumberEle { get; set; }
        private bool IsSeleceFavoritelst { get; set; }

        public ObservableCollection<Recipe> MylstRecipes { get; set; }
        public ObservableCollection<Recipe> MylstRecipes_Favorite { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            PosPageButton = -1;
            IsSeleceFavoritelst = false;

            AnimationButton.ins.SetButtonBefore(DashboardButton);
            MylstRecipes = new ObservableCollection<Recipe>(RecipeDAO.GetAllForLst());
            MylstRecipes_Favorite = new ObservableCollection<Recipe>(MylstRecipes.Where(item => item.IsFavorite == true).ToList());

            CollectionView View = (CollectionView)CollectionViewSource.GetDefaultView(MylstRecipes);
            View.Filter = item =>
            {
                if (String.IsNullOrEmpty(SearchText.Text))
                {
                    return true;
                }
                else
                {
                    return ((item as Recipe).Name.IndexOf(SearchText.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                }
            };

            SearchResult.ItemsSource = MylstRecipes;

            currentNumberEle = Global.MaxColOfList * Global.MaxRowOfList;

            maxnumber = (MylstRecipes.Count / currentNumberEle) + (((MylstRecipes.Count % currentNumberEle) == 0) ? 0 : 1);
            CheckShowPageNumber();
            lstRecipe.ItemsSource = MylstRecipes.Take(currentNumberEle);
        }

        private void Menubutton_Click(object sender, RoutedEventArgs e)
        {
            AnimationButton.ins.Animation_ButtonClick(sender as Button);

            switch((sender as Button).Name)
            {
                case "DashboardButton":
                    BlockAbout.Visibility = Visibility.Hidden;
                    IsSeleceFavoritelst = false;
                    lstRecipe.ItemsSource = MylstRecipes.Take(currentNumberEle);
                    maxnumber = (MylstRecipes.Count / currentNumberEle) + (((MylstRecipes.Count % currentNumberEle) == 0) ? 0 : 1);
                    ResetPageNumber();
                    CheckShowPageNumber();
                    break;

                case "FavoriteListButton":
                    BlockAbout.Visibility = Visibility.Hidden;
                    IsSeleceFavoritelst = true;
                    lstRecipe.ItemsSource = MylstRecipes_Favorite.Take(currentNumberEle);
                    maxnumber = (MylstRecipes_Favorite.Count / currentNumberEle) + (((MylstRecipes_Favorite.Count % currentNumberEle) == 0) ? 0 : 1);
                    ResetPageNumber();
                    CheckShowPageNumber();
                    break;

                case "AboutButton":
                    BlockAbout.Visibility = Visibility.Visible;
                    break;

            };
        }

        private void ResetPageNumber()
        {
            PageNumber.Visibility = NextButton.Visibility = moreLastText.Visibility = Visibility.Visible;
            PreButton.Visibility = moreFirstText.Visibility = Visibility.Collapsed;

            FirstPageButton.Visibility = SecondPageButton.Visibility = LastPageButton.Visibility = Visibility.Visible;
            FirstPageButton.Background = (Brush)(new BrushConverter().ConvertFromString("#FF004C1A"));
            FirstPageButton.Foreground = (Brush)(new BrushConverter().ConvertFromString("#DDFFFFFF"));

            LastPageButton.Background = SecondPageButton.Background = (Brush)(new BrushConverter().ConvertFromString("#00FFFFFF"));
            LastPageButton.Foreground = SecondPageButton.Foreground = (Brush)(new BrushConverter().ConvertFromString("#DD000000"));
        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            PackIcon FavoriteIcon = (sender as Button).Content as PackIcon;

            string color = ((lstRecipe.SelectedItem as Recipe).IsFavorite == false) ? "#FFFF0000" : "#DDFFFFFF";

            (lstRecipe.SelectedItem as Recipe).IsFavorite = (color == "#FFFF0000") ? true : false;

            FavoriteIcon.Foreground = (Brush)(new BrushConverter()).ConvertFromString(color);

            CheckAddIntoFavoriteList((lstRecipe.SelectedItem as Recipe));

        }

        private void CheckShowPageNumber()
        {
            switch (maxnumber)
            {
                case 0:
                    PageNumber.Visibility = Visibility.Hidden;
                    break;

                case 1:
                    PageNumber.Visibility = Visibility.Hidden;
                    break;

                case 2:
                    SecondPageButton.Visibility = Visibility.Collapsed;
                    LastPageButton.Content = 2;
                    moreLastText.Visibility = Visibility.Collapsed;
                    NextButton.Visibility = Visibility.Collapsed;
                    break;

                case 3:
                    moreLastText.Visibility = Visibility.Collapsed;
                    NextButton.Visibility = Visibility.Collapsed;

                    break;

                default:
                    break;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            switch (PosPageButton)
            {
                case -1:
                    ChangeColor(0);
                    PosPageButton = 0;
                    break;

                case 0:
                    UpdateNumberPage(1);
                    break;
            }

            LoadElementsWithPageNumber();
        }

        private void UpdateNumberPage(int Pos)
        {
            FirstPageButton.Content = (Convert.ToInt32(FirstPageButton.Content) + Pos).ToString();
            SecondPageButton.Content = (Convert.ToInt32(SecondPageButton.Content) + Pos).ToString();
            LastPageButton.Content = (Convert.ToInt32(LastPageButton.Content) + Pos).ToString();

            if (Convert.ToInt32(FirstPageButton.Content) != 1 && PreButton.Visibility == Visibility.Collapsed)
            {
                ((Storyboard)MainWD.FindResource("Show_PreButton")).Begin();
            }
            if (!LastPageButton.Content.Equals(maxnumber.ToString()) && NextButton.Visibility == Visibility.Collapsed)
            {
                ((Storyboard)MainWD.FindResource("Show_NextButton")).Begin();
            }


            if (SecondPageButton.Content.Equals(maxnumber.ToString()))
            {
                UpdateNumberPage(-1);

                ChangeColor(1);

                ((Storyboard)MainWD.FindResource("Hidden_NextButton")).Begin();
                PosPageButton = 1;
            }
            if (SecondPageButton.Content.Equals("1"))
            {
                UpdateNumberPage(1);

                ChangeColor(-1);

                ((Storyboard)MainWD.FindResource("Hidden_PreButton")).Begin();
                PosPageButton = -1;
            }

        }

        private void PageButton_Click(object sender, RoutedEventArgs e) {
           
            if (maxnumber > 3)
            {
                int number = 0;

                ChangeColor(0);

                PosPageButton = 0;

                switch ((sender as Button).Name)
                {
                    case "FirstPageButton":
                        number = (PosPageButton == 0) ? -1 : (PosPageButton == 1) ? -2 : 0;
                        break;

                    case "SecondPageButton":
                        number = (PosPageButton == -1) ? 1 : (PosPageButton == 1) ? -1 : 0;
                        break;

                    case "LastPageButton":
                        number = (PosPageButton == 0) ? 1 : (PosPageButton == -1) ? 2 : 0;
                        break;
                }
                UpdateNumberPage(number);
            } 
            else
            {
                switch ((sender as Button).Name)
                {
                    case "FirstPageButton":
                        ChangeColor(-1);
                        PosPageButton = -1;
                        break;

                    case "SecondPageButton":
                        ChangeColor(0);
                        PosPageButton = 0;
                        break;

                    case "LastPageButton":
                        ChangeColor(1);
                        PosPageButton = 1;
                        break;
                }
            }

            LoadElementsWithPageNumber();
        }

        private void LoadElementsWithPageNumber()
        {
            var Mylst = (IsSeleceFavoritelst) ? MylstRecipes_Favorite : MylstRecipes;

            switch (PosPageButton)
            {
                case -1:
                    lstRecipe.ItemsSource = Mylst.Skip((Convert.ToInt32(FirstPageButton.Content) - 1) * currentNumberEle).Take(currentNumberEle);
                    break;

                case 0:
                    lstRecipe.ItemsSource = Mylst.Skip((Convert.ToInt32(SecondPageButton.Content) - 1) * currentNumberEle).Take(currentNumberEle);
                    break;

                case 1:
                    lstRecipe.ItemsSource = Mylst.Skip((Convert.ToInt32(LastPageButton.Content) - 1) * currentNumberEle).Take(currentNumberEle);
                    break;
            }
        }

        private void PreButton_Click(object sender, RoutedEventArgs e)
        {
            switch (PosPageButton)
            {
                case 0:
                    UpdateNumberPage(-1);

                    break;

                case 1:
                    ChangeColor(0);
                    PosPageButton = 0;
                    
                    break;
            }

            LoadElementsWithPageNumber();
        }

        private void ChangeColor(int Pos)
        {
            switch (Pos)
            {
                case -1:
                    AnimationButton.ins.Animation_ChangeBackground(FirstPageButton, "#FF004C1A");
                    AnimationButton.ins.Animation_ChangeForeground(FirstPageButton, "#DDFFFFFF");

                    AnimationButton.ins.Animation_ChangeBackground(SecondPageButton, "#00FFFFFF");
                    AnimationButton.ins.Animation_ChangeForeground(SecondPageButton, "#DD000000");
                    AnimationButton.ins.Animation_ChangeBackground(LastPageButton, "#00FFFFFF");
                    AnimationButton.ins.Animation_ChangeForeground(LastPageButton, "#DD000000");
                    break;

                case 0:
                    AnimationButton.ins.Animation_ChangeBackground(SecondPageButton, "#FF004C1A");
                    AnimationButton.ins.Animation_ChangeForeground(SecondPageButton, "#DDFFFFFF");

                    AnimationButton.ins.Animation_ChangeBackground(FirstPageButton, "#00FFFFFF");
                    AnimationButton.ins.Animation_ChangeForeground(FirstPageButton, "#DD000000");
                    AnimationButton.ins.Animation_ChangeBackground(LastPageButton, "#00FFFFFF");
                    AnimationButton.ins.Animation_ChangeForeground(LastPageButton, "#DD000000");
                    break;

                case 1:
                    AnimationButton.ins.Animation_ChangeBackground(LastPageButton, "#FF004C1A");
                    AnimationButton.ins.Animation_ChangeForeground(LastPageButton, "#DDFFFFFF");

                    AnimationButton.ins.Animation_ChangeBackground(FirstPageButton, "#00FFFFFF");
                    AnimationButton.ins.Animation_ChangeForeground(FirstPageButton, "#DD000000");
                    AnimationButton.ins.Animation_ChangeBackground(SecondPageButton, "#00FFFFFF");
                    AnimationButton.ins.Animation_ChangeForeground(SecondPageButton, "#DD000000");
                    break;
            }
        }

        private void BlockRecipe_MouseEnter(object sender, MouseEventArgs e)
        {
            var blockrecipe = ((((sender as Grid).Children[0] as StackPanel).Children[0] as Grid).Children[1] as Grid);
            var item = (e.OriginalSource as Grid).DataContext;

            blockrecipe.Visibility = blockrecipe.Children[0].Visibility = blockrecipe.Children[1].Visibility = Visibility.Visible;

            lstRecipe.SelectedItem = item;
        }

        private void BlockRecipe_MouseLeave(object sender, MouseEventArgs e)
        {
            if ((sender as Grid).DataContext.ToString() != "{DisconnectedItem}")
            {
                var blockrecipe = ((((sender as Grid).Children[0] as StackPanel).Children[0] as Grid).Children[1] as Grid);
                var item = lstRecipe.SelectedItem as Recipe;

                blockrecipe.Children[1].Visibility = Visibility.Collapsed;

                blockrecipe.Children[0].Visibility = (!item.IsFavorite) ? Visibility.Collapsed : Visibility.Visible;
            }
          
        }

        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            SeletedButton = ((sender as Button).Parent as Grid).Children[0] as Button;
            DetailWindow window = new DetailWindow();
            window.DataContext = (((((sender as Button).Parent as Grid).Parent as Grid).Parent as StackPanel).Parent as Grid).DataContext;
            window.SetColorFavoriteButton += Window_SetColorFavoriteButton;
            window.Show();
           
        }

        private void Window_SetColorFavoriteButton(Recipe Target)
        {
            string color = (Target.IsFavorite) ? "#FFFF0000" : "#DDFFFFFF";

            ((this.SeletedButton.Content) as PackIcon).Foreground = (Brush)(new BrushConverter()).ConvertFromString(color);
            this.SeletedButton.Visibility = (!Target.IsFavorite) ? Visibility.Collapsed : Visibility.Visible;

            CheckAddIntoFavoriteList(Target);
        }

        private void AddRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            AddRecipe addRecipe = new AddRecipe();

            addRecipe.AddRecipeIntoLst += new AddRecipe.MyDelegate(target => {

                MylstRecipes.Add(RecipeDAO.GetAllInfoByID(target));

            });
            addRecipe.Closed += new EventHandler((o, f) =>
            {
                LoadElementsWithPageNumber();
            });
            addRecipe.Show();

            
        }


        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            var a = (sender as StackPanel);
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            var Imagelst = ((sender as Image).Parent as Grid).DataContext as BindingList<string>;

            (sender as Image).Source = (Imagelst.Count != 0)? (ImageSource)((new ImageSourceConverter()).ConvertFromString(Imagelst[0])): null;

            var heightBlock = (((((sender as Image).Parent as Grid).Parent as Grid).Parent as StackPanel).Children[1] as StackPanel).ActualHeight;
            var heightBlockRecipe = (int)(lstRecipe.ActualHeight / Global.MaxColOfList);
            (sender as Image).Height = heightBlockRecipe -10- heightBlock;
        }

        private void FavoriteButton_Loaded(object sender, RoutedEventArgs e)
        {
            var PackIcon = (sender as Button).Content as PackIcon;

            var IsFavorite = (sender as Button).DataContext;

            string color = (Convert.ToBoolean(IsFavorite) == false) ? "#DDFFFFFF" : "#FFFF0000";

            ((sender as Button).Parent as Grid).Visibility = (sender as Button).Visibility = (Convert.ToBoolean(IsFavorite)) ? Visibility.Visible : Visibility.Collapsed;
            ((sender as Button).Parent as Grid).Children[1].Visibility = Visibility.Collapsed;
            PackIcon.Foreground = (Brush)(new BrushConverter()).ConvertFromString(color);
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchResult.Visibility = (SearchText.Text.Length == 0) ? Visibility.Hidden : Visibility.Visible;
            CollectionViewSource.GetDefaultView(MylstRecipes).Refresh();
        }

        private void ImageRecipeSearch_Loaded(object sender, RoutedEventArgs e)
        {
            var Imagelst = ((sender as Image).Parent as Grid).DataContext as BindingList<string>;

            (sender as Image).Source = (Imagelst.Count != 0) ? (ImageSource)((new ImageSourceConverter()).ConvertFromString(Imagelst[0])) : null;
        }

        private void SearchText_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchResult.Visibility = Visibility.Hidden;
        }

        private void SearchResult_Selected(object sender, RoutedEventArgs e)
        {
            DetailWindow window = new DetailWindow();
            window.DataContext = (sender as StackPanel).DataContext;
            
            window.Closing += new CancelEventHandler((s, o) => {
                this.SearchText.Text = "";

                CheckAddIntoFavoriteList(window.DataContext as Recipe);

                LoadElementsWithPageNumber();
            });
            window.Show();
            
        }

        private void CheckAddIntoFavoriteList(Recipe Target)
        {
            //Save to DataBase
            var MyNodeXML = MyProjectXML.ins.db.SelectSingleNode($"root/Recipe [@Value='{Target.ID}']");

            MyNodeXML.SelectSingleNode("IsFavorite").Attributes["Value"].Value = (Target.IsFavorite).ToString();

            MyProjectXML.ins.Save();

            //Check add
            if (!Target.IsFavorite)
            {
                MylstRecipes_Favorite.Remove(Target);

                if (IsSeleceFavoritelst)
                {
                    lstRecipe.ItemsSource = MylstRecipes_Favorite.Take(currentNumberEle);
                    maxnumber = (MylstRecipes_Favorite.Count / currentNumberEle) + (((MylstRecipes_Favorite.Count % currentNumberEle) == 0) ? 0 : 1);
                    CheckShowPageNumber();
                }
            }
            else
            {
                MylstRecipes_Favorite.Add(Target);
            }
        }

        private void ButtonPowerOff_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonMinisize_Click(object sender, RoutedEventArgs e)
        {
            MainWD.WindowState = WindowState.Minimized;
        }

        private void MainWD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWD.DragMove();
        }
    }

}
