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
    public class VoucherRepository : IVoucherRepository
    {
        public Task<List<Voucher>> GetVouchersAsync(int pageNumber, int pageSize)
        {
            return VoucherDAO.Instance.GetVouchersAsync(pageNumber, pageSize);
        }

        public Task<int> GetVoucherCountAsync()
        {
            return VoucherDAO.Instance.GetVoucherCountAsync();
        }

        public Task UpdateVoucherAsync(Voucher voucher)
        {
            return VoucherDAO.Instance.UpdateVoucherAsync(voucher);
        }

        public Task<Voucher> GetVoucherByIdAsync(int id)
        {
            return VoucherDAO.Instance.GetVoucherByIdAsync(id);
        }

        public Task DeleteVoucherAsync(int voucherId)
        {
            return VoucherDAO.Instance.DeleteVoucherAsync(voucherId);
        }

        public Task<Voucher> CreateVoucherAsync(Voucher newVoucher)
        {
            return VoucherDAO.Instance.CreateVoucherAsync(newVoucher);
        }
        public Task<bool> IsValidVoucherAsync(string voucherCode, decimal currentCartTotal)
        {
            return VoucherDAO.Instance.IsValidVoucherAsync(voucherCode, currentCartTotal);
        }

        public Task<Voucher> GetVoucherByCodeAsync(string voucherCode)
        {
            return VoucherDAO.Instance.GetVoucherByCodeAsync(voucherCode);
        }
        public Task ClaimVoucherAsync(int userId, int voucherId)
        {
            return VoucherDAO.Instance.ClaimVoucherAsync(userId, voucherId);
        }
        public Task<bool> HasUserAlreadyClaimedVoucherAsync(int userId, int voucherId)
        {
            return VoucherDAO.Instance.HasUserAlreadyClaimedVoucherAsync(userId, voucherId);
        }
    }
}
