using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetProductById(int id);
        Task<Product> GetProductCartById(int id);
        Task<List<Product>> GetProductByName(string name);
        Task<List<Product>> GetAllProduct();
        Task<List<ProductCategory>> GetAllProductCategory();
        Task<List<ProductBrand>> GetAllProductBrand();
        Task AddProduct(Product p);
        Task UpdateProduct(Product p);
        Task DeleteProduct(int id);
        
    }
}
