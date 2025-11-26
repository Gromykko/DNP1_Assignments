using System;
using Entities;
using RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext ctx;

    public EfcCommentRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        await ctx.Comments.AddAsync(comment);
        await ctx.SaveChangesAsync();
        return comment;
    }


    public async Task DeleteAsync(int id)
    {
        Comment? existing = await ctx.Comments.SingleOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            throw new NotFoundException($"Comment with id {id} not found");
        }

        ctx.Comments.Remove(existing);
        await ctx.SaveChangesAsync();
    }


    public IQueryable<Comment> GetMany()
    {
        return ctx.Comments.AsQueryable();
    }


    public async Task<Comment> GetSingleAsync(int id)
    {
        Comment? existing = await ctx.Comments.SingleOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            throw new NotFoundException($"Comment with id {id} not found");
        }
        return existing;
    }

    public async Task UpdateAsync(Comment comment)
    {
        if (!await ctx.Comments.AnyAsync(p => p.Id == comment.Id))
        {
            throw new NotFoundException("Comment with id {comment.Id} not found");
        }

        ctx.Comments.Update(comment);
        await ctx.SaveChangesAsync();
    }
}