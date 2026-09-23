using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Persistence;
using User.Persistence.Context;
using User.ServicesAbstract;
using UserService.Grpc;
namespace UserGrpcService.Services;

public class UserGRPsService : UserGRPcService.UserGRPcServiceBase
{
    private readonly AppDbContext _context;

    public UserGRPsService(AppDbContext context)
    {
        _context = context;
    }

    public override async Task<GetUsersResponse> GetUsersByIds(
        GetUsersRequest request,
        ServerCallContext context)
    {
        var userIds = request.UserIds
            .Select(Guid.Parse)
            .ToList();

        var users = await _context.Users
            .Where(x => userIds.Contains(x.Id))
            .Select(x => new UserResponse
            {
                Id = x.Id.ToString(),
                Name = x.FullName,
                PhotoUrl = x.pictures ?? ""
            })
            .ToListAsync(context.CancellationToken);

        var response = new GetUsersResponse();

        response.Users.AddRange(users);

        return response;
    }
}