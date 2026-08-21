using Auth.Domain.Contracts;
using StackExchange.Redis;

namespace Auth.Persistence.Repositories;

public  class OtpRepository(
    IConnectionMultiplexer redis)
    : IOtpRepository
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task SaveOtpAsync( string phoneNumber,string code)
    {
        await _database.StringSetAsync(  $"otp:{phoneNumber}",  code,  TimeSpan.FromMinutes(5));
    }

    public async Task<string?> GetOtpAsync(
        string phoneNumber)
    {
        var value = await _database.StringGetAsync(
            $"otp:{phoneNumber}");

        return value.HasValue
            ? value.ToString()
            : null;
    }

    public async Task RemoveOtpAsync(string phoneNumber)
    {
        await _database.KeyDeleteAsync(
            $"otp:{phoneNumber}");
    }
}