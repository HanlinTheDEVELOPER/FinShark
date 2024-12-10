using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinShark.Data;
using FinShark.Interface;
using FinShark.Model;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Repository
{
    public class PortfolioRepo : IPortfolioRepo
    {
        private readonly AppDbContext _context;
        public PortfolioRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreatePortfolio(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio?> DeletePortfolio(AppUser user, string symbol)
        {
            var portfolio = await _context.Portfolios.FirstOrDefaultAsync(p => p.AppUserId == user.Id && p.Stock.Symbol.ToLower() == symbol.ToLower());
            if (portfolio is null)
            {
                return null;
            }
            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where(p => p.AppUserId == user.Id).
            Select(stock => new Stock
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                Company = stock.Stock.Company,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                MarketCap = stock.Stock.MarketCap,
                Industry = stock.Stock.Industry,
            }).ToListAsync();
        }

        public async Task<bool> IsPortfolioExist(Portfolio portfolio)
        {
            return await _context.Portfolios.AnyAsync(p => p.AppUserId == portfolio.AppUserId && p.StockId == portfolio.StockId);
        }
    }
}