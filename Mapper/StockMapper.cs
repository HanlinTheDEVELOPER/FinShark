using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinShark.Dto.Stock;
using FinShark.Model;

namespace FinShark.Mapper
{
    public static class StockMapper
    {
        public static StockDto ToStockDto(this Stock stock)
        {
            return new StockDto
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                Company = stock.Company,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap,
                Comments = stock.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }
        public static Stock FromStockDto(this CreateStockDto stock)
        {
            return new Stock
            {
                Symbol = stock.Symbol,
                Company = stock.Company,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap
            };
        }
    }
}