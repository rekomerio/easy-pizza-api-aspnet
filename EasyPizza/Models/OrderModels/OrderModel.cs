using EasyPizza.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPizza.Models.OrderModels
{
    public class OrderModel
    {
            public long Id { get; set; }
            public float Price { get; set; }        // Calculate in backend from items
            public PaymentMethod PaymentMethod { get; set; }
            public PaymentStatus PaymentStatus { get; set; } // Set in backend
            public OrderStatus OrderStatus { get; set; } // Set in backend
            public OrderType OrderType { get; set; }
            public string Note { get; set; }
            public ICollection<OrderItem> OrderItems { get; set; }
            public Recipient Recipient { get; set; }
            public DateTime CreatedAt { get; set; }
            public long UserId { get; set; }
            public long RestaurantId { get; set; }
            public Restaurant Restaurant { get; set; }
        
    }
}
