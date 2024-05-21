using AlgoStockInventory.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlgoStockInventory.Models
{
    [Table("StockPurchases")]
    public class StockPurchases
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int purchaseId { get; set; }
        public DateTime purchaseDate { get; set; }

        public int productId { get; set; }

        public int quantity { get; set; }

        public string suplierName { get; set; }
        public double invoiceAmount { get; set; }
        public string invoiceNo { get; set; }

        
    }
    













}




