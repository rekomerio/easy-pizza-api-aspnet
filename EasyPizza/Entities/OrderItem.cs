using System.ComponentModel.DataAnnotations.Schema;

namespace EasyPizza.Entities
{
    public class OrderItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment primary key
        public long Id { get; set; }
        public int Quantity { get; set; }
        public long OrderId { get; set; }
        public long MenuItemId { get; set; }
    }
}