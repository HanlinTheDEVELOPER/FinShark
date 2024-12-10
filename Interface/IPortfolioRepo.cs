using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinShark.Model;

namespace FinShark.Interface
{
    public interface IPortfolioRepo
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<Portfolio> CreatePortfolio(Portfolio portfolio);
        Task<Portfolio?> DeletePortfolio(AppUser user, string symbol);
        Task<bool> IsPortfolioExist(Portfolio portfolio);
    }
}