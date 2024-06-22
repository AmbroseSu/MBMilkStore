﻿using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IProductBrandRepository
    {
        Task<List<ProductBrand>> GetProductBrands();

        Task SaveProductBrand(ProductBrand productBrand);

        Task UpdateProductBrand(ProductBrand productBrand);

        Task DeleteProductBrand(ProductBrand productBrand);

        Task<ProductBrand> GetProductBrandById(int productBrandId);
    }
}
