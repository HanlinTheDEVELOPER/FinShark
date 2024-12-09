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
        public async Task<List<Stock>> GetPortfolio(AppUser user)
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
    }
}