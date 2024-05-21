using AlgoStockInventory.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlgoStockInventory.Models
{
    [Table("Products")]
    public class Products
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int productId { get; set; }
        public string productName { get; set; }

        public DateTime createdDate { get; set; }

        public double price { get; set; }

        public string productDetails { get; set; }

        
    }
}
 



