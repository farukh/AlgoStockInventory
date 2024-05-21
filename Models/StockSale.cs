using AlgoStockInventory.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlgoStockInventory.Models
{
    [Table("StockSale")]
    public class StockSale
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int saleId { get; set; }
        public string invoiceNo { get; set; }

        public string customerName { get; set; }

        public string mobileNo { get; set; }

        public DateTime saleDate { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public double totalAmount { get; set; }

        
    }
    























}




