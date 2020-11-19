using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodRecipeApp.Model
{
    public class MyRandom
    {
        private static MyRandom _instance;

        public static MyRandom instance
        {
            get { 
                if (_instance == null)
                {
                    _instance = new MyRandom();
                }    
                return _instance; 
            }
        }

        private  Random _ran;

        private MyRandom()
        {
            _ran = new Random();
        }

        public int Next(int range)
        {
            int result = 0;

            result = _ran.Next(range);

            return result;
        }
    }
}
