using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinShark.Dto.Stock;
using FinShark.Helper;
using FinShark.Model;

namespace FinShark.Interface
{
    public interface IStockRepo
    {
        Task<List<Stock>> GetAllStocks(QueryObject queryObject);
        Task<Stock?> GetStockById(int id);
        Task<Stock> CreateStock(Stock createStock);
        Task<Stock?> UpdateStock(int id, UpdateStockDto updateStock);
        Task<Stock?> DeleteStock(int id);
        Task<bool> IsStockExist(int id);
    }
}