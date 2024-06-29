using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public class OrderDAO
    {
        private static OrderDAO instance;
        private static object instanceLock = new object();

        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                }
                return instance;
            }
        }

        public async Task<int> CreateOrderAsync(int userId, float orderTotalAmount, int? voucherId = null)
        {
            try
            {
                using (var context = new M_BMilkStoreDBContext())
                {
                    var order = new Order
                    {
                        UserId = userId,
                        OrderDate = DateTime.UtcNow,
                        Status = true,
                        OrderTotalAmount = orderTotalAmount,
                        VoucherId = voucherId,
                    };

                    context.Orders.Add(order);
                    await context.SaveChangesAsync();

                    return order.OrderId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Create Failed :" + ex.Message);
            }
        }

        public async Task<bool> CreateOrderDetailsAsync(int orderId, List<OrderDetail> orderDetails)
        {
            try
            {
                using (var context = new M_BMilkStoreDBContext())
                {
                    foreach (var orderDetail in orderDetails)
                    {
                        orderDetail.OrderId = orderId;
                    }

                    context.OrderDetails.AddRange(orderDetails);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
