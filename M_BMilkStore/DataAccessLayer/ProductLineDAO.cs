using BussinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ProductLineDAO
    {
        private static ProductLineDAO instance;
        private static object instanceLock = new object();

        public static ProductLineDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductLineDAO();
                    }
                }
                return instance;
            }
        }


        public async Task<List<ProductLine>> GetProductLines()
        {
            var listProductLines = new List<ProductLine>();
            try
            {

                using var context = new M_BMilkStoreDBContext();
                listProductLines = context.ProductLines
                    .Include(pl => pl.Product)
                    .ToList();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProductLines;
        }

        public async Task SaveProductLine(ProductLine productLine)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                context.ProductLines.Add(productLine);
                productLine.Status = true;
                productLine.IsDeleted = false;
                context.SaveChanges();


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateProductLine(ProductLine productLine)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                context.Entry<ProductLine>(productLine).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteProductLine(ProductLine productLine)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                var proli = context.ProductLines.SingleOrDefault(proline => proline.ProductLineId == productLine.ProductLineId);
                if (proli == null)
                {
                    throw new Exception("Product Line not exist.");
                }
                else
                {
                    context.ProductLines.Remove(proli);
                    context.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductLine> GetProductLineById(int productlineId)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                return context.ProductLines.FirstOrDefault(prli => prli.ProductLineId.Equals(productlineId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<ProductLine>> GetProductLinesByProductId(int productId)
        {
            var listProductLines = new List<ProductLine>();
            try
            {
                using var context = new M_BMilkStoreDBContext();
                return context.ProductLines.Where(proli => proli.ProductId.Equals(productId) && proli.ExpiredDate >= DateTime.Now && proli.Status == true && proli.IsDeleted == false)
                    .OrderBy(proli => proli.ExpiredDate).Include(pl => pl.Product).ToList();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }




    }
}
