using BussinessObject;
using Repository.Interfaces;
using Repository;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository iProductRepository;

        public ProductService()
        {
            iProductRepository = new ProductRepository();
        }
        public async Task AddProduct(Product product)
        {
            await iProductRepository.AddProduct(product);
        }

        public async Task DeleteProduct(int id)
        {
            await iProductRepository.DeleteProduct(id);
        }

        public async Task<List<Product>> GetAllProduct()
        {
            return await iProductRepository.GetAllProduct();
        }

        public async Task<List<ProductBrand>> GetAllProductBrand()
        {
            return await iProductRepository.GetAllProductBrand();
        }

        public async Task<List<ProductCategory>> GetAllProductCategory()
        {
            return await iProductRepository.GetAllProductCategory();  
        }

        public async Task<Product> GetProductById(int id)
        {
            return await iProductRepository.GetProductById(id);
        }

        public async Task<Product> GetProductCartById(int id) => await iProductRepository.GetProductCartById(id);
        public async Task<List<Product>> GetProductByName(string name)
        {
            return await iProductRepository.GetProductByName(name);
        }
        public async Task UpdateProduct(Product product)
        {
            await iProductRepository.UpdateProduct(product);
        }
    }
}
