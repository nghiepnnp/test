namespace Domain.DTOs
{
    public class CommentDto
    {
        public Guid ProductId { get; set; }

        public string? Content { get; set; }

        public string? UserName { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
