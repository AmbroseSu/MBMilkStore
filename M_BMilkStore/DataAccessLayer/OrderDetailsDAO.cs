using BussinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class OrderDetailsDAO
    {
        private static OrderDetailsDAO instance;
        private static object instanceLock = new object();

        public static OrderDetailsDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailsDAO();
                    }
                }
                return instance;
            }
        }

        public async Task<List<OrderDetail>> GetOrderDetails()
        {
            var listOrderDetails = new List<OrderDetail>();
            try
            {

                using var context = new M_BMilkStoreDBContext();
                listOrderDetails = context.OrderDetails
                    .Include(pl => pl.Product)
                    .Include(pl => pl.Order)
                    .ToList();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listOrderDetails;
        }




    }
}
