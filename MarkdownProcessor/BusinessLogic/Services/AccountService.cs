using Microsoft.AspNetCore.Identity;
using Persistence.Entities;
using Persistence.Repositories;

namespace BusinessLogic.Services;

public class AccountService(AccountRepository accountRepository, JwtService jwtService)
{
    public async Task RegisterAsync(string email, string password, string firstName)
    {
        var account = new Account
        {
            Email = email,
            PasswordHash = password,
            FirstName = firstName
        };
        
        var passwordHash = new PasswordHasher<Account>().HashPassword(account, password);
        account.PasswordHash = passwordHash;

        await accountRepository.AddUserAsync(account);
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var account = await accountRepository.GetByEmailAsync(email);
        
        if (account == null)
            return null;
        
        var result = new PasswordHasher<Account>().VerifyHashedPassword(account, account.PasswordHash, password);

        if (result == PasswordVerificationResult.Success)
        {
            return jwtService.GenerateJwtToken(account);
        }

        return null;
    }
}