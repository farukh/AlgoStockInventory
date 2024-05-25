using AlgoStockInventory.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Diagnostics.Eventing.Reader;

namespace AlgoStockInventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockInventoryController : ControllerBase
    {
        private readonly AlgoStockDbContext _context;
        public StockInventoryController(AlgoStockDbContext context)
        {
            _context = context;
        }

        #region Purchase API
        [HttpPost("CreateNewPurchase")]
        public ApiResponse CreateNewPurchase([FromBody] StockPurchases obj)
        {
            ApiResponse _res = new ApiResponse();
            if (!ModelState.IsValid)
            {
                _res.Result = false;
                _res.Message = "Validateion Error";
                return _res;
            }

            try
            {
                var isExist = _context.StockPurchases.SingleOrDefault(m => m.invoiceNo.ToLower() == obj.invoiceNo.ToLower());
                if (isExist == null)
                {
                    _context.StockPurchases.Add(obj);
                    _context.SaveChanges();
                    var isStockExist = _context.Stocks.SingleOrDefault(m => m.productId == obj.productId);
                    if (isStockExist == null)
                    {
                        Stocks _stocks = new Stocks()
                        {
                            createdDate = DateTime.Now,
                            lastModifiedDate = DateTime.Now,
                            productId = obj.productId,
                            quantity = obj.quantity,
                        };
                        _context.Stocks.Add(_stocks);
                        _context.SaveChanges();
                    }
                    else
                    {
                        isStockExist.quantity += obj.quantity;
                        isStockExist.lastModifiedDate = DateTime.Now;
                        _context.SaveChanges();
                    }


                    _res.Result = true;
                    _res.Message = "Purchase Entry Created Successfully";
                }
                else
                {
                    _res.Result = false;
                    _res.Message = "Invoice No Already Exists.";

                }
                return _res;

            }
            catch (Exception ex)
            {
                _res.Result = false;
                _res.Message = ex.Message;
                return _res;
            }
        }

        [HttpGet("GetAllPurchase")]
        public ApiResponse GetAllPurchase()
        {
            ApiResponse _res = new ApiResponse();
            try
            {
                var all = (from purchase in _context.StockPurchases
                           join product in _context.Products on purchase.productId equals product.productId
                           select new
                           {
                               invoiceAmount = purchase.invoiceAmount,
                               invoiceNo = purchase.invoiceNo,
                               productId = purchase.productId,
                               purchaseDate = purchase.purchaseDate,
                               purchaseId = purchase.purchaseId,
                               supplierName = purchase.suplierName,
                               productName = product.productName,
                           }).OrderByDescending(m => m.purchaseId).ToList();
                _res.Result = true;
                _res.Data = all;
                return _res;
            }
            catch (Exception ex)
            {
                _res.Result = false;
                _res.Message = ex.Message;
                return _res;
            }

        }
        #endregion



        #region Sale API
        [HttpPost("CreateNewSale")]
        public ApiResponse CreateNewSale([FromBody] StockSale obj)
        {
            ApiResponse _res = new ApiResponse();
            if (!ModelState.IsValid)
            {
                _res.Result = false;
                _res.Message = "Validateion Error";
                return _res;
            }

            try
            {
                var isInvoiceExist = _context.StockSale.SingleOrDefault(m => m.invoiceNo.ToLower() == obj.invoiceNo.ToLower());
                var isStockExist = _context.Stocks.SingleOrDefault(m => m.productId == obj.productId);

                if (isInvoiceExist == null && isStockExist != null)
                {
                    _context.StockSale.Add(obj);
                    _context.SaveChanges();

                    isStockExist.quantity -= obj.quantity;
                    isStockExist.lastModifiedDate = DateTime.Now;
                    _context.SaveChanges();

                    _res.Result = true;
                    _res.Message = "Sales Entry Created Successfully";
                }
                else
                {
                    _res.Result = false;
                    if (isInvoiceExist != null)
                        _res.Message = "Invoice No Already Exists.";
                    else
                        _res.Message = "Stock Not Available";


                }
                return _res;

            }
            catch (Exception ex)
            {
                _res.Result = false;
                _res.Message = ex.Message;
                return _res;
            }
        }

        [HttpGet("GetAllSale")]
        public ApiResponse GetAllSale()
        {
            ApiResponse _res = new ApiResponse();
            try
            {
                var all = (from sale in _context.StockSale
                           join product in _context.Products on sale.productId equals product.productId
                           select new
                           {
                               mobileNo = sale.mobileNo,
                               invoiceNo = sale.invoiceNo,
                               productId = sale.productId,
                               quantity = sale.quantity,
                               saleDate = sale.saleDate,
                               totalAmount = sale.totalAmount,
                               saleId = sale.saleId,
                               productName = product.productName,
                               customerName = sale.customerName

                           }).OrderByDescending(m => m.saleId).ToList();
                _res.Result = true;
                _res.Data = all;
                return _res;
            }
            catch (Exception ex)
            {
                _res.Result = false;
                _res.Message = ex.Message;
                return _res;
            }

        }
        #endregion

        #region Products API
        [HttpPost("CreateNewProduct")]
        public ApiResponse CreateNewProduct([FromBody] Products obj)
        {
            ApiResponse _res = new ApiResponse();
            if (!ModelState.IsValid)
            {
                _res.Result = false;
                _res.Message = "Validateion Error";
                return _res;
            }

            try
            {
                var isProductExist = _context.Products.SingleOrDefault(m => m.productName.ToLower() == obj.productName.ToLower());

                if (isProductExist == null)
                {
                    _context.Products.Add(obj);
                    _context.SaveChanges();
 
                    _res.Result = true;
                    _res.Data = obj;
                    _res.Message = "Product Entry Created Successfully";
                }
                else
                {
                    _res.Result = false;
                    _res.Message = " Product With Same Name Already Exists.";

                }
                return _res;

            }
            catch (Exception ex)
            {
                _res.Result = false;
                _res.Message = ex.Message;
                return _res;
            }
        }

        [HttpGet("GetAllProducts")]
        public ApiResponse GetAllProducts()
        {
            ApiResponse _res = new ApiResponse();
            try
            {
                var all = _context.Products.ToList();
                _res.Result = true;
                _res.Data = all;
                return _res;
            }
            catch (Exception ex)
            {
                _res.Result = false;
                _res.Message = ex.Message;
                return _res;
            }

        }
       

        [HttpPatch("EditProduct")]
        public ApiResponse EditProduct([FromBody] Products obj) 
        {
            ApiResponse _res = new ApiResponse();
            if (!ModelState.IsValid)
            {
                _res.Result = false;
                _res.Message = "Validateion Error";
                return _res;
            }
            try
            {
                 var isProductExist = _context.Products.SingleOrDefault(m => m.productId == obj.productId);

                if (isProductExist!= null)
                {
                    isProductExist.productName = obj.productName;
                    isProductExist.productDetails = obj.productDetails;
                    isProductExist.price = obj.price;
                    _context.SaveChanges();

                    _res.Result = true;
                    _res.Message = "Product Edit Details Saved Successfully";
                }
                else
                {
                    _res.Result = false;
                    _res.Message = "Product with ID = "+obj.productId +"do not exit.";


                }
                return _res;

            }
            catch (Exception ex)
            {
                _res.Result = false;
                _res.Message = ex.Message;
                return _res;
            }

        }

        [HttpDelete("DeleteProduct")]
        public ApiResponse DeleteProduct(int id)
        {
            ApiResponse _res = new ApiResponse();
            if (id == 0)
            {
                _res.Result = false;
                _res.Message = "Product ID Required";
                return _res;
            }
            try
            {
                var isProductExist = _context.Products.SingleOrDefault(m => m.productId == id);
                var isStocksExist = _context.Stocks.SingleOrDefault(m => m.productId == id);
                //var isStockSaleExist = _context.StockSale.SingleOrDefault(m => m.productId == obj.productId);
                //var isStockPurchaseExist = _context.StockPurchases.SingleOrDefault(m => m.productId == obj.productId);

                if(isStocksExist !=null)
                {
                    _res.Result = false;
                    _res.Message = $"Product cannot be deleted as product with ID = {id} have Stock Quantity {isStocksExist.quantity}.";
                }

               else if (isProductExist != null)
                {
                    _context.Products.Remove(isProductExist);
                    _context.SaveChanges();
                    _res.Result = true;
                    _res.Message = "Product Removed Successfully";
                }
                else
                {
                    _res.Result = false;
                    _res.Message = "Product with ID = " + id+ "do not exit.";


                }
                return _res;

            }
            catch (Exception ex)
            {
                _res.Result = false;
                _res.Message = ex.Message;
                return _res;
            }

        }
        #endregion
        #region Stock API

        [HttpGet("GetAllStock")]
        public ApiResponse GetAllStock()
        {
            ApiResponse _res = new ApiResponse();
            try
            {
                var all = (from stock in _context.Stocks
                           join product in _context.Products on stock.productId equals product.productId
                           select new
                           {
                               stockId = stock.stockId,
                               createdDate = stock.createdDate,
                               lastModifiedDate = stock.lastModifiedDate,
                               productId = stock.productId,
                               quantity = stock.quantity,
                               productName = product.productName,
                           }).OrderByDescending(m => m.stockId).ToList();
                _res.Result = true;
                _res.Data = all;
                return _res;
            }
            catch (Exception ex)
            {
                _res.Result = false;
                _res.Message = ex.Message;
                return _res;
            }

        }
        [HttpGet("CheckStockByProductId")]
        public ApiResponse CheckStockByProductId(int productId)
        {
            ApiResponse _res = new ApiResponse();
            try
            {
                var stock = _context.Stocks.SingleOrDefault(s => s.productId == productId);
                if (stock != null)
                {
                    if (stock.quantity != 0)
                    {
                        _res.Result = true;
                        _res.Data = stock;
                        _res.Message = "Stock Available";
                    }
                    else
                    {
                        _res.Result = false;
                        _res.Message = "Stock Not Available";

                    }

                }
                else
                {
                    _res.Result = false;
                    _res.Message = "Stock Not Available";

                }

                return _res;
            }
            catch (Exception ex)
            {
                _res.Result = false;
                _res.Message = ex.Message;
                return _res;
            }

        }
        #endregion
    }
}
