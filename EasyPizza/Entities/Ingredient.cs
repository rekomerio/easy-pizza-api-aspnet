using System.ComponentModel.DataAnnotations.Schema;

namespace EasyPizza.Entities
{
    public class Ingredient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment primary key
        public long Id { get; set; }
        public long MenuItemId { get; set; }
        public string Name { get; set; }
    }
}
