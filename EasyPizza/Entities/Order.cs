using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyPizza.Entities
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment primary key
        public long Id { get; set; } 
        public float Price { get; set; }        
        public int PaymentMethod { get; set; } 
        public int PaymentStatus { get; set; }
        public int DeliveryStatus { get; set; }
        public string Note { get; set; } 
        public ICollection<OrderItem> OrderItems { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        public long UserId { get; set; }
        public long RestaurantId { get; set; }
        public User User { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
