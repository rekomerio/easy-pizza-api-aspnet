using EasyPizza.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        Pending,
        Paid,
        Failed,
    }

    public enum OrderStatus
    {
        Pending,
        Accepted,
        Cooking,
        Delivering,
        Delivered,
        Failed,
    }

    public enum OrderType
    {
        Delivery,
        Pickup
    }

    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment primary key
        public long Id { get; set; }
        [Required]
        public float Price { get; set; }        // Calculate in backend from items
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public PaymentStatus PaymentStatus { get; set; } // Set in backend
        [Required]
        public OrderStatus OrderStatus { get; set; } // Set in backend
        [Required]
        public OrderType OrderType { get; set; }
        public string Note { get; set; }
        [Required]
        public ICollection<OrderItem> OrderItems { get; set; }
        [Required]
        public Recipient Recipient { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public long RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
