using EasyPizza.Entities;
using System;
using System.Collections.Generic;
using EasyPizza.Models.MenuItemModels;

namespace EasyPizza.Models.MenuModels
{
    public class MenuModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long RestaurantId { get; set; }
        public ICollection<MenuItemModel> MenuItems { get; set; }
    }
}
