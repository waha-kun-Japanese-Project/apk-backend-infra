using Community.Domain.Contracts;
using Community.ServiceAbstraction;
using Microsoft.AspNetCore.Connections;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Service;

public class CountService(
    ICommentRepo commentRepo,
    IConnectionMultiplexer redis,
    IIssueShareRepo issueShareRepo,
    IIssueVoteRepo issueVoteRepo) : ICountService
{
    private IDatabase Db => redis.GetDatabase();
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(10);


    private static string VoteKey(Guid issueId) => $"issue:{issueId}:votecount";
    private static string ShareKey(Guid issueId) => $"issue:{issueId}:sharecount";
    private static string CommentKey(Guid issueId) => $"issue:{issueId}:CommentCount";

    public async Task<long> GetVoteCountAsync(Guid issueId)
    => await GetOrInitAsync(VoteKey(issueId), () => issueVoteRepo.GetCountByIssueIdAsync(issueId));

    public async Task<long> GetShareCountAsync(Guid issueId)
        => await GetOrInitAsync(ShareKey(issueId), () => issueShareRepo.GetCountByIssueIdAsync(issueId));
    public async Task<long> GetCommentCountAsync(Guid issueId)
        => await GetOrInitAsync(CommentKey(issueId), () => commentRepo.GetCountByIssueIdAsync(issueId));

    public async Task<long> IncrementVoteAsync(Guid issueId)
    {
        var newValue = await Db.StringIncrementAsync(VoteKey(issueId));
        await Db.KeyExpireAsync(VoteKey(issueId), CacheTtl); // refresh the timer
        return newValue;
    }

    public async Task<long> DecrementVoteAsync(Guid issueId)
    {
        var newValue = await Db.StringDecrementAsync(VoteKey(issueId));
        await Db.KeyExpireAsync(VoteKey(issueId), CacheTtl); // refresh the timer
        return newValue;
    }

    public async Task<long> IncrementShareAsync(Guid issueId)
    {
        var newValue = await Db.StringIncrementAsync(ShareKey(issueId));
        await Db.KeyExpireAsync(ShareKey(issueId), CacheTtl);
        return newValue;
    }

    public async Task<long> IncreamentCommentAsync(Guid issueId )
    {
        var newValue = await Db.StringIncrementAsync(CommentKey(issueId));
        await Db.KeyExpireAsync(CommentKey(issueId), CacheTtl);
        return newValue;
    }

    
    public async Task<long> DecrementCommentAsync(Guid issueId)
    {
        var newValue = await Db.StringDecrementAsync(CommentKey(issueId));
        await Db.KeyExpireAsync(CommentKey(issueId), CacheTtl); // refresh the timer
        return newValue;
    }
    private async Task<long> GetOrInitAsync(string key, Func<Task<int>> loadFromDb)
    {
        var value = await Db.StringGetAsync(key);
        if (value.HasValue)
            return (long)value;

        var dbCount = await loadFromDb();
        await Db.StringSetAsync(key, dbCount, expiry: CacheTtl, when: When.NotExists);
        return dbCount;
    }
}


