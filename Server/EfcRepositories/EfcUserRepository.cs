using System;
using Entities;
using RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext ctx;

    public EfcUserRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<User> AddAsync(User user)
    {
        await ctx.Users.AddAsync(user);
        await ctx.SaveChangesAsync();
        return user;
    }


    public async Task DeleteAsync(int id)
    {
        User? existing = await ctx.Users.SingleOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            throw new NotFoundException($"User with id {id} not found");
        }

        ctx.Users.Remove(existing);
        await ctx.SaveChangesAsync();
    }


    public IQueryable<User> GetMany()
    {
        return ctx.Users.AsQueryable();
    }


    public async Task<User> GetSingleAsync(int id)
    {
        User? existing = await ctx.Users.SingleOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            throw new NotFoundException($"User with id {id} not found");
        }
        return existing;
    }

    public async Task UpdateAsync(User user)
    {
        if (!await ctx.Users.AnyAsync(p => p.Id == user.Id))
        {
            throw new NotFoundException("User with id {user.Id} not found");
        }

        ctx.Users.Update(user);
        await ctx.SaveChangesAsync();
    }
}