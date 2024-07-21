using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ProductBrandDAO
    {

        private static ProductBrandDAO instance;
        private static object instanceLock = new object();

        public static ProductBrandDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductBrandDAO();
                    }
                }
                return instance;
            }
        }

        public async Task<List<ProductBrand>> GetProductBrands()
        {
            var listProductBrands = new List<ProductBrand>();
            try
            {
                using var context = new M_BMilkStoreDBContext();
                listProductBrands = context.ProductBrands.Where(c => c.Status == true).ToList();

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProductBrands;
        }

        public async Task SaveProductBrand(ProductBrand productBrand)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                productBrand.Status = true;
                context.ProductBrands.Add(productBrand);
                context.SaveChanges();

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateProductBrand(ProductBrand productBrand)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                context.Entry<ProductBrand>(productBrand).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
                 
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteProductBrand(ProductBrand productBrand)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                var probr = context.ProductBrands.SingleOrDefault(probra => probra.ProductBrandId == productBrand.ProductBrandId);
                if (probr == null)
                {
                    throw new Exception("Product Brand not exist.");
                }
                else
                {
                    probr.Status = false;
                    context.Entry<ProductBrand>(probr).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
                

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public async Task<ProductBrand> GetProductBrandById(int productBrandId)
        {
            using var context = new M_BMilkStoreDBContext();
            return context.ProductBrands.FirstOrDefault(probr => probr.ProductBrandId.Equals(productBrandId) && probr.Status == true);
        }


    }
}
