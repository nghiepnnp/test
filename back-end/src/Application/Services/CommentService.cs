using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Hubs;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<CommentService> _logger;
        private readonly IHubContext<CommentHub> _hubContext;

        public CommentService(AppDbContext dbContext,
            ILogger<CommentService> logger,
            IHubContext<CommentHub> hubContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task<Comment?> AddCommentAsync(CommentDto comment)
        {
            try
            {
                var newComment = new Comment
                {
                    ProductId = comment.ProductId,
                    Content = comment.Content,
                    UserName = comment.UserName
                };

                _dbContext.Comments.Add(newComment);

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    // Đang push toàn bộ user, 
                    // Chưa có lưu lại từng connectionId, nên mở nhiều site có thể push cùng lúc nhiều lần
                    await _hubContext.Clients.All.SendAsync("ReceiveComment", new
                    {
                        newComment
                    });
                }

                return await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == newComment.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred.");
                throw;
            }
        }

        public async Task<IEnumerable<Comment>> GetAllCommentByProductIdAsync(Guid id)
        {
            try
            {
                return await _dbContext.Comments
                    .Where(x => x.ProductId == id)
                    .OrderByDescending(x => x.CreatedAt).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred.");
                throw;
            }
        }
    }
}
