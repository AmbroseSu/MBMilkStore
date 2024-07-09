using BussinessObject;
using DataAccessLayer.DAO;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        public async Task<int> CreateOrderAsync(int userId, float orderTotalAmount, int? voucherId = null)
           => await OrderDAO.Instance.CreateOrderAsync(userId, orderTotalAmount, voucherId);

        public async Task<bool> CreateOrderDetailsAsync(int orderId, List<OrderDetail> orderDetails) 
            => await OrderDAO.Instance.CreateOrderDetailsAsync(orderId, orderDetails);
        public Task<(List<Order>, int)> GetOrdersAsync(int pageNumber, int pageSize)
        {
            return OrderDAO.Instance.GetOrdersAsync(pageNumber, pageSize);
        }

        public Task<Order> GetOrderByIdAsync(int orderId)
        {
            return OrderDAO.Instance.GetOrderByIdAsync(orderId);
        }
        public Task<bool> UpdateOrderAsync(Order order)
        {
            return OrderDAO.Instance.UpdateOrderAsync(order);
        }
        public Task<bool> SoftDeleteOrderAsync(int orderId)
        {
            return OrderDAO.Instance.SoftDeleteOrderAsync(orderId);
        }
    }
}
