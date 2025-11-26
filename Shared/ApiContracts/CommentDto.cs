using System;

namespace ApiContracts;

public class CommentDto
{
    public required string Body { set; get; }
    public required int UserId { set; get; }
    public required int PostId { set; get; }
    public required int Id{ set; get; }
}
