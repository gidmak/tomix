using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price {get; set;}
        
        public bool IsAvailable {get; set;}
        
    }
}