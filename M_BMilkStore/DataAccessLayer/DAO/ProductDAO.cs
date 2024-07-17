using BussinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public class ProductDAO
    {
        private static ProductDAO instance;
        private static object instanceLock = new object();

        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                }
                return instance;
            }
        }

        public async Task<List<Product>> GetAllProduct()
        {
            var listProduct = new List<Product>();
            try
            {
                using var context = new M_BMilkStoreDBContext();
                listProduct = await context.Products
                    .Where(p => p.Status == true && p.IsDeleted == false)
                    .Include(p => p.ProductBrand)
                    .Include(p => p.ProductCategory)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProduct;
        }

        public async Task<List<ProductCategory>> GetAllProductCategory()
        {
            var listProductCategory = new List<ProductCategory>();
            try
            {
                using var context = new M_BMilkStoreDBContext();
                listProductCategory = await context.ProductCategories.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProductCategory;
        }

        public async Task<List<ProductBrand>> GetAllProductBrand()
        {
            var listProductBrand = new List<ProductBrand>();
            try
            {
                using var context = new M_BMilkStoreDBContext();
                listProductBrand = await context.ProductBrands.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProductBrand;
        }

        public async Task<Product> GetProductCartById(int id)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                var product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Product> GetProductById(int id)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                return await context.Products
                    .Include(p => p.ProductBrand)
                    .Include(p => p.ProductCategory)
                    .FirstOrDefaultAsync(p => p.ProductId == id && (p.Status ?? false) && !(p.IsDeleted ?? true));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Product>> GetProductByName(string name)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                return await context.Products
                    .Where(p => p.Name.Contains(name) && (p.Status ?? false) && (!p.IsDeleted ?? true))
                    .Include(p => p.ProductBrand)
                    .Include(p => p.ProductCategory)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddProduct(Product product)
        {
            using var context = new M_BMilkStoreDBContext();
            try
            {
                await context.Products.AddAsync(product);
                product.Status = true;
                product.IsDeleted = false;
                product.ListOrderDetail = null;
                product.ListProductLine = null;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task DeleteProduct(int id)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                Product product = await context.Products.SingleOrDefaultAsync(p => p.ProductId == id);
                if (product == null)
                {
                    throw new Exception("No product found");
                }
                else
                {
                    product.IsDeleted = true;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateProduct(Product product)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                product.IsDeleted = false ;
                context.Entry(product).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PageResult<Product>> GetProductsPagedAsync(int pageIndex, int pageSize, string searchString)
        {
            using var context = new M_BMilkStoreDBContext();
            var query = context.Products.Include(x=>x.ProductBrand).Include(x=>x.ProductCategory).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(u =>
                                    u.Name.Contains(searchString) ||
                                    u.Description.Contains(searchString) ||
                                    u.ProductBrand.Name.Contains(searchString) ||
                                    u.ProductCategory.Name.Contains(searchString)
                                    
                                    
                                    );

            }

            var totalItems = await query.CountAsync();

            var products = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageResult<Product>
            {
                Items = products,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }
    }
}
