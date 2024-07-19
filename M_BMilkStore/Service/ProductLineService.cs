using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject;
using Repository;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service
{
    public class ProductLineService : IProductLineService
    {
        private readonly IProductLineRepository iProductLineRepository;

        public ProductLineService()
        {
            iProductLineRepository = new ProductLineRepository();
        }

        public async Task<List<ProductLine>> GetProductLines()
        {
            return await iProductLineRepository.GetProductLines();
        }

        public async Task SaveProductLine(ProductLine productLine)
        {
            await iProductLineRepository.SaveProductLine(productLine);
        }

        public async Task UpdateProductLine(ProductLine productLine)
        {
            await iProductLineRepository.UpdateProductLine(productLine);
        }

        public async Task DeleteProductLine(ProductLine productLine)
        {
            await iProductLineRepository.DeleteProductLine(productLine);
        }

        public async Task<ProductLine> GetProductLineById(int productLineId)
        {
            return await iProductLineRepository.GetProductLineById(productLineId);
        }

        public async Task<List<ProductLine>> GetProductLinesByProductId(int productId)
        {
            return await iProductLineRepository.GetProductLinesByProductId(productId);
        }

        public async Task<int> GetRemainingQuantityByProductId(int productId)
        {
            return await iProductLineRepository.GetRemainingQuantityByProductId(productId);
        }
    }
}
