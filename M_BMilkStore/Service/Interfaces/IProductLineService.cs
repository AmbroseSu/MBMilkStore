using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IProductLineService
    {
        Task<List<ProductLine>> GetProductLines();

        Task SaveProductLine(ProductLine productLine);

        Task UpdateProductLine(ProductLine productLine);

        Task DeleteProductLine(ProductLine productLine);

        Task<ProductLine> GetProductLineById(int productLineId);

        Task<List<ProductLine>> GetProductLinesByProductId(int productId);
    }
}
