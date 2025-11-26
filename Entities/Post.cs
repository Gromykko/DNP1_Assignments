using System;

namespace Entities;

public class Post
{
    public Post(string title, string body, int userId)
    {
        Title = title;
        Body = body;
        UserId = userId;
    }

    private Post(){}

    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public User user {get;set;}=null!;
    public int UserId { get; set; }
}
