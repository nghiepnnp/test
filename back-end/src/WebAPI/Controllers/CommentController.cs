using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.DTOs;
using WebAPI.Utilities;

namespace WebAPI.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddComment(CommentDto comment)
        {
            try
            {
                var result = await _commentService.AddCommentAsync(comment);

                return Ok(ApiResult.Success(result!));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult.Failure(null!, ex.Message));
            }
        }
    }
}
