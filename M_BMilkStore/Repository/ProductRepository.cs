using BussinessObject;
using DataAccessLayer.DAO;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductRepository : IProductRepository
    {
        public async Task AddProduct(Product product) => await ProductDAO.Instance.AddProduct(product);

        public async Task DeleteProduct(int id) => await ProductDAO.Instance.DeleteProduct(id);

        public async Task<List<Product>> GetAllProduct() => await ProductDAO.Instance.GetAllProduct();
        public async Task<List<ProductCategory>> GetAllProductCategory() => await ProductDAO.Instance.GetAllProductCategory();
        public async Task<List<ProductBrand>> GetAllProductBrand() => await ProductDAO.Instance.GetAllProductBrand();

        public async Task<Product> GetProductById(int id) => await ProductDAO.Instance.GetProductById(id);
        public async Task<List<Product>> GetProductByName(string name) => await ProductDAO.Instance.GetProductByName(name);

        public async Task UpdateProduct(Product product) => await ProductDAO.Instance.UpdateProduct(product);
    }
}
