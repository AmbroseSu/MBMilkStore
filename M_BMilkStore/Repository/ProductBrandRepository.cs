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
    public class ProductBrandRepository : IProductBrandRepository
    {
        public async Task<List<ProductBrand>> GetProductBrands() => await ProductBrandDAO.Instance.GetProductBrands();

        public async Task SaveProductBrand(ProductBrand productBrand) => await ProductBrandDAO.Instance.SaveProductBrand(productBrand);

        public async Task UpdateProductBrand(ProductBrand productBrand) => await ProductBrandDAO.Instance.UpdateProductBrand(productBrand);

        public async Task DeleteProductBrand(ProductBrand productBrand) => await ProductBrandDAO.Instance.DeleteProductBrand(productBrand);

        public async Task<ProductBrand> GetProductBrandById(int productBrandId) => await ProductBrandDAO.Instance.GetProductBrandById(productBrandId);
    }
}
