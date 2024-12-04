using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinShark.Data;
using FinShark.Dto.Comment;
using FinShark.Interface;
using FinShark.Mapper;
using FinShark.Model;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Repository
{
    public class CommentRepo : ICommentRepo
    {
        private readonly AppDbContext _context;

        public CommentRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Comment>> GetAllComments()
        {
            return await _context.Comments.AsNoTracking().ToListAsync();
        }

        public async Task<Comment?> GetCommentById(int id)
        {
            var comment = await _context.Comments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (comment is null)
            {
                return null;
            }
            return comment;
        }

        public async Task<Comment> CreateComment(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;

        }
        public async Task<Comment?> UpdateComment(int id, UpdateCommentDto comment)
        {
            var newComment = comment.FromUpdateCommentDto();
            var existingComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (existingComment is null)
            {
                return null;
            }
            existingComment.Title = newComment.Title;
            existingComment.Content = newComment.Content;
            await _context.SaveChangesAsync();
            return existingComment;
        }

        public async Task<Comment?> DeleteComment(int id)
        {
            var existingComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (existingComment is null)
            {
                return null;
            }
            _context.Comments.Remove(existingComment);
            await _context.SaveChangesAsync();
            return existingComment;
        }
    }
}