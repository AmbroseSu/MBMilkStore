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
    public class ProductLineRepository : IProductLineRepository
    {
        public async Task<List<ProductLine>> GetProductLines() => await ProductLineDAO.Instance.GetProductLines();

        public async Task SaveProductLine(ProductLine productLine) => await ProductLineDAO.Instance.SaveProductLine(productLine);

        public async Task UpdateProductLine(ProductLine productLine) => await ProductLineDAO.Instance.UpdateProductLine(productLine);

        public async Task DeleteProductLine(ProductLine productLine) => await ProductLineDAO.Instance.DeleteProductLine(productLine);

        public async Task<ProductLine> GetProductLineById(int productLineId) => await ProductLineDAO.Instance.GetProductLineById(productLineId);

        public async Task<List<ProductLine>> GetProductLinesByProductId(int productId) => await ProductLineDAO.Instance.GetProductLinesByProductId(productId);
    }
}
