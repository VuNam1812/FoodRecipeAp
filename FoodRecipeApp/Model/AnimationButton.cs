using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FoodRecipeApp.Model
{
    public class AnimationButton
    {
        private static AnimationButton _ins;

        public static AnimationButton ins
        {
            get {
                if (_ins == null)
                {
                    _ins = new AnimationButton();
                }
                return _ins; 
            }
           
        }


        private Button MenuButtonBefore { get; set; }
        private BrushConverter converter { get; set; }

        private AnimationButton()
        {
            MenuButtonBefore = null;
            converter = new BrushConverter();
        }

        public void Animation_ButtonClick(Button target)
        {
            Animation_LostForcus();
            MenuButtonBefore = target;
            target.Background.Opacity = 100;

        }

        public void SetButtonBefore(Button target)
        {
            MenuButtonBefore = target;
        }

        public void Animation_ChangeBackground(Button target, string color)
        {
            target.Background = (Brush) this.converter.ConvertFromString(color);
        }

        public void Animation_ChangeForeground(Button target, string color)
        {
            target.Foreground = (Brush)this.converter.ConvertFromString(color);
        }

        private void Animation_LostForcus()
        {
            if (MenuButtonBefore != null)
            {
                MenuButtonBefore.Background.Opacity = 0;
            }
            else
            {
                // do nothing;
            }

        }
    }
}
