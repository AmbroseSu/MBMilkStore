using BussinessObject;
using Microsoft.VisualBasic;
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
    public class ProductBrandService : IProductBrandService
    {
        private readonly IProductBrandRepository iProductBrandRepository;

        public ProductBrandService()
        {
            iProductBrandRepository = new ProductBrandRepository();
        }

        public async Task<List<ProductBrand>> GetProductBrands()
        {
            return await iProductBrandRepository.GetProductBrands();
        }

        public async Task SaveProductBrand(ProductBrand productBrand)
        {
            await iProductBrandRepository.SaveProductBrand(productBrand);
        }

        public async Task UpdateProductBrand(ProductBrand productBrand)
        {
            await iProductBrandRepository.UpdateProductBrand(productBrand);
        }

        public async Task DeleteProductBrand(ProductBrand productBrand)
        {
            await iProductBrandRepository.DeleteProductBrand(productBrand);
        }

        public async Task<ProductBrand> GetProductBrandById(int productBrandId)
        {
            return await iProductBrandRepository.GetProductBrandById(productBrandId);
        }

    }
}
