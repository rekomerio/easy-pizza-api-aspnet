using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyPizza.Models.IngredientModels;

namespace EasyPizza.Models.MenuItemModels
{
    public class MenuItemModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public ICollection<IngredientModel> Ingredients { get; set; }
    }
}
