using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinShark.Dto.Portfolio;
using FinShark.Extension;
using FinShark.Interface;
using FinShark.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.Controller
{
    [ApiController]
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepo _stockRepo;
        private readonly IPortfolioRepo _portfolioRepo;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepo stockRepo, IPortfolioRepo portfolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetPortfolio()
        {

            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
            {
                return NotFound("User not found");
            }
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(user);
            return Ok(userPortfolio);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> CreatePortfolio(PortfolioReqDto portfolioReqDto)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepo.GetStockBySymbol(portfolioReqDto.Symbol);
            if (stock is null) return NotFound("Stock not found");
            var newPortfolio = new Portfolio
            {
                AppUserId = user.Id,
                StockId = stock.Id,
            };
            if (await _portfolioRepo.IsPortfolioExist(newPortfolio)) return BadRequest("Stock already exist in portfolio");
            var userPortfolio = await _portfolioRepo.CreatePortfolio(newPortfolio);
            return Created();
        }

        [HttpDelete("{symbol}")]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(user);
            if (!userPortfolio.Any(s => s.Symbol.ToLower() == symbol.ToLower())) return NotFound("Stock not found in your portfolio");
            var portfolio = await _portfolioRepo.DeletePortfolio(user, symbol);
            if (portfolio is null) return NotFound("Portfolio not found");
            return Ok("Portfolio deleted");
        }
    }
}