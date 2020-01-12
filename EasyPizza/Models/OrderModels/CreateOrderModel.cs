using EasyPizza.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace EasyPizza.Models.OrderModels
{
    public class CreateOrderModel
    {
        public PaymentMethod PaymentMethod { get; set; }
        public OrderType OrderType { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public long RestaurantId { get; set; }
        public Recipient Recipient { get; set; }
        public string Note { get; set; }
    }
}
