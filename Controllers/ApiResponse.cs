namespace AlgoStockInventory.Controllers
{
    public class ApiResponse
    {
        public bool Result { get; set; }
        public Object Data { get; set; }
        public string? Message { get; set; }
    }
}