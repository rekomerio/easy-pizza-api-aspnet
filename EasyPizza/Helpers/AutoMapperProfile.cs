using AutoMapper;
using EasyPizza.Entities;
using EasyPizza.Models.UserModels;
using EasyPizza.Models.RestaurantModels;
using EasyPizza.Models.MenuModels;
using EasyPizza.Models.MenuItemModels;
using EasyPizza.Models.IngredientModels;
using EasyPizza.Models.OrderModels;

namespace EasyPizza.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            /* <From, To> */
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UserUpdateModel, User>();

            CreateMap<Restaurant, RestaurantModel>();
            CreateMap<CreateModel, Restaurant>();
            CreateMap<UpdateModel, Restaurant>();

            CreateMap<Menu, MenuModel>();

            CreateMap<MenuItem, MenuItemModel>();

            CreateMap<Ingredient, IngredientModel>();

            CreateMap<Order, OrderModel>();
            CreateMap<CreateOrderModel, Order> ();
        }
    }
}

