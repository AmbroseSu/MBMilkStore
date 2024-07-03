using BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Service;
using Service.Interfaces;

namespace M_BMilkStoreClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IProductService _productService;
        private readonly IProductLineService iProductLineService;
        public IndexModel(ILogger<IndexModel> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
            iProductLineService = new ProductLineService();
        }

        public IList<Product> Product { get; set; } = new List<Product>();

        public async Task OnGetAsync(string searchString)
        {
            IList<Product> allProducts;
            if (!string.IsNullOrEmpty(searchString))
            {
                allProducts = await _productService.GetProductByName(searchString);
            }
            else
            {
                allProducts = await _productService.GetAllProduct();
            }
            Product = new List<Product>();

            foreach (var product in allProducts)
            {
                var productLines = await iProductLineService.GetProductLinesByProductId(product.ProductId);
                if (productLines != null && productLines.Any())
                {
                    int totalQuantity = productLines.Where(pl => pl.Status == true && pl.IsDeleted == false).Sum(pl => pl.Quantity);

                    Product.Add(new Product
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Image = product.Image,
                        ProductBrandId = product.ProductBrandId,
                        ProductCategoryId = product.ProductCategoryId,
                        ProductBrand = product.ProductBrand,
                        ProductCategory = product.ProductCategory,
                        ListProductLine = product.ListProductLine, // Giữ nguyên danh sách product line nếu cần
                        ListOrderDetail = product.ListOrderDetail, // Giữ nguyên danh sách order detail nếu cần
                        Status = product.Status,
                        IsDeleted = product.IsDeleted,

                        // Tạo một thuộc tính tạm thời để lưu total quantity tính được
                        TotalQuantity = totalQuantity
                    });
                }
            }


        }
    }
}
