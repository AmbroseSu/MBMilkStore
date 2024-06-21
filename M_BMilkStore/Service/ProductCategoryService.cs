using BussinessObject;
using Repository;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository iProductCategoryRepository;

        public ProductCategoryService()
        {
            iProductCategoryRepository = new ProductCategoryRepository();
        }

        public async Task<List<ProductCategory>> GetProductCategories()
        {
            return await iProductCategoryRepository.GetProductCategories();
        }

        public async Task SaveProductCategory(ProductCategory productCategory)
        {
            await iProductCategoryRepository.SaveProductCategory(productCategory);
        }

        public async Task UpdateProductCategory(ProductCategory productCategory)
        {
            await iProductCategoryRepository.UpdateProductCategory(productCategory);
        }

        public async Task DeleteProductCategory(ProductCategory productCategory)
        {
            await iProductCategoryRepository.DeleteProductCategory(productCategory);
        }

        public async Task<ProductCategory> GetProductCategoryById(int productCategoryId)
        {
            return await iProductCategoryRepository.GetProductCategoryById(productCategoryId);
        }

    }
}
