using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinShark.Dto.Account;
using FinShark.Interface;
using FinShark.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.Controller
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                var createdAccount = await _userManager.CreateAsync(appUser, registerDto.Password);
                if (!createdAccount.Succeeded)
                {
                    return StatusCode(500, createdAccount.Errors);
                }
                var addToRole = await _userManager.AddToRoleAsync(appUser, "User");
                if (!addToRole.Succeeded)
                {
                    return StatusCode(500, addToRole.Errors);
                }
                return StatusCode(201, new UserDto
                {
                    Name = appUser.UserName,
                    Email = appUser.Email,
                    Token = _tokenService.CreateToken(appUser)
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized("Invalid email or password");
            var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!checkPassword.Succeeded) return Unauthorized("Invalid email or password");
            return Ok(new UserDto
            {
                Name = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });

        }
    }
}