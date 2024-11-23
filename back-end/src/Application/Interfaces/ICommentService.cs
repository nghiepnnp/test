using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICommentService
    {
        public Task<IEnumerable<Comment>> GetAllCommentByProductIdAsync(Guid productId);
        public Task<Comment?> AddCommentAsync(CommentDto comment);
    }
}
