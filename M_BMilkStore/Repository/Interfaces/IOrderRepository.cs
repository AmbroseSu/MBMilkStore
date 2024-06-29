using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IOrderRepository
    {
        public Task<int> CreateOrderAsync(int userId, float orderTotalAmount, int? voucherId = null);
        public Task<bool> CreateOrderDetailsAsync(int orderId, List<OrderDetail> orderDetails);
    }
}
