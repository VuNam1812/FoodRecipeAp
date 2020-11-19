using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FoodRecipeApp.Model
{

    public class MyProjectXML
    {
        private static MyProjectXML _ins;

        public static MyProjectXML ins
        {
            get { 
                if (_ins == null)
                {
                    _ins = new MyProjectXML();
                }
                return _ins; }
        }


        private string Path = AppDomain.CurrentDomain.BaseDirectory + "Data/ProjectDB.xml";

        public XmlDocument db { get; set; }

        private MyProjectXML()
        {
            db = new XmlDocument();
            db.Load(Path);
        }

        public void Save()
        {
            db.Save(Path);
        }
    }


}
