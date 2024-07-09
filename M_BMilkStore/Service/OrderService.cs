using BussinessObject;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateOrderAsync(int userId, float orderTotalAmount, int? voucherId = null)
            => await _repository.CreateOrderAsync(userId, orderTotalAmount, voucherId);
        public async Task<bool> CreateOrderDetailsAsync(int orderId, List<OrderDetail> orderDetails)
            => await _repository.CreateOrderDetailsAsync(orderId, orderDetails);
        public Task<(List<Order>, int)> GetOrdersAsync(int pageNumber, int pageSize)
        {
            return _repository.GetOrdersAsync(pageNumber, pageSize);
        }
        public Task<Order> GetOrderByIdAsync(int orderId)
        {
            return _repository.GetOrderByIdAsync(orderId);
        }
        public Task<bool> UpdateOrderAsync(Order order)
        {
            return _repository.UpdateOrderAsync(order);
        }
        public Task<bool> SoftDeleteOrderAsync(int orderId)
        {
            return _repository.SoftDeleteOrderAsync(orderId);
        }
    }
}
