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
        public async Task<Product> GetProductCartById(int id) => await ProductDAO.Instance.GetProductCartById(id);
        public async Task<List<Product>> GetProductByName(string name) => await ProductDAO.Instance.GetProductByName(name);

        public async Task UpdateProduct(Product product) => await ProductDAO.Instance.UpdateProduct(product);

        public async Task<PageResult<Product>> GetProductsPagedAsync(int pageIndex, int pageSize, string searchString)=> await ProductDAO.Instance.GetProductsPagedAsync(pageIndex, pageSize, searchString);

        public async Task<List<Product>> GetProductByCategoryId(int categoryId) => await ProductDAO.Instance.GetProductByCategoryId(categoryId);

        public async Task<List<Product>> GetProductByBrandId(int brandId) => await ProductDAO.Instance.GetProductByBrandId(brandId);
    } 
}
