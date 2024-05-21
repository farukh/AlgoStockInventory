using AlgoStockInventory.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlgoStockInventory.Models
{
    [Table("Stocks")]
    public class Stocks
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int stockId { get; set; }
        public int productId { get; set; }

        public int quantity { get; set; }

        public DateTime createdDate { get; set; }

        public DateTime lastModifiedDate { get; set; }

        
    }
     





}




