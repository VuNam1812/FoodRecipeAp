using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FoodRecipeApp.Model
{
    public class CookingStep
    {
        public static int currentnumber = 0;

        public int StepNumber { get; set; }
        public string StepDerc { get; set; }
        public List<string> StepImages { get; set; }

        public CookingStep()
        {
            StepNumber = currentnumber+1;
            currentnumber += 1;
            StepImages = new List<string>();
        }

    }
}
