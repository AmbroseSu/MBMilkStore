using BussinessObject;
using DataAccessLayer;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        public async Task<List<ProductCategory>> GetProductCategories() => await ProductCategoryDAO.Instance.GetProductCategories();

        public async Task SaveProductCategory(ProductCategory productCategory) => await ProductCategoryDAO.Instance.SaveProductCategory(productCategory);

        public async Task UpdateProductCategory(ProductCategory productCategory) => await ProductCategoryDAO.Instance.UpdateProductCategory(productCategory);

        public async Task DeleteProductCategory(ProductCategory productCategory) => await ProductCategoryDAO.Instance.DeleteProductCategory(productCategory);

        public async Task<ProductCategory> GetProductCategoryById(int productCategoryId) => await ProductCategoryDAO.Instance.GetProductCategoryById(productCategoryId);


    }
}
