using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject;

namespace Repository.Interfaces
{
    public interface IProductLineRepository
    {
        Task<List<ProductLine>> GetProductLines();

        Task SaveProductLine(ProductLine productLine);

        Task UpdateProductLine(ProductLine productLine);

        Task DeleteProductLine(ProductLine productLine);

        Task<ProductLine> GetProductLineById(int productLineId);

        Task<List<ProductLine>> GetProductLinesByProductId(int productId);
        Task<int> GetRemainingQuantityByProductId(int productId);
    }
}
