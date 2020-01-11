using EasyPizza.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyPizza.Entities
{
    public enum PaymentMethod
    {
        Paypal,
        CreditCard,
        Cash,
    }

    public enum PaymentStatus
    {
        Paid,
        Pending,
        Failed,
    }

    public enum DeliveryStatus
    {
        Accepted,
        Cooking,
        Delivering,
        Delivered,
        Failed,
    }

    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment primary key
        public long Id { get; set; } 
        public float Price { get; set; }        
        public PaymentMethod PaymentMethod { get; set; } 
        public PaymentStatus PaymentStatus { get; set; } 
        public DeliveryStatus DeliveryStatus { get; set; } 
        public string Note { get; set; } 
        public ICollection<OrderItem> OrderItems { get; set; }
        public Recipient Recipient { get; set; }
        public DateTime CreatedAt { get; set; } 
        public long UserId { get; set; } 
        public long RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
