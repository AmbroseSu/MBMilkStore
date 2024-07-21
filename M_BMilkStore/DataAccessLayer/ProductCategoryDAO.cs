using BussinessObject;
using DataAccessLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ProductCategoryDAO
    {
        private static ProductCategoryDAO instance;
        private static object instanceLock = new object();

        public static ProductCategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductCategoryDAO();
                    }
                }
                return instance;
            }
        }


        public async Task<List<ProductCategory>> GetProductCategories()
        {
            var listProductCategories = new List<ProductCategory>();
            try
            {
                
                using var context = new M_BMilkStoreDBContext();
                listProductCategories = context.ProductCategories.Where(c => c.Status == true).ToList();
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProductCategories;
        }

        public async Task SaveProductCategory(ProductCategory productCategory)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                productCategory.Status = true;
                context.ProductCategories.Add(productCategory);
                context.SaveChanges();
                

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateProductCategory(ProductCategory productCategory)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                context.Entry<ProductCategory>(productCategory).State
                    =Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteProductCategory(ProductCategory productCategory)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                var proca = context.ProductCategories.SingleOrDefault(procate => procate.ProductCategoryId == productCategory.ProductCategoryId);
                if(proca == null)
                {
                    throw new Exception("Product Category not exist.");
                }
                else
                {
                    proca.Status = false;
                    context.Entry<ProductCategory>(proca).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
                

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductCategory> GetProductCategoryById(int productCategoryId)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                return context.ProductCategories.FirstOrDefault(prca => prca.ProductCategoryId.Equals(productCategoryId) && prca.Status == true);
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }



    }
}
