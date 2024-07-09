using BussinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public class VoucherDAO
    {
        private static VoucherDAO instance;
        private static object instanceLock = new object();

        public static VoucherDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new VoucherDAO();
                    }
                }
                return instance;
            }
        }
        public async Task<List<Voucher>> GetVouchersAsync(int pageNumber, int pageSize)
        {
            using (var context = new M_BMilkStoreDBContext())
            {
                return await context.Vouchers
                                     .Where(v => !v.IsDeleted)
                                     .OrderByDescending(v => v.CreationDate)
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();
            };
        }

        public async Task<int> GetVoucherCountAsync()
        {
            using (var context = new M_BMilkStoreDBContext())
            {
                return await context.Vouchers.Where(v => !v.IsDeleted).CountAsync();
            };
        }

        public async Task UpdateVoucherAsync(Voucher voucher)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                var existingVoucher = await context.Vouchers.Include(v => v.ListOrders).FirstOrDefaultAsync(v => v.VoucherId == voucher.VoucherId);
                if (existingVoucher != null)
                {
                    if (existingVoucher.ListOrders != null && existingVoucher.ListOrders.Any(o => o.Status == false))
                    {
                        throw new Exception("Cannot update voucher as there are incomplete orders associated with it.");
                    }

                    context.Entry(existingVoucher).CurrentValues.SetValues(voucher);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Voucher not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<Voucher> GetVoucherByIdAsync(int id)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                return await context.Vouchers.FirstOrDefaultAsync(v => v.VoucherId == id);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteVoucherAsync(int voucherId)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                var existingVoucher = await context.Vouchers.Include(v => v.ListOrders).FirstOrDefaultAsync(v => v.VoucherId == voucherId);
                if (existingVoucher != null)
                {
                    if (existingVoucher.ListOrders != null && existingVoucher.ListOrders.Any(o => o.Status == false))
                    {
                        throw new Exception("Cannot delete voucher as there are incomplete orders associated with it.");
                    }

                    existingVoucher.IsDeleted = true;
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Voucher not found");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Voucher> CreateVoucherAsync(Voucher newVoucher)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                newVoucher.CreationDate = DateTime.UtcNow;
                newVoucher.IsDeleted = false;
                await context.Vouchers.AddAsync(newVoucher);
                await context.SaveChangesAsync();
                return newVoucher;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create voucher: {ex.Message}");
            }
        }

        public async Task<bool> IsValidVoucherAsync(string voucherCode, decimal currentCartTotal)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                var voucher = await context.Vouchers.FirstOrDefaultAsync(v => v.VoucherName == voucherCode && !v.IsDeleted);
                if (voucher == null)
                {
                    return false;
                }
                if (voucher.ExpiryDate < DateTime.UtcNow) return false;
                if (currentCartTotal < voucher.MinimumPrice) return false;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to validate voucher: {ex.Message}");
            }
        }

        public async Task<Voucher> GetVoucherByCodeAsync(string voucherCode)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                return await context.Vouchers.FirstOrDefaultAsync(v => v.VoucherName.Equals(voucherCode) && !v.IsDeleted);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get voucher by code: {ex.Message}");
            }
        }

        public async Task ClaimVoucherAsync(int userId, int voucherId)
        {
            try
            {
                using var context = new M_BMilkStoreDBContext();
                var userVoucher = new UserVoucher
                {
                    UserId = userId,
                    VoucherId = voucherId,
                    RedemptionDate = DateTime.UtcNow
                };

                await context.UserVouchers.AddAsync(userVoucher);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to claim voucher: {ex.Message}");
            }
        }

        public async Task<bool> HasUserAlreadyClaimedVoucherAsync(int userId, int voucherId)
        {
            using var context = new M_BMilkStoreDBContext();
            return await context.UserVouchers.AnyAsync(uv => uv.UserId == userId && uv.VoucherId == voucherId);
        }
    }
}
