using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var userPortfolio = await _portfolioRepo.GetPortfolio(user);
            return Ok(userPortfolio);
        }
    }
}