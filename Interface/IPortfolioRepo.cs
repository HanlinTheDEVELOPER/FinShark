using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinShark.Model;

namespace FinShark.Interface
{
    public interface IPortfolioRepo
    {
        Task<List<Stock>> GetPortfolio(AppUser user);
    }
}