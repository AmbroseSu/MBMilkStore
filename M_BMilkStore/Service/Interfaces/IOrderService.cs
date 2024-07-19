using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IOrderService
    {
        public Task<int> CreateOrderAsync(int userId, float orderTotalAmount, int? voucherId = null);

        public Task<bool> CreateOrderDetailsAsync(int orderId, List<OrderDetail> orderDetails);
        Task<(List<Order>, int)> GetOrdersAsync(int pageNumber, int pageSize);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> SoftDeleteOrderAsync(int orderId);
        Task<List<Order>> GetOrderHistoryByUserIdAsync(int userId);
        Task<bool> RequestRefundAsync(int orderId, string refundMessage);
    }
}
