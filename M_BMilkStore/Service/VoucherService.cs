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
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherService(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public Task<List<Voucher>> GetVouchersAsync(int pageNumber, int pageSize)
        {
            return _voucherRepository.GetVouchersAsync(pageNumber, pageSize);
        }

        public Task<int> GetVoucherCountAsync()
        {
            return _voucherRepository.GetVoucherCountAsync();
        }

        public Task UpdateVoucherAsync(Voucher voucher)
        {
            return _voucherRepository.UpdateVoucherAsync(voucher);
        }

        public Task<Voucher> GetVoucherByIdAsync(int id)
        {
            return _voucherRepository.GetVoucherByIdAsync(id);
        }
        public Task DeleteVoucherAsync(int voucherId)
        {
            return _voucherRepository.DeleteVoucherAsync(voucherId);
        }
        public Task<Voucher> CreateVoucherAsync(Voucher newVoucher)
        {
            return _voucherRepository.CreateVoucherAsync(newVoucher);
        }
        public Task<bool> IsValidVoucherAsync(string voucherCode, decimal currentCartTotal)
        {
            return _voucherRepository.IsValidVoucherAsync(voucherCode, currentCartTotal);
        }
        public Task<Voucher> GetVoucherByCodeAsync(string voucherCode)
        {
            return _voucherRepository.GetVoucherByCodeAsync(voucherCode);
        }
        public Task ClaimVoucherAsync(int userId, int voucherId)
        {
            return _voucherRepository.ClaimVoucherAsync(userId, voucherId);
        }
        public Task<bool> HasUserAlreadyClaimedVoucherAsync(int userId, int voucherId)
        {
            return _voucherRepository.HasUserAlreadyClaimedVoucherAsync(userId, voucherId);
        }
    }
}
