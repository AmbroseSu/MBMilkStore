using BussinessObject;
using Microsoft.EntityFrameworkCore;
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
                        Status = false,
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

        public async Task<(List<Order>, int)> GetOrdersAsync(int pageNumber, int pageSize)
        {
            try
            {
                using (var context = new M_BMilkStoreDBContext())
                {
                    var totalOrders = await context.Orders.CountAsync();
                    var orders = await context.Orders
                                               .Where(o => !o.isDeleted)
                                              .Include(o => o.User)
                                              .Include(o => o.Voucher)
                                              .Include(o => o.ListOrderDetail)
                                              .OrderByDescending(o => o.OrderDate)
                                              .Skip((pageNumber - 1) * pageSize)
                                              .Take(pageSize)
                                              .ToListAsync();
                    return (orders, totalOrders);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve orders: " + ex.Message);
            }
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            try
            {
                using (var context = new M_BMilkStoreDBContext())
                {
                    return await context.Orders
                                        .Include(o => o.User)
                                        .Include(o => o.Voucher)
                                        .Include(o => o.ListOrderDetail)
                                            .ThenInclude(od => od.Product)
                                        .FirstOrDefaultAsync(o => o.OrderId == orderId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve order: " + ex.Message);
            }
        }


        public async Task<bool> UpdateOrderAsync(Order order)
        {
            try
            {
                using (var context = new M_BMilkStoreDBContext())
                {
                    var existingOrder = await context.Orders.FindAsync(order.OrderId);
                    if (existingOrder == null)
                    {
                        throw new Exception("Order not found.");
                    }

                    existingOrder.Status = order.Status;
                    existingOrder.OrderTotalAmount = order.OrderTotalAmount;
                    existingOrder.VoucherId = order.VoucherId; // Ensure this line is present
                    context.Orders.Update(existingOrder);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Update Failed: " + ex.Message);
            }
        }

        public async Task<bool> SoftDeleteOrderAsync(int orderId)
        {
            try
            {
                using (var context = new M_BMilkStoreDBContext())
                {
                    var order = await context.Orders.FindAsync(orderId);
                    if (order == null)
                    {
                        throw new Exception("Order not found.");
                    }

                    order.isDeleted = true;
                    context.Orders.Update(order);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Soft delete failed: " + ex.Message);
            }
        }

        public async Task<List<Order>> GetOrderHistoryByUserIdAsync(int userId)
        {
            try
            {
                using (var context = new M_BMilkStoreDBContext())
                {
                    return await context.Orders
                                        .Where(o => o.UserId == userId)
                                        .Include(o => o.User)
                                        .Include(o => o.Voucher)
                                        .Include(o => o.ListOrderDetail)
                                            .ThenInclude(od => od.Product)
                                        .OrderByDescending(o => o.OrderDate)
                                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve order history: " + ex.Message);
            }
        }

    }
}
