using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BiliMusic.Models.Main;

namespace BiliMusic.Modules
{
    public class Main
    {
        public Main()
        {
            Menus = new ObservableCollection<MenuCategoryModel>() {

            };
        }
        public ObservableCollection<MenuCategoryModel> Menus { get; set; }
        


    }
}
