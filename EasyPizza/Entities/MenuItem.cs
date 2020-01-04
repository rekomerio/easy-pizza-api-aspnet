using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyPizza.Entities
{
    public class MenuItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment primary key
        public long Id { get; set; }
        public long MenuId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
