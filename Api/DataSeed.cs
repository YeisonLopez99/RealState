using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

public static class DataSeed
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();

        if (!db.Owners.Any())
        {
            var (salt, hash) = HashPassword("Pa$$w0rd"); // demo password
            var stored = Convert.ToBase64String(salt.Concat(hash).ToArray());
            var owner = new Domain.Entities.Owner("Demo Owner", "owner@example.com", stored, "3000000000", "Demo address");
            db.Owners.Add(owner);
            await db.SaveChangesAsync();
        }
    }

    private static (byte[] salt, byte[] hash) HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        using var derive = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
        var hash = derive.GetBytes(32);
        return (salt, hash);
    }
}