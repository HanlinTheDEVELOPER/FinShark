using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinShark.Dto.Comment;
using FinShark.Extension;
using FinShark.Interface;
using FinShark.Mapper;
using FinShark.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.Controller
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepo _commentRepo;
        private readonly IStockRepo _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(ICommentRepo commentRepo, IStockRepo stockRepo, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {

            var commentsList = await _commentRepo.GetAllComments();
            List<CommentDto> comments = commentsList.Select(c => c.ToCommentDto()).ToList();
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var comment = await _commentRepo.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId}")]
        [Authorize]
        public async Task<IActionResult> CreateComment(int stockId, CreateCommentDto createCommentDto)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user is null) return Unauthorized();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _stockRepo.IsStockExist(stockId))
            {
                return BadRequest();
            }
            var comment = createCommentDto.FromCommentDto(stockId);
            comment.AppUserId = user.Id;
            var newComment = await _commentRepo.CreateComment(comment);
            return CreatedAtAction(nameof(Get), new { id = newComment.Id }, newComment.ToCommentDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, UpdateCommentDto updateCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await _commentRepo.UpdateComment(id, updateCommentDto);
            if (comment is null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentRepo.DeleteComment(id);
            if (comment is null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}