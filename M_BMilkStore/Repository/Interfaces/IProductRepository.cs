using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProduct();
        Task<List<ProductCategory>> GetAllProductCategory();
        Task<List<ProductBrand>> GetAllProductBrand();
        Task<Product> GetProductById(int id);
        Task<Product> GetProductCartById(int id);
        Task<List<Product>> GetProductByName(string name);
        Task AddProduct(Product product);
        Task DeleteProduct(int id);
        Task UpdateProduct(Product product);

    }
}
