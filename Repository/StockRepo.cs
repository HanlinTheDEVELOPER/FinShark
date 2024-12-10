using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinShark.Data;
using FinShark.Dto.Stock;
using FinShark.Helper;
using FinShark.Interface;
using FinShark.Mapper;
using FinShark.Model;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Repository
{
    public class StockRepo : IStockRepo
    {
        private readonly AppDbContext _context;
        public StockRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Stock>> GetAllStocks(QueryObject queryObject)
        {
            var stocks = _context.Stocks.AsNoTracking().Include(s => s.Comments).ThenInclude(c => c.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject.Symbol) || !string.IsNullOrWhiteSpace(queryObject.Company))
            {
                stocks = stocks.Where(s =>
                    (!string.IsNullOrWhiteSpace(queryObject.Symbol) && s.Symbol.Contains(queryObject.Symbol)) ||
                    (!string.IsNullOrWhiteSpace(queryObject.Company) && s.Company.Contains(queryObject.Company)));
            }
            if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
            {
                switch (queryObject.SortBy.ToLower())
                {
                    case "symbol":
                        stocks = queryObject.IsDecending
                            ? stocks.OrderByDescending(s => s.Symbol)
                            : stocks.OrderBy(s => s.Symbol);
                        break;
                    case "company":
                        stocks = queryObject.IsDecending
                            ? stocks.OrderByDescending(s => s.Company)
                            : stocks.OrderBy(s => s.Company);
                        break;
                    case "purchase":
                        stocks = queryObject.IsDecending
                            ? stocks.OrderByDescending(s => s.Purchase)
                            : stocks.OrderBy(s => s.Purchase);
                        break;
                    case "lastdiv":
                        stocks = queryObject.IsDecending
                        ? stocks.OrderByDescending(s => s.LastDiv)
                        : stocks.OrderBy(s => s.LastDiv);
                        break;
                    case "industry":
                        stocks = queryObject.IsDecending
                        ? stocks.OrderByDescending(s => s.Industry)
                        : stocks.OrderBy(s => s.Industry);
                        break;
                    default:
                        stocks = queryObject.IsDecending
                            ? stocks.OrderByDescending(s => s.MarketCap)
                            : stocks.OrderBy(s => s.MarketCap);
                        break;
                }
            }

            stocks = stocks.Skip((queryObject.PageNumber - 1) * queryObject.PageSize).Take(queryObject.PageSize);
            var stocksList = await stocks.ToListAsync();
            return stocksList;
        }

        public async Task<Stock?> GetStockById(int id)
        {
            var stock = await _context.Stocks.AsNoTracking().Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == id);
            if (stock is null)
            {
                return null;
            }
            return stock;
        }
        public async Task<bool> IsStockExist(int id)
        {
            return await _context.Stocks.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock> CreateStock(Stock stock)
        {

            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> UpdateStock(int id, UpdateStockDto updateStock)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (existingStock is null)
            {
                return null;
            }
            existingStock.Symbol = updateStock.Symbol;
            existingStock.Company = updateStock.Company;
            existingStock.Purchase = updateStock.Purchase;
            existingStock.LastDiv = updateStock.LastDiv;
            existingStock.Industry = updateStock.Industry;
            existingStock.MarketCap = updateStock.MarketCap;

            await _context.SaveChangesAsync();
            return existingStock;
        }
        public async Task<Stock?> DeleteStock(int id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stock is null)
            {
                return null;
            }
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> GetStockBySymbol(string symbol)
        {
            var stock = await _context.Stocks.AsNoTracking().FirstOrDefaultAsync(s => s.Symbol == symbol);
            return stock;
        }
    }
}


