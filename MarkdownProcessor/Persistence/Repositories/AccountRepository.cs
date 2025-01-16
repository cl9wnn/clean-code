using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Repositories;

public class AccountRepository(WebDbContext dbContext)
{
    public async Task AddUserAsync(Account account)
    {
        var isAccExists = await IsUserExists(account.Email!);

        if (isAccExists)
        {
            throw new Exception($"User {account.Email} not available");
        }
        await dbContext.Accounts.AddAsync(account);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Account?> GetByEmailAsync(string email)
    {
        return await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.Email == email);
    }

    private async Task<bool> IsUserExists(string email)
    {
        return await dbContext.Accounts.AnyAsync(a => a.Email == email);
    }
}