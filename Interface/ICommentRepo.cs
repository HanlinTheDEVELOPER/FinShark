using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinShark.Dto.Comment;
using FinShark.Model;

namespace FinShark.Interface
{
    public interface ICommentRepo
    {
        Task<List<Comment>> GetAllComments();
        Task<Comment?> GetCommentById(int id);
        Task<Comment> CreateComment(Comment comment);
        Task<Comment?> UpdateComment(int id, UpdateCommentDto updateComment);
        Task<Comment?> DeleteComment(int id);
    }
}