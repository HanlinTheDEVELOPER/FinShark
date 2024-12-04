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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Controller
{
    [ApiController]
    [Route("api/stock")]
    public class StockController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IStockRepo _stockRepo;
        public StockController(AppDbContext context, IStockRepo stockRepo)
        {
            _context = context;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryObject queryObject)
        {

            var stocks = await _stockRepo.GetAllStocks(queryObject);
            var stockDtos = stocks.Select(s => s.ToStockDto());
            return Ok(stockDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stock = await _stockRepo.GetStockById(id);
            if (stock is null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock(CreateStockDto stock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Stock newStock = stock.FromStockDto();
            await _stockRepo.CreateStock(newStock);
            return CreatedAtAction(nameof(GetById), new { id = newStock.Id }, newStock.ToStockDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock(int id, UpdateStockDto stock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingStock = await _stockRepo.UpdateStock(id, stock);
            if (existingStock is null)
            {
                return NotFound();
            }
            return Ok(existingStock.ToStockDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var stock = await _stockRepo.DeleteStock(id);
            if (stock is null)
            {
                return NotFound();
            }
            return NoContent();
        }


    }


}