using System;

namespace Entities;

public class Comment
{
    public Comment(int postId, int userId, string body)
    {
        PostId = postId;
        UserId = userId;
        Body = body;
    }

    private Comment(){}

    public int Id { set; get; }
    public string? Body { set; get; }
    public User user {set;get;}
    public int UserId { set; get; }
    public Post post {set;get;}=null!;
    public int PostId { set; get; }
}
