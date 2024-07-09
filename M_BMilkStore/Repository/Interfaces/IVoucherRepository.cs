using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IVoucherRepository
    {
        Task<List<Voucher>> GetVouchersAsync(int pageNumber, int pageSize);

        Task<int> GetVoucherCountAsync();
        Task UpdateVoucherAsync(Voucher voucher);
        Task<Voucher> GetVoucherByIdAsync(int id);
        Task DeleteVoucherAsync(int voucherId);
        Task<Voucher> CreateVoucherAsync(Voucher newVoucher);

        Task<bool> IsValidVoucherAsync(string voucherCode, decimal currentCartTotal);
        Task<Voucher> GetVoucherByCodeAsync(string voucherCode);
        Task ClaimVoucherAsync(int userId, int voucherId);
        Task<bool> HasUserAlreadyClaimedVoucherAsync(int userId, int voucherId);
        Task<List<Voucher>> GetAllCurrentVouchersAsync();
    }
}
