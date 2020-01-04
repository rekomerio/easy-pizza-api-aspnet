using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyPizza.Entities
{
    public class Menu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment primary key
        public long Id { get; set; }
        public long RestaurantId { get; set; }
        public string Name { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; }
    }
}
