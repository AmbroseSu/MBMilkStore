using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task<List<ProductCategory>> GetProductCategories();

        Task SaveProductCategory(ProductCategory productCategory);

        Task UpdateProductCategory(ProductCategory productCategory);

        Task DeleteProductCategory(ProductCategory productCategory);

        Task<ProductCategory> GetProductCategoryById(int productCategoryId);

    }
}
